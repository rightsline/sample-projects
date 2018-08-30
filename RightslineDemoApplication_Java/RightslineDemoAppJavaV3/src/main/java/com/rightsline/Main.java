package com.rightsline;

import java.io.UnsupportedEncodingException;
import java.net.MalformedURLException;
import java.nio.charset.StandardCharsets;

public class Main {

    public static void main(String[] args) {
        if(ConfigSetup.getConfigFile()){
            RestClient.DemoMethod();
        }

    }
}
