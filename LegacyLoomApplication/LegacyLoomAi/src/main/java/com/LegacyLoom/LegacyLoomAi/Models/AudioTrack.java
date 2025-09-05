package com.LegacyLoom.LegacyLoomAi.Models;

import java.util.UUID;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
public class AudioTrack {

    @Id
    private UUID id;
    private String url;
}
