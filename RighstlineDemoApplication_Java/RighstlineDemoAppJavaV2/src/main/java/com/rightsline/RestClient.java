package com.rightsline;
import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;

public class RestClient {

    public static String BaseConnectionString = "https://api-qa.rightsline.com/v2/";
    static String CatalogItem = "catalog-item/";
    static String DemoTableItem = "table/";
    static HttpURLConnection client;

    public static void DemoMethod(){
        System.out.println(SetUpClient("GET", BaseConnectionString, CatalogItem, "1581"));
    }

    public static String SetUpClient(String httpMethod, String targetURL, String entityType, String itemId){
        try {
            //Create client
            URL url = new URL(targetURL + entityType + itemId);
            System.out.println(url.toString());
            client = (HttpURLConnection) url.openConnection();
            client.setRequestMethod(httpMethod);
            client.setRequestProperty("Authorization", ConfigSetup.getBasicAuth());
            System.out.println(ConfigSetup.getBasicAuth());
            BufferedReader in = new BufferedReader(
                    new InputStreamReader(client.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();
            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
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
