package com.LegacyLoom.LegacyLoomAi.DTOs;

import java.util.UUID;

import lombok.AllArgsConstructor;

@AllArgsConstructor
public class Story {
    public UUID id;
    public String title;
    public String content;
}
