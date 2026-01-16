package legacyloom.timelineservice.dto.request;

import lombok.Data;
import org.springframework.web.multipart.MultipartFile;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

@Data
public class TimelineCreateDTO {
    private String title;
    private String description;
    private LocalDateTime eventDate;
    private String visibility; // "private", "group", "public"
    private List<MultipartFile> mediaFiles;
    private List<String> tags;
    private List<UUID> sharedWith; // User IDs for group visibility
}