package com.rightsline;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.io.File;
import java.io.FileReader;
import java.util.HashMap;

public class ConfigSetup {
    public static HashMap<String, String> credentials = new HashMap<>();

    public static boolean getConfigFile() {
        File file = new File("./Config/dev_config.json");
        try {
            JsonObject configJson = new JsonParser().parse(new FileReader(file.getPath())).getAsJsonObject();
            String accessKey = configJson.get("accessKey").getAsString();
            String secretKey = configJson.get("secretKey").getAsString();
            String xApiKey = configJson.get("xApiKey").getAsString();

            System.out.println("Acess:" + accessKey);
            System.out.println("sekret:" + secretKey);
            System.out.println("api:" + xApiKey);
            credentials.put("accessKey", accessKey);
            System.out.println("not that one");
            credentials.put("secretKey", secretKey);
            System.out.println("not that one either");
            credentials.put("xApiKey", xApiKey);

        } catch (Exception e) {
            System.out.println("Please ensure that you have a valid config file in the Config folder ");
            System.out.println("EXCEPTION: " + e);
        }
		return !credentials.isEmpty();
    }

    public static HashMap<String, String> getCredentials() {
        return credentials;
    }
}
