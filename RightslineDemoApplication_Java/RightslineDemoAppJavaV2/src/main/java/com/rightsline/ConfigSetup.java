package com.rightsline;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.io.File;
import java.io.FileReader;
import java.util.Base64;

public class ConfigSetup {
    private static String basicAuth;

    public static String getBasicAuth() {
        return basicAuth;
    }

    public static boolean getConfigFile(){
        File file = new File("./Config/dev_config.json");

        try {
            JsonObject credentials = new JsonParser().parse(new FileReader(file.getPath())).getAsJsonObject();
            String user = credentials.get("user").getAsString();
            String pass = credentials.get("pass").getAsString();
            String combined = user + ":" + pass;
            basicAuth = "Basic " + Base64.getEncoder().encodeToString(combined.getBytes());            
        }
        catch(Exception e){
            System.out.println("Please ensure that you have a valid config file in the Config folder ");
        }
		return !basicAuth.isEmpty();        
//        System.out.println(getBasicAuth());
//        String pageName = credentials.getJSONObject("pageInfo").getString("pageName");
    }
}
