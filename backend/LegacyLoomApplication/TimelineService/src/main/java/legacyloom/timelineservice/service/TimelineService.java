package legacyloom.timelineservice.service;

import legacyloom.timelineservice.dto.request.TimelineCreateDTO;
import legacyloom.timelineservice.dto.request.TimelineUpdateDTO;
import legacyloom.timelineservice.dto.response.ServiceResponse;
import legacyloom.timelineservice.dto.response.TimelineDTO;
import org.springframework.data.domain.Page;

import java.util.UUID;

public interface TimelineService {
    ServiceResponse<TimelineDTO> createTimeline(TimelineCreateDTO timelineCreateDTO, UUID userId);
    ServiceResponse<Page<TimelineDTO>> getUserTimelines(UUID userId, int page, int limit);
    ServiceResponse<TimelineDTO> getTimelineById(String id);
    ServiceResponse<Boolean> updateTimeline(String id, TimelineUpdateDTO timelineUpdateDTO, UUID userId);
    ServiceResponse<Boolean> deleteTimeline(String id, UUID userId);
}