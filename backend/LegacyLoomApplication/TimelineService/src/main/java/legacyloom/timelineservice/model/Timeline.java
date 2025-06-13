package legacyloom.timelineservice.model;

import lombok.Data;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

@Data
@Document(collection = "timelines")
public class Timeline {
    @Id
    private String id;
    private UUID userId;
    private String title;
    private String description;
    private LocalDateTime eventDate;
    private String visibility; // "private", "group", "public"
    private List<TimelineMedia> media;
    private List<String> tags;
    private List<UUID> sharedWith;
    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
    private boolean isDeleted;

    // If you're not using Lombok, add getters and setters manually
    public boolean isDeleted() {
        return isDeleted;
    }

    public void setDeleted(boolean deleted) {
        isDeleted = deleted;
    }
}