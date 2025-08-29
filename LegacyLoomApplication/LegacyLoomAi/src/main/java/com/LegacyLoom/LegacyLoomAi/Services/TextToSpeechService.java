package com.LegacyLoom.LegacyLoomAi.Services;

import com.LegacyLoom.LegacyLoomAi.DTOs.ServiceResponse;
import com.fasterxml.jackson.databind.JsonNode;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestClientException;
import org.springframework.web.client.RestTemplate;

import com.fasterxml.jackson.databind.ObjectMapper;

import javax.sound.sampled.AudioFileFormat;
import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioInputStream;
import javax.sound.sampled.AudioSystem;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.util.Base64;

@Service
public class TextToSpeechService {
    
    @Value("${gemini.api.key}")
    private String apiKey;

    @Value("${google.gemini.url}")
    private String geminiUrl;

    @SuppressWarnings("unused")
    public ServiceResponse<byte[]> generateSpeech(String storyContent) {
        try {
            if (storyContent == null || storyContent.trim().isEmpty()) {
                throw new IllegalArgumentException("Text input cannot be null or empty");
            }

            RestTemplate restTemplate = new RestTemplate();
            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_JSON);

            ObjectMapper mapper = new ObjectMapper();
            String requestJson = mapper.writeValueAsString(new Object() {
                public final Object contents = new Object[] {
                    new Object() {
                        public final String role = "user";
                        public final Object parts = new Object[] {
                            new Object() {
                                public final String text = storyContent;
                            }
                        };
                    }
                };
                public final Object generationConfig = new Object() {
                    public final double temperature = 1;
                    public final String[] responseModalities = {"AUDIO"};
                    public final Object speechConfig = new Object() {
                        public final Object voiceConfig = new Object() {
                            public final Object prebuiltVoiceConfig = new Object() {
                                public final String voiceName = "Enceladus";
                            };
                        };
                    };
                };
            });

            HttpEntity<String> entity = new HttpEntity<>(requestJson, headers);
            String url = geminiUrl + "?key=" + apiKey + "&alt=sse";

            String response = restTemplate.postForObject(url, entity, String.class);
            byte[] audioData = parseStreamingResponse(response);
            byte[] wavData = convertToWav(audioData, "audio/L16;rate=24000"); 

            String fileName = "narration_" + System.currentTimeMillis() + ".wav";
            saveAudioToFile(wavData, fileName);

            return ServiceResponse.success(wavData);
        } catch (IllegalArgumentException e) {
            return ServiceResponse.error("Invalid input", e.getMessage());
        } catch (RestClientException e) {
            return ServiceResponse.error("Gemini API error", "Failed to connect to the Gemini API: " + e.getMessage());
        } catch (Exception e) {
            return ServiceResponse.error("Internal server error", "An unexpected error occurred: " + e.getMessage());
        }
    }

    private byte[] parseStreamingResponse(String response) throws Exception {
    try {
        if (response == null || response.trim().isEmpty()) {
            throw new RuntimeException("Empty response from Gemini API");
        }

        String[] lines = response.split("\n");
        StringBuilder audioBase64 = new StringBuilder();
        ObjectMapper mapper = new ObjectMapper();

        for (String line : lines) {
            if (!line.startsWith("data: ") || line.trim().equals("data: [DONE]")) {
                continue; // Skip non-data or [DONE] lines
            }

            String jsonData = line.substring(6).trim();
            if (jsonData.isEmpty()) {
                continue; // Skip empty JSON data
            }

            try {
                JsonNode jsonNode = mapper.readTree(jsonData);
                JsonNode inlineData = jsonNode.at("/candidates/0/content/parts/0/inlineData");
                if (inlineData.has("data") && !inlineData.get("data").isNull()) {
                    audioBase64.append(inlineData.get("data").asText());
                }
            } catch (Exception e) {
                System.err.println("Failed to parse JSON line: " + jsonData + ", Error: " + e.getMessage());
                continue; // Skip malformed JSON lines
            }
        }

        if (audioBase64.length() == 0) {
            throw new RuntimeException("No audio data received from Gemini API");
        }

        return Base64.getDecoder().decode(audioBase64.toString());
    } catch (Exception e) {
        throw new RuntimeException("Failed to parse streaming response: " + e.getMessage(), e);
    }
}
    private byte[] convertToWav(byte[] audioData, String mimeType) throws Exception {
        int bitsPerSample = 16;
        int sampleRate = 24000;
        String[] parts = mimeType.split(";");
        for (String param : parts) {
            param = param.trim();
            if (param.toLowerCase().startsWith("rate=")) {
                try {
                    sampleRate = Integer.parseInt(param.split("=", 2)[1]);
                } catch (NumberFormatException ignored) {
                }
            } else if (param.startsWith("audio/L")) {
                try {
                    bitsPerSample = Integer.parseInt(param.split("L", 2)[1]);
                } catch (NumberFormatException ignored) {
                }
            }
        }

        int numChannels = 1;
        int dataSize = audioData.length;
        int bytesPerSample = bitsPerSample / 8;
        int blockAlign = numChannels * bytesPerSample;
        int byteRate = sampleRate * blockAlign;
        int chunkSize = 36 + dataSize;

        ByteArrayOutputStream header = new ByteArrayOutputStream();
        header.write("RIFF".getBytes());
        header.write(intToByteArray(chunkSize));
        header.write("WAVE".getBytes());
        header.write("fmt ".getBytes());
        header.write(intToByteArray(16));
        header.write(shortToByteArray((short) 1));
        header.write(shortToByteArray((short) numChannels));
        header.write(intToByteArray(sampleRate));
        header.write(intToByteArray(byteRate));
        header.write(shortToByteArray((short) blockAlign));
        header.write(shortToByteArray((short) bitsPerSample));
        header.write("data".getBytes());
        header.write(intToByteArray(dataSize));

        ByteArrayOutputStream wavData = new ByteArrayOutputStream();
        wavData.write(header.toByteArray());
        wavData.write(audioData);

        return wavData.toByteArray();
    }

    public void saveAudioToFile(byte[] audioData, String fileName) throws Exception {
        AudioFormat format = new AudioFormat(24000, 16, 1, true, false);
        try (AudioInputStream audioInputStream = new AudioInputStream(
                new ByteArrayInputStream(audioData), format, audioData.length)) {
            AudioSystem.write(audioInputStream, AudioFileFormat.Type.WAVE, new File(fileName));
        }
        System.out.println("File saved to: " + fileName);
    }

    private byte[] intToByteArray(int value) {
        return new byte[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
            (byte) ((value >> 24) & 0xFF)
        };
    }

    private byte[] shortToByteArray(short value) {
        return new byte[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF)
        };
    }
}
