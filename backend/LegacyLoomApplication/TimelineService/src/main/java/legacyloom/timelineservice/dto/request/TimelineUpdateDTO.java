package legacyloom.timelineservice.dto.request;

import legacyloom.timelineservice.dto.shared.TimelineMediaDTO;
import lombok.Data;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

@Data
public class TimelineUpdateDTO {
    private String title;
    private String description;
    private LocalDateTime eventDate;
    private String visibility;
    private List<String> tags;
    private List<UUID> sharedWith;
    private List<TimelineMediaDTO> media;
}