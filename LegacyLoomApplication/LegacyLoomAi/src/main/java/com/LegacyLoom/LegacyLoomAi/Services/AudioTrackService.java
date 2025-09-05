package com.LegacyLoom.LegacyLoomAi.Services;

import java.util.List;
import java.util.UUID;

import org.springframework.stereotype.Service;

import com.LegacyLoom.LegacyLoomAi.DTOs.ServiceResponse;
import com.LegacyLoom.LegacyLoomAi.Models.AudioTrack;
import com.LegacyLoom.LegacyLoomAi.Repository.AudioTrackRepository;

@Service
public class AudioTrackService {

    private AudioTrackRepository repository;

    public AudioTrackService(AudioTrackRepository repository) {
        this.repository = repository;
    }

    public ServiceResponse<String> save(AudioTrack track){
        try {
            if(repository.existsById(track.getId())){
                var oldTrack = repository.findById(track.getId()).orElse(null);
                if(oldTrack == null){
                    return ServiceResponse.error("No track found or deleted", "Try again later");
                }
                return ServiceResponse.success(oldTrack.getUrl());
            }
            repository.save(new AudioTrack(track.getId(), track.getUrl()));
            return ServiceResponse.success(track.getUrl());
        } catch (Exception e) {
            return ServiceResponse.error(e.getMessage(), "Error while saving audio track");
        }
    }

    public ServiceResponse<String> getUrl(UUID id){
        try {
            var track = repository.findById(id).orElse(null);
            if(track == null){
                return ServiceResponse.error("Error while getting the url", "Try again later");
            }
            return ServiceResponse.success(track.getUrl());
        } catch (Exception e) {
            return ServiceResponse.error(e.getMessage(), "Error while retrieving the audio detail");
        }
    }

    public ServiceResponse<List<AudioTrack>> getUrls(){
        try {
            return ServiceResponse.success(repository.findAll());
        } catch (Exception e) {
            return ServiceResponse.error(e.getMessage(), "Error while retrieving the details");
        }
    }
}
