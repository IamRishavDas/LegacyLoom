package com.LegacyLoom.LegacyLoomAi.Controllers;

import org.springframework.web.bind.annotation.RestController;

import com.LegacyLoom.LegacyLoomAi.Models.AudioTrack;
import com.LegacyLoom.LegacyLoomAi.Services.AudioTrackService;

import jakarta.validation.Valid;

import java.util.List;
import java.util.UUID;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;



@RestController
@RequestMapping(value = "/api/audios")
public class AudioTrackController {

    private AudioTrackService service;
    
    public AudioTrackController(AudioTrackService service) {
        this.service = service;
    }
    
    @PostMapping("/")
    public ResponseEntity<String> postAudioTrack(@Valid @RequestBody AudioTrack entity) {
        var response = service.save(entity);
        if(response.isSuccess()){
            return  ResponseEntity.ok().body(response.getData());
        } else {
            return ResponseEntity.badRequest().body(null);
        }
    }

    @GetMapping("/")
    public ResponseEntity<List<AudioTrack>> getAudioTracks() {
        var response = service.getUrls();
        return ResponseEntity.ok().body(response.getData());
    }
    

    @GetMapping("/{id}")
    public ResponseEntity<String> getAudioTrack(@PathVariable UUID id) {
        var response = service.getUrl(id);
        return ResponseEntity.ok().body(response.getData());
    }
}
