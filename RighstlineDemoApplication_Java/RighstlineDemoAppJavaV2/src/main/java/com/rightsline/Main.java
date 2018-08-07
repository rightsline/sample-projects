package com.rightsline;

import java.io.FileNotFoundException;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Main {

    public static void main(String[] args) {
        // write your code
        ConfigSetup.getConfigFile();
        System.out.println("Rightsline Demo Java V0.0.1");
        RestClient.DemoMethod();
    }
}
