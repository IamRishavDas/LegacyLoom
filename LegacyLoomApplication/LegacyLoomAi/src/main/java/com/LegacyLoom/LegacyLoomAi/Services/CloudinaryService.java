package com.LegacyLoom.LegacyLoomAi.Services;

import java.io.ByteArrayInputStream;
import java.util.UUID;

import org.springframework.stereotype.Service;

import com.LegacyLoom.LegacyLoomAi.DTOs.ServiceResponse;
import com.LegacyLoom.LegacyLoomAi.Models.AudioTrack;
import com.cloudinary.Cloudinary;
import com.cloudinary.utils.ObjectUtils;

@Service
public class CloudinaryService {

    private Cloudinary cloudinary;
    private AudioTrackService trackService;

    public CloudinaryService(Cloudinary cloudinary, AudioTrackService trackService) {
        this.cloudinary = cloudinary;
        this.trackService = trackService;
    }
    
    public ServiceResponse<String> upload(UUID id, byte[] audioData){
        try {
            ByteArrayInputStream audioStream = new ByteArrayInputStream(audioData);
            var uploadResult = cloudinary.uploader().upload(audioStream, ObjectUtils.asMap(
                "resource_type", "auto",
                "folder", "LegacyLoom_AudioTracks",
                "public_id", id.toString(),
                "format", "wav"
            ));

            String audioUrl = uploadResult.get("secure-url").toString();

            trackService.save(new AudioTrack(id, audioUrl));
            return ServiceResponse.success(audioUrl);
            
        } catch (Exception e) {
            return ServiceResponse.error(e.getMessage(), "Error while uploading audio file");
        }
    }

    public ServiceResponse<Boolean> remove(UUID id){
        try {
            cloudinary.uploader().destroy(id.toString(), ObjectUtils.asMap("resource_type", "video"));
            return ServiceResponse.success(true);
        } catch (Exception e) {
            return ServiceResponse.error(e.getMessage(), "Error while deleteing the audio track");
        }
    }

}
