package com.rightsline;

import java.io.UnsupportedEncodingException;
import java.net.MalformedURLException;

public class Main {

    public static void main(String[] args) {
        if(ConfigSetup.getConfigFile()){
            RestClient.DemoMethod();
        }

    }
}
