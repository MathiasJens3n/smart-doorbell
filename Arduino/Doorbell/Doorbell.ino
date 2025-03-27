#include <WiFi.h>
#include <WebServer.h>
#include <EEPROM.h>
#include <HTTPClient.h>
#include "esp_camera.h"
#include <LittleFS.h> 
#include "base64.h"

#define EEPROM_SIZE 128
#define BUTTON_PIN 12  // GPIO pin for the push button to take a picture
#define RESET_BUTTON_PIN 13  // GPIO pin for resetting WiFi credentials

// Camera pins for ESP32-CAM
#define PWDN_GPIO_NUM  32
#define RESET_GPIO_NUM -1
#define XCLK_GPIO_NUM  0
#define SIOD_GPIO_NUM  26
#define SIOC_GPIO_NUM  27

#define Y9_GPIO_NUM    35
#define Y8_GPIO_NUM    34
#define Y7_GPIO_NUM    39
#define Y6_GPIO_NUM    36
#define Y5_GPIO_NUM    21
#define Y4_GPIO_NUM    19
#define Y3_GPIO_NUM    18
#define Y2_GPIO_NUM    5
#define VSYNC_GPIO_NUM 25
#define HREF_GPIO_NUM  23
#define PCLK_GPIO_NUM  22

const char* apSSID = "ESP32-CAM-Setup";
const char* apPassword = "Kode1234!";
const String baseUrl = "http://37.27.27.136/";  // Change to your actual API URL
String id;

WebServer server(80);
unsigned long resetButtonPressTime = 0;
bool resetInProgress = false;

void saveWiFiCredentials(String ssid, String password, String regCode) {
    EEPROM.writeString(0, ssid);
    EEPROM.writeString(32, password);
    EEPROM.writeString(96, regCode);
    EEPROM.commit();
}

void loadWiFiCredentials(String &ssid, String &password, String &userId, String &regCode) {
    ssid = EEPROM.readString(0);
    password = EEPROM.readString(32);
    userId = EEPROM.readString(64);
    regCode = EEPROM.readString(96);
    id = userId;
}

void resetWiFiCredentials() {
    Serial.println("Resetting WiFi credentials...");
    for (int i = 0; i < EEPROM_SIZE; i++) {
        EEPROM.write(i, 0);
    }
    EEPROM.commit();
    Serial.println("WiFi credentials erased. Restarting...");
    ESP.restart();
}

void startAccessPoint() {
    WiFi.softAP(apSSID, apPassword);
    Serial.println("Access Point Started");
    Serial.println(WiFi.softAPIP());

    server.on("/", HTTP_GET, serveHTML);

    server.on("/connect", HTTP_POST, []() {
        String ssid = server.arg("ssid");
        String password = server.arg("password");
        String regCode = server.arg("reg_code");

        Serial.println(regCode);
        if (ssid.length() > 0 && password.length() > 0) {
            saveWiFiCredentials(ssid, password, regCode);
            server.send(200, "text/html", "WiFi credentials saved! Restarting...");
            ESP.restart();
        } else {
            server.send(400, "text/html", "Invalid input");
        }
    });

    server.begin();
}

void serveHTML() {
    File file = LittleFS.open("/index.html");
    Serial.println(LittleFS.totalBytes());
    Serial.println(LittleFS.usedBytes());
    if (!file) {
        server.send(404, "text/plain", "File not found");
        return;
    }
    server.streamFile(file, "text/html");
    file.close();
}

bool connectToWiFi() {
    String ssid, password, userId, regCode;
    loadWiFiCredentials(ssid, password, userId, regCode);

    if (ssid.length() > 0 && password.length() > 0) {
        Serial.print("Connecting to WiFi: ");
        Serial.println(ssid);
        WiFi.begin(ssid.c_str(), password.c_str());

        int tries = 60;
        while (WiFi.status() != WL_CONNECTED && tries-- > 0) {
            delay(500);
            Serial.print(".");
        }

        if(userId == "" && WiFi.status() == WL_CONNECTED){
          userId = getUserId(regCode);
        }

        return WiFi.status() == WL_CONNECTED;
    }
    return false;
}

void setupCamera() {
    camera_config_t config;
    config.ledc_channel = LEDC_CHANNEL_0;
    config.ledc_timer = LEDC_TIMER_0;
    config.pin_d0 = Y2_GPIO_NUM;
    config.pin_d1 = Y3_GPIO_NUM;
    config.pin_d2 = Y4_GPIO_NUM;
    config.pin_d3 = Y5_GPIO_NUM;
    config.pin_d4 = Y6_GPIO_NUM;
    config.pin_d5 = Y7_GPIO_NUM;
    config.pin_d6 = Y8_GPIO_NUM;
    config.pin_d7 = Y9_GPIO_NUM;
    config.pin_xclk = XCLK_GPIO_NUM;
    config.pin_pclk = PCLK_GPIO_NUM;
    config.pin_vsync = VSYNC_GPIO_NUM;
    config.pin_href = HREF_GPIO_NUM;
    config.pin_sscb_sda = SIOD_GPIO_NUM;
    config.pin_sscb_scl = SIOC_GPIO_NUM;
    config.pin_pwdn = PWDN_GPIO_NUM;
    config.pin_reset = RESET_GPIO_NUM;
    config.xclk_freq_hz = 20000000;
    config.pixel_format = PIXFORMAT_JPEG;

    config.frame_size = FRAMESIZE_QVGA;
    config.jpeg_quality = 10;
    config.fb_count = 1;

    esp_err_t err = esp_camera_init(&config);
    if (err != ESP_OK) {
        Serial.println("Camera init failed");
    } else {
        Serial.println("Camera init success");
    }
}

String getUserId(String regCode){
    HTTPClient http;
    WiFiClient client;
    String userId;

    http.begin(client, baseUrl+"Device");
    http.addHeader("Content-Type", "application/json");
    String payload = "{\"registrationcode\":\""+regCode+"\"}";
    Serial.println(payload);
    
    int httpResponseCode = http.POST(payload);
    
    if (httpResponseCode > 0) {  
        // Read response as a string
        String response = http.getString();
        Serial.println("GetUserId ResponseCode: " +httpResponseCode);
        Serial.println("Response: " + response);

        // Extract userId manually
        int start = response.indexOf("\"userid\":") + 10;  // Find start of userId value
        int end = response.indexOf("}", start);  // Find closing quote
        Serial.println(start);
        Serial.println(end);
        if (start > 8 && end > start) {
            userId = response.substring(start+1, end);
            Serial.println("Extracted User ID: " + userId);
            id = userId;
            EEPROM.writeString(64, userId);
            EEPROM.commit();
        } else {
            Serial.println("Failed to extract userId!");
        }
    } else {
        Serial.print("Error in HTTP request: ");
        Serial.println(httpResponseCode);
    }

    http.end();
    return userId;
}

void sendPicture(camera_fb_t *fb) {
    if (WiFi.status() != WL_CONNECTED) {
        Serial.println("WiFi not connected, cannot send picture.");
        return;
    }
    HTTPClient http;
    WiFiClient client;
    http.begin(client, baseUrl+"Image");
    http.addHeader("Content-Type", "application/json");
    String imageBase64 = base64::encode(fb->buf, fb->len);
    String payload = "{\"data\":\""+imageBase64+"\",\"userid\":\""+id+"\"}";

    Serial.println(payload);
    int httpResponseCode = http.POST(payload);
    Serial.print("Server response: ");
    Serial.println(httpResponseCode);
    http.end();
}

void takePicture() {
    camera_fb_t *fb = esp_camera_fb_get();
    if (!fb) {
        Serial.println("Camera capture failed");
        return;
    }

    Serial.println("Image data as byte array:");
    Serial.print("[");
    for (size_t i = 0; i < fb->len; i++) {
        Serial.print(fb->buf[i]); // Print each byte
        if (i < fb->len - 1) Serial.print(", "); // Add comma except for last byte
    }
    Serial.println("]");

    sendPicture(fb);
    esp_camera_fb_return(fb);  // Free memory
}

void setup() {
    Serial.begin(115200);
    pinMode(BUTTON_PIN, INPUT_PULLUP);
    pinMode(RESET_BUTTON_PIN, INPUT_PULLUP);
    EEPROM.begin(EEPROM_SIZE);

    if (!LittleFS.begin(true)) {
        Serial.println("LittleFS Mount Failed");
        return;
    }

    if (!connectToWiFi()) {
        Serial.println("\nFailed to connect. Starting AP mode...");
        startAccessPoint();
    } else {
        Serial.print("\nConnected! IP Address: ");
        Serial.println(WiFi.localIP());
    }

    setupCamera();
}

void loop() {
    server.handleClient();
    
    if (digitalRead(BUTTON_PIN) == LOW) {
        Serial.println("Button pressed! Taking picture...");
        takePicture();
        delay(1000); // Debounce delay
    }

    if (digitalRead(RESET_BUTTON_PIN) == LOW) {
        if (!resetInProgress) {
            resetInProgress = true;
            resetButtonPressTime = millis();
        } else if (millis() - resetButtonPressTime >= 5000) {
            resetWiFiCredentials();
        }
    } else {
        resetInProgress = false;
    }
}
