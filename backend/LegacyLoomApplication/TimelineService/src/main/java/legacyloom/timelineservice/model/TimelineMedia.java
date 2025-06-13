package legacyloom.timelineservice.model;

import lombok.Data;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
public class TimelineMedia {
    private UUID mediaId;
    private String storagePath;
    private String mediaType; // "image", "video", etc.
    private String caption;
    private LocalDateTime createdAt;
    private String signedUrl; // Temporary URL for media access
}