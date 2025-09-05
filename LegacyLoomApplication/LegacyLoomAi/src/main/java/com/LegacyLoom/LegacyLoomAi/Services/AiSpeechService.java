package com.LegacyLoom.LegacyLoomAi.Services;

import org.springframework.stereotype.Service;

@Service
public class AiSpeechService {
    
    private AudioTrackService audioTrackService;
    private TextToSpeechService textToSpeechService;
    private CloudinaryService cloudinaryService;
    
    public AiSpeechService(AudioTrackService audioTrackService, TextToSpeechService textToSpeechService,
            CloudinaryService cloudinaryService) {
        this.audioTrackService = audioTrackService;
        this.textToSpeechService = textToSpeechService;
        this.cloudinaryService = cloudinaryService;
    }

    public void getOrGenerateSpeech(){
        
    }
    

}
