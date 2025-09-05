package com.LegacyLoom.LegacyLoomAi.Repository;

import java.util.UUID;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.LegacyLoom.LegacyLoomAi.Models.AudioTrack;

@Repository
public interface AudioTrackRepository extends JpaRepository<AudioTrack, UUID> {
    
}
