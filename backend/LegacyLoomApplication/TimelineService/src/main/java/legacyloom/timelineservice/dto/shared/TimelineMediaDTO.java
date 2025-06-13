package legacyloom.timelineservice.dto.shared;

import lombok.Data;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
public class TimelineMediaDTO {
    private UUID mediaId;
    private String storagePath;
    private String mediaType; // "image", "video", etc.
    private String caption;
    private LocalDateTime createdAt;
    private String signedUrl; // Temporary URL for media access
}