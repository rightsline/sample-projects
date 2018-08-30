package com.rightsline;

import auth.AWS4SignerBase;
import auth.AWS4SignerForAuthorizationHeader;
import auth.AWS4SignerForQueryParameterAuth;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import util.BinaryUtils;
import util.HttpUtils;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

public class RestClient {

    public static String BaseConnectionString = "https://api-dev.rightsline.com/v3/";


    static HttpURLConnection client;
    static String apiEndpointUrl = "https://api-dev.rightsline.com/v3/auth/temporary-credentials";
    static String json = "{\n\t\"accessKey\":\"" + ConfigSetup.getCredentials().get("accessKey") + "\",\n\t\"secretKey\":\"" + ConfigSetup.getCredentials().get("secretKey") + "\"\n}";
    static Map<String, String> initialHeaders = new HashMap<String, String>() {{
        put("x-api-key", ConfigSetup.getCredentials().get("xApiKey"));
        put("content-type", "application/json");
    }};


    //These are example JSON files that we have created
    //There are fields in the Feature example that are currently not working and will need to be removed
    private static String CatalogItemEpisodePostExampleJson = "./Catalog Item Example JSON/CatalogItemEpisodePOST.json";
    private static String CatalogItemEpisodePutExampleJson = "./Catalog Item Example JSON/CatalogItemEpisodePUT.json";
    private static String CatalogItemFeaturePostExampleJson = "./Catalog Item Example JSON/CatalogItemEpisodePOST.json";
    private static String RelationshipPostExampleJson = "./Relationship Example JSON/RelationshipPost.json";
    private static String TablePostExampleJson = "./Table Example JSON/TablePostExample.json";

    public static void DemoMethod() {

//        System.out.println(GetRequestDemoMethod("catalog-item", "1541"));
        String newId = PostEntityDemoMethod("catalog-item", CatalogItemEpisodePostExampleJson);
        System.out.println(newId);
//        System.out.println(UpdateEntityDemoMethod("catalog-item", newId, CatalogItemEpisodePutExampleJson));
//        System.out.println(DeleteEntityDemoMethod("catalog-item", "1557"));
//        DeleteEntityDemoMethod("catalog-Item", newId);
    }

    private static HashMap<String, String> getApiResponse() throws MalformedURLException {
        String response = HttpUtils.invokeHttpRequest(new URL(apiEndpointUrl), "POST", initialHeaders, json);
        System.out.println(response);

        JsonObject awsCredentials = new JsonParser().parse(response).getAsJsonObject();

        HashMap<String, String> awsCreds = new HashMap<>();
        awsCreds.put("accessKey", awsCredentials.get("accessKey").getAsString());
        awsCreds.put("secretKey", awsCredentials.get("secretKey").getAsString());
        awsCreds.put("sessionToken", awsCredentials.get("sessionToken").getAsString());
        awsCreds.put("expiration", awsCredentials.get("expiration").getAsString());

        return awsCreds;
    }

//    public static String PostEntityDemoMethod() {
//        String newId = PostEntityDemoMethod("catalog-item", CatalogItemEpisodePostExampleJson);
//        System.out.println("The ID for the newest created catalog-item is: " + newId);
//        return newId;
//    }
//
//    public static void PostCatalogItemFeatureDemoMethod() {
//        String newId = PostEntityDemoMethod("catalog-item", CatalogItemFeaturePostExampleJson);
//        System.out.println("The ID for the newest created catalog-item is: " + newId);
//    }
//
//    public static void PostTableDemoMethod() {
//        String newId = PostEntityDemoMethod("table", TablePostExampleJson);
//        System.out.println("The ID for the newest created catalog-item is: " + newId);
//    }
//
//    public static void PostRelationshipDemoMethod() {
//        String newId = PostEntityDemoMethod("relationship", RelationshipPostExampleJson);
//        System.out.println("The ID for the newest created catalog-item is: " + newId);
//    }

    /**
     * Valid EntityTypes are Catalog-Item, Table, Contact, Rightset, Deal
     *
     * @param entityType
     * @param itemId
     */
    public static String DeleteEntityDemoMethod(String entityType, String itemId) {
        try {
            HashMap<String, String> awsCreds = getApiResponse();

            //Create client
            URL url = new URL(BaseConnectionString + entityType + "/" + itemId);
            AWS4SignerForAuthorizationHeader auth = new AWS4SignerForAuthorizationHeader(url, "DELETE", "execute-api", "us-east-1");
            Map<String, String> headers = new HashMap<>();
            headers.put("content-type", "application/json");
            headers.put("x-amz-security-token", awsCreds.get("sessionToken"));
            headers.put("x-api-key", ConfigSetup.getCredentials().get("xApiKey"));


            String authorization = auth.computeSignature(headers, new HashMap<>(), AWS4SignerBase.EMPTY_BODY_SHA256, awsCreds.get("accessKey"), awsCreds.get("secretKey"));
            client = (HttpURLConnection) url.openConnection();

//            client.setRequestProperty("Authorization", authorization);
            headers.put("Authorization", authorization);
            System.out.println("AUTH: " + authorization);
            client.setRequestMethod("DELETE");

            client.connect();
            String response = HttpUtils.invokeHttpRequest(url, "DELETE", headers, null);
            return response;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        } finally {
            if (client != null) {
                client.disconnect();
            }
        }
    }

    /**
     * Valid EntityTypes are Catalog-Item, Table, Contact, Rightset, Deal
     * Returns the updated entity's information in xml string format
     *
     * @param EntityType
     * @param EntityId
     * @param jsonFilePath
     * @return
     */
    public static String UpdateEntityDemoMethod(String EntityType, String EntityId, String jsonFilePath) {
//        try {
//            //Create client
//            URL url = new URL(BaseConnectionString + EntityType + "/" + EntityId);
//            client = (HttpURLConnection) url.openConnection();
//            client.setRequestMethod("PUT");
//            client.setRequestProperty("Content-Type", "application/json");
//            client.setDoOutput(true);
//            client.setRequestProperty("Authorization", ConfigSetup.getCredentials());
//
//            //Attach the JSON string for the entity you wish to create
//            //We have specified an example file path but this is changeable
//            String folderPath = System.getProperty("user.dir") + jsonFilePath;
//            String jsonBody = ReadFile(folderPath);
//            System.out.println(jsonBody);
//            OutputStream stream = client.getOutputStream();
//            OutputStreamWriter wr = new OutputStreamWriter(stream, "UTF-8");
//            wr.write(jsonBody);
//            wr.flush();
//            wr.close();
//            stream.close();
//            client.connect();
//
//            //Get the response
//            BufferedReader bufferedReader = new BufferedReader(
//                    new InputStreamReader(client.getInputStream()));
//            String result;
//            //Append each line
//            StringBuffer response = new StringBuffer();
//            while ((result = bufferedReader.readLine()) != null) {
//                response.append(result);
//            }
//            bufferedReader.close();
//            return response.toString();
//        } catch (Exception e) {
//            e.printStackTrace();
//            return null;
//        } finally {
//            if (client != null) {
//                client.disconnect();
//            }
//        }
        return "";
    }

    /**
     * Creates an HTTP GET request and returns a JSON string
     * Valid EntityTypes are Catalog-Item, Table, Contact, Rightset, Deal,
     * Returns the entity in XML string format
     */
    public static String GetRequestDemoMethod(String entityType, String itemId) {
        try {
            HashMap<String, String> awsCreds = getApiResponse();

            //Create client
            URL url = new URL(BaseConnectionString + entityType + "/" + itemId);
            AWS4SignerForAuthorizationHeader auth = new AWS4SignerForAuthorizationHeader(url, "GET", "execute-api", "us-east-1");
            Map<String, String> headers = new HashMap<>();
            headers.put("content-type", "application/json");
            headers.put("x-amz-security-token", awsCreds.get("sessionToken"));
            headers.put("x-api-key", ConfigSetup.getCredentials().get("xApiKey"));


            String authorization = auth.computeSignature(headers, new HashMap<>(), AWS4SignerBase.EMPTY_BODY_SHA256, awsCreds.get("accessKey"), awsCreds.get("secretKey"));
            client = (HttpURLConnection) url.openConnection();

//            client.setRequestProperty("Authorization", authorization);
            headers.put("Authorization", authorization);
            System.out.println("AUTH: " + authorization);
            client.setRequestMethod("GET");

            client.connect();
            String response = HttpUtils.invokeHttpRequest(url, "GET", headers, null);
            return response;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        } finally {
            if (client != null) {
                client.disconnect();
            }
        }
    }

    /**
     * Method to send a Post Request for a new entity to be created.
     * Valid EntityTypes are Catalog-Item, Table, Contact, Rightset, Deal
     * Returns the id of the created entity
     */

    public static String PostEntityDemoMethod(String entityType, String jsonFilePath) {

        try {
            HashMap<String, String> awsCreds = getApiResponse();

            //Create client
            URL url = new URL(BaseConnectionString + entityType);
            AWS4SignerForAuthorizationHeader auth = new AWS4SignerForAuthorizationHeader(url, "POST", "execute-api", "us-east-1");
            Map<String, String> headers = new HashMap<>();
            headers.put("content-type", "application/json");
            headers.put("x-amz-security-token", awsCreds.get("sessionToken"));
            headers.put("x-api-key", ConfigSetup.getCredentials().get("xApiKey"));
            File file = new File(jsonFilePath);
            Charset charset = Charset.forName("UTF-8");

            String jsonFile1 = String.join("\n", Files.readAllLines(file.toPath()));
            System.out.println(jsonFile1.substring(1));
            System.out.println(" Hash: " + (AWS4SignerBase.bytesToHex(AWS4SignerBase.hash(jsonFile1.substring(1)))));
//
            String authorization = auth.computeSignature(headers, null, BinaryUtils.toHex(AWS4SignerBase.hash(jsonFile1.substring(1))), awsCreds.get("accessKey"), awsCreds.get("secretKey"));
            client = (HttpURLConnection) url.openConnection();

//            client.setRequestProperty("Authorization", authorization);
            headers.put("Authorization", authorization);
            client.setRequestMethod("POST");

            client.connect();
            String response = HttpUtils.invokeHttpRequest(url, "POST", headers, jsonFile1.substring(1));
            return response;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        } finally {
            if (client != null) {
                client.disconnect();
            }
        }
    }

    /**
     * Reads all the text in a file and returns it as a string. Used for reading JSON files
     */
    private static String ReadFile(String filePath) {
        StringBuilder sb = new StringBuilder();
        try {
            BufferedReader r = new BufferedReader(new FileReader(filePath));
            String line;
            while ((line = r.readLine()) != null) {
                sb.append(line);
            }
        } catch (Exception e) {
            System.out.println(e.getStackTrace());
        }
        return sb.toString();

    }
}
