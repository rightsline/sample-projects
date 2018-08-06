package com.rightsline;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;

public class RestClient {

    public static String BaseConnectionString = "http://api-qa.rightsline.com/v2/";
    static String CatalogItem = "catalog-item/";
    static String DemoTableItem = "table/";
    static HttpURLConnection client;

    public static void DemoMethod(){
        System.out.println(SetUpClient("GET", BaseConnectionString, CatalogItem));
    }

    public static String SetUpClient(String httpMethod, String targetURL, String entityType){
        try {
            //Create client
            URL url = new URL(targetURL + entityType);
            client = (HttpURLConnection) url.openConnection();
            client.setRequestMethod(httpMethod);
            client.setRequestProperty("Content-Type",
                    "application/json");
            client.setRequestProperty("Content-Language", "en-US");
            client.setRequestProperty("Authorization", ConfigSetup.getBasicAuth());
            client.setUseCaches(false);
            client.setDoOutput(true);

            //Send request
            DataOutputStream wr = new DataOutputStream (
                    client.getOutputStream());
            wr.close();

            client.connect();
            //Get Response  
            InputStream is = client.getInputStream();
            BufferedReader rd = new BufferedReader(new InputStreamReader(is));
            StringBuilder response = new StringBuilder();
            String line;
            while ((line = rd.readLine()) != null) {
                response.append(line);
                response.append('\r');
            }
            rd.close();
            return response.toString();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        } finally {
            if (client != null) {
                client.disconnect();
            }
        }
    }
}
