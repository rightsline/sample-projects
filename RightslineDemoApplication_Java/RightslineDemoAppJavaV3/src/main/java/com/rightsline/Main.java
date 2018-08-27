package com.rightsline;

import java.io.UnsupportedEncodingException;
import java.net.MalformedURLException;

public class Main {

    public static void main(String[] args) {
        if(ConfigSetup.getConfigFile()){
            System.out.println("CONFIG FILE ACQUIRED. PROCEEDING");
            try {
                RestClient.DemoMethod();
            } catch (MalformedURLException e) {
                e.printStackTrace();
            }
        }

    }
}
