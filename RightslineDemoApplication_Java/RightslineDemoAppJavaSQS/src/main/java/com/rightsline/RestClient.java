package com.rightsline;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.rightsline.AWSSigner.AWS4SignerBase;
import com.rightsline.AWSSigner.AWS4SignerForAuthorizationHeader;
import com.rightsline.AWSSigner.HttpUtils;


import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Timer;
import java.util.TimerTask;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class RestClient {
    private static String BaseUrl = "https://sqs.us-west-2.amazonaws.com/";
    private static HashMap<String, String> config = ConfigSetup.getConfigFile();
    private static int SecondsToPoll = 5;
    private static String individualMessageRegex = "\\{(.*?)\\}<";
    private static String receiptRegex = "ReceiptHandle>(.*?)<\\/Receipt";
    private static String entityRegex = "v2\\/(.*?)\\/";
    private static String MessagesToReceieve = "10";


    public static void DemoMethod(){
        String messages = getSQSMessages();
        ArrayList<JsonObject> jsonObjects = FilterXml(messages);

        ArrayList<String> receipts = FilterReceiptHandles(messages);
        DeleteMessage(receipts);
        Notify(jsonObjects, "catalog-item");
    }

    public static String getSQSMessages() {
        String region = config.get("Region");
        String endpointUri = BaseUrl + config.get("AccountId") + "/" + config.get("QueueName");
        String requestParameters = "Action=ReceiveMessage&MaxNumberOfMessages=" + MessagesToReceieve;
        HashMap<String, String> requestParams = new HashMap()
        {
            {
                put("Action", "ReceiveMessage");
                put("MaxNumberOfMessages", MessagesToReceieve);
            }
        };
        URL url = null;
        try {
            url = new URL(endpointUri + "?" + requestParameters);
        }
        catch(Exception e){
            e.printStackTrace();
        }
        HashMap headers = new HashMap<String, String>();
        headers.put(AWS4SignerBase.X_Amz_Content_SHA256, AWS4SignerBase.EMPTY_BODY_SHA256);
        AWS4SignerForAuthorizationHeader signer = new AWS4SignerForAuthorizationHeader(url, "GET", "sqs", region);
        String authorization = signer.computeSignature(headers, requestParams, AWS4SignerBase.EMPTY_BODY_SHA256, config.get("AccessKey"), config.get("SecretKey"));
        headers.put("Authorization", authorization);
        String response = HttpUtils.invokeHttpRequest(url, "GET", headers, null);
        return response;
    }
    public static void StartBackgroundMonitoring(){
        TimerTask PollTask = new TimerTask(){
            public void run(){
                String messages = getSQSMessages();
                ArrayList<JsonObject> jsonObjects = FilterXml(messages);
                Notify(jsonObjects, "catalog-item");
                Runnable deleteMessagesThread = () ->{
                    ArrayList<String> receipts = FilterReceiptHandles(messages);
                    DeleteMessage(receipts);
                };
                new Thread(deleteMessagesThread).start();
            }
        };
        Timer t = new Timer();
        t.schedule(PollTask, 2000, SecondsToPoll * 1000);
    }
    public static ArrayList<JsonObject> FilterXml(String XMLMessage){
        ArrayList<JsonObject> filteredMessages = new ArrayList<>();
        Pattern xmlFilter = Pattern.compile(individualMessageRegex);
        Matcher m = xmlFilter.matcher(XMLMessage);
        while(m.find()){
            //replace the &quot; with actual quotes from the XML message
            String match = m.group(0).replace("&quot;", "\"");
            filteredMessages.add(new JsonParser().parse(match.substring(0, match.length() - 1)).getAsJsonObject());
        }
        return filteredMessages;
    }
    public static ArrayList<String> FilterReceiptHandles(String XMLMessage){
        Pattern receiptFilter = Pattern.compile(receiptRegex);
        Matcher m = receiptFilter.matcher(XMLMessage);
        ArrayList<String> receipts = new ArrayList<>();
        while(m.find()){
//            try{

                receipts.add(m.group(1));
//            }
//            catch(Exception e){
//                e.printStackTrace();
//            }
        }
        return receipts;
    }
    private static void Notify(ArrayList<JsonObject> messages, String entitySearch){
        System.out.println("Poll at " + java.time.LocalDateTime.now());
        int numMessages = 0;
        Pattern entityPattern = Pattern.compile(entityRegex);
        for(JsonObject message : messages) {
            String entityUrl = message.get("entityUrl").toString();
            Matcher m = entityPattern.matcher(entityUrl);
            while(m.find()){
                String entityFound = m.group(1);
                if(entityFound.equals(entitySearch)){
                    numMessages++;
                    System.out.println("A(n) " + entityFound + " was " + message.get("action").toString() + ", URL:" + message.get("entityUrl"));
                }
            }
        }
        if(numMessages == 0){
            System.out.println("Recieved "+ messages.size() + " messages No messages regarding " + entitySearch + " were found.");
        }
    }

    /**
     *  Sends a request to Amazon to delete a batch of receipt handles for the messages received
      * @param receiptHandles
     */
    private static void DeleteMessage(ArrayList<String> receiptHandles){
        for(String receipt : receiptHandles){
            String region = config.get("Region");
            String endpointUri = BaseUrl + config.get("AccountId") + "/" + config.get("QueueName");
            String encodedReceipt = "";
            try {
                encodedReceipt = URLEncoder.encode(receipt, "UTF-8");
            } catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
            //Receipt handle needs to be encoded for the URL but not in the requestParams hashmap
            String requestParameters = "Action=DeleteMessage&ReceiptHandle=" + encodedReceipt;
            HashMap<String, String> requestParams = new HashMap()
            {
                {
                    put("Action", "DeleteMessage");
                    put("ReceiptHandle", receipt);
                }
            };
            URL url = null;
            try {
                url = new URL(endpointUri + "?" + requestParameters);
            }
            catch(Exception e){
                e.printStackTrace();
            }
            HashMap headers = new HashMap<String, String>();
            headers.put(AWS4SignerBase.X_Amz_Content_SHA256, AWS4SignerBase.EMPTY_BODY_SHA256);
            AWS4SignerForAuthorizationHeader signer = new AWS4SignerForAuthorizationHeader(url, "GET", "sqs", region);
            String authorization = signer.computeSignature(headers, requestParams, AWS4SignerBase.EMPTY_BODY_SHA256, config.get("AccessKey"), config.get("SecretKey"));
            headers.put("Authorization", authorization);
            String response = HttpUtils.invokeHttpRequest(url, "GET", headers, null);
            //System.out.println(response);
        }
    }
}
