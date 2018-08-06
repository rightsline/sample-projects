package com.rightsline;
//import org.json.*;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.io.FileReader;
import java.util.Base64;




public class ConfigSetup {
    private static String basicAuth;

    public static String getBasicAuth() {
        return basicAuth;
    }

    public static void getConfigFile(){
        String folderPath = System.getProperty("user.dir");
        String configFilePath = folderPath + "\\Config\\configV2.json";
        try {
            JsonObject credentials = new JsonParser().parse(new FileReader(configFilePath)).getAsJsonObject();
            String user = credentials.get("user").getAsString();
            String pass = credentials.get("pass").getAsString();
            String combined = user + ":" + pass;
            basicAuth = "Basic " + Base64.getEncoder().encodeToString(combined.getBytes());
        }
        catch(Exception e){
            System.out.println("Please ensure that you have a valid configV2.json file in the" + folderPath + " Config folder ");
        }
//        System.out.println(getBasicAuth());
//        String pageName = credentials.getJSONObject("pageInfo").getString("pageName");
    }
}
