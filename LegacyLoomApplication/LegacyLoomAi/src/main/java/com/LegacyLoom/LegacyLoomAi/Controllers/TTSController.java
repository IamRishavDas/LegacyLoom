package com.LegacyLoom.LegacyLoomAi.Controllers;

import com.LegacyLoom.LegacyLoomAi.DTOs.ServiceResponse;
import com.LegacyLoom.LegacyLoomAi.DTOs.Story;
import com.LegacyLoom.LegacyLoomAi.Services.TextToSpeechService;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/tts")
public class TTSController {
    private final TextToSpeechService textToSpeechService;

    public TTSController(TextToSpeechService textToSpeechService){
        this.textToSpeechService = textToSpeechService;
    }

    @PostMapping(value = "/generate", produces = "audio/wav")
    public ResponseEntity<?> generateSpeech(@RequestBody Story story) throws Exception{

        if(story.title == null || story.content == null){
            throw new IllegalArgumentException("Story title or content can not be null");
        }

        String storyText = story.title + "\n" + story.content;
        ServiceResponse<byte[]> response = textToSpeechService.generateSpeech(storyText);

        if (response.isSuccess()) {
            String sanitizedTitle = story.title.replaceAll("[^a-zA-Z0-9\\p{L}]", "_");
            return ResponseEntity.ok()
                    .contentType(MediaType.parseMediaType("audio/wav"))
                    .header("Content-Disposition", "attachment; filename=\"" + sanitizedTitle + "_narration.wav\"")
                    .body(response.getData());
        } else {
            HttpStatus status = response.getError().equals("Gemini API error") ? HttpStatus.SERVICE_UNAVAILABLE : HttpStatus.BAD_REQUEST;
            if (response.getMessage().contains("429")) {
                status = HttpStatus.TOO_MANY_REQUESTS;
            }
            return ResponseEntity.status(status)
                    .contentType(MediaType.APPLICATION_JSON)
                    .body(response);
        }
    }
}
