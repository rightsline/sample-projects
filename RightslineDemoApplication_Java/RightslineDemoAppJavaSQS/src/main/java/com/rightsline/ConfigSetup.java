package com.rightsline;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;


import java.io.File;
import java.io.FileReader;
import java.util.Base64;
import java.util.HashMap;

public class ConfigSetup {
    


    public static HashMap<String, String> getConfigFile(){
        HashMap<String, String> Credentials = new HashMap<>();
        File file = new File("./Config/dev_config.json");

        try {
            JsonObject credentials = new JsonParser().parse(new FileReader(file.getPath())).getAsJsonObject();
            String accessKey = credentials.get("AccessKey").getAsString();
            String secretKey = credentials.get("SecretKey").getAsString();
            String region = credentials.get("Region").getAsString();
            String queueName = credentials.get("QueueName").getAsString();
            String accountId = credentials.get("AccountId").getAsString();
            Credentials.put("AccountId", accountId);
            Credentials.put("QueueName", queueName);
            Credentials.put("AccessKey", accessKey);
            Credentials.put("SecretKey", secretKey);
            Credentials.put("Region", region);            
        }
        catch(Exception e){
            System.out.println("Please ensure that you have a valid config file in the Config folder ");
            e.printStackTrace();
        }
        return Credentials;


    }
}
