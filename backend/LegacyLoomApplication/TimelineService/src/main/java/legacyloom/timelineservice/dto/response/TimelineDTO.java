package legacyloom.timelineservice.dto.response;

import legacyloom.timelineservice.dto.shared.TimelineMediaDTO;
import lombok.Data;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

@Data
public class TimelineDTO {
    private UUID id;
    private UUID userId;
    private String title;
    private String description;
    private LocalDateTime eventDate;
    private String visibility;
    private List<TimelineMediaDTO> media;
    private List<String> tags;
    private List<UUID> sharedWith;
    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
}