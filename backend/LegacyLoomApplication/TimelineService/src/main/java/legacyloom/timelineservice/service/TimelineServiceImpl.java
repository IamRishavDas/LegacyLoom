package legacyloom.timelineservice.service;

import legacyloom.timelineservice.dto.request.TimelineCreateDTO;
import legacyloom.timelineservice.dto.request.TimelineUpdateDTO;
import legacyloom.timelineservice.dto.response.ServiceResponse;
import legacyloom.timelineservice.dto.response.TimelineDTO;
import legacyloom.timelineservice.exception.ResourceNotFoundException;
import legacyloom.timelineservice.model.Timeline;
import legacyloom.timelineservice.repository.TimelineRepository;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class TimelineServiceImpl implements TimelineService {

    private final TimelineRepository timelineRepository;
    private final ModelMapper modelMapper;

    @Override
    public ServiceResponse<TimelineDTO> createTimeline(TimelineCreateDTO timelineCreateDTO, UUID userId) {
        try {
            Timeline timeline = modelMapper.map(timelineCreateDTO, Timeline.class);
            timeline.setUserId(userId);
            timeline.setCreatedAt(LocalDateTime.now());
            timeline.setUpdatedAt(LocalDateTime.now());
            timeline.setDeleted(false);
            
            Timeline savedTimeline = timelineRepository.save(timeline);
            TimelineDTO timelineDTO = modelMapper.map(savedTimeline, TimelineDTO.class);
            
            return ServiceResponse.<TimelineDTO>builder()
                    .success(true)
                    .data(timelineDTO)
                    .statusCode(HttpStatus.CREATED.value())
                    .successMessage("Timeline created successfully")
                    .build();
        } catch (Exception e) {
            return ServiceResponse.<TimelineDTO>builder()
                    .success(false)
                    .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                    .errorMessage("Failed to create timeline: " + e.getMessage())
                    .build();
        }
    }

    @Override
    public ServiceResponse<Page<TimelineDTO>> getUserTimelines(UUID userId, int page, int limit) {
        try {
            Page<Timeline> timelines = timelineRepository.findByUserIdAndIsDeletedFalse(
                    userId, 
                    PageRequest.of(page, limit)
            );
            
            Page<TimelineDTO> timelineDTOs = timelines.map(
                    timeline -> modelMapper.map(timeline, TimelineDTO.class)
            );
            
            return ServiceResponse.<Page<TimelineDTO>>builder()
                    .success(true)
                    .data(timelineDTOs)
                    .statusCode(HttpStatus.OK.value())
                    .build();
        } catch (Exception e) {
            return ServiceResponse.<Page<TimelineDTO>>builder()
                    .success(false)
                    .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                    .errorMessage("Failed to fetch timelines: " + e.getMessage())
                    .build();
        }
    }

    @Override
    public ServiceResponse<TimelineDTO> getTimelineById(String id) {
        try {
            Timeline timeline = timelineRepository.findByIdAndIsDeletedFalse(id)
                    .orElseThrow(() -> new ResourceNotFoundException("Timeline not found with id: " + id));
            
            TimelineDTO timelineDTO = modelMapper.map(timeline, TimelineDTO.class);
            
            return ServiceResponse.<TimelineDTO>builder()
                    .success(true)
                    .data(timelineDTO)
                    .statusCode(HttpStatus.OK.value())
                    .build();
        } catch (ResourceNotFoundException e) {
            return ServiceResponse.<TimelineDTO>builder()
                    .success(false)
                    .statusCode(HttpStatus.NOT_FOUND.value())
                    .errorMessage(e.getMessage())
                    .build();
        } catch (Exception e) {
            return ServiceResponse.<TimelineDTO>builder()
                    .success(false)
                    .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                    .errorMessage("Failed to fetch timeline: " + e.getMessage())
                    .build();
        }
    }

    @Override
    public ServiceResponse<Boolean> updateTimeline(String id, TimelineUpdateDTO timelineUpdateDTO, UUID userId) {
        try {
            Timeline timeline = timelineRepository.findByIdAndIsDeletedFalse(id)
                    .orElseThrow(() -> new ResourceNotFoundException("Timeline not found with id: " + id));
            
            if (!timeline.getUserId().equals(userId)) {
                return ServiceResponse.<Boolean>builder()
                        .success(false)
                        .statusCode(HttpStatus.FORBIDDEN.value())
                        .errorMessage("You are not authorized to update this timeline")
                        .build();
            }
            
            modelMapper.map(timelineUpdateDTO, timeline);
            timeline.setUpdatedAt(LocalDateTime.now());
            timelineRepository.save(timeline);
            
            return ServiceResponse.<Boolean>builder()
                    .success(true)
                    .data(true)
                    .statusCode(HttpStatus.OK.value())
                    .successMessage("Timeline updated successfully")
                    .build();
        } catch (ResourceNotFoundException e) {
            return ServiceResponse.<Boolean>builder()
                    .success(false)
                    .statusCode(HttpStatus.NOT_FOUND.value())
                    .errorMessage(e.getMessage())
                    .build();
        } catch (Exception e) {
            return ServiceResponse.<Boolean>builder()
                    .success(false)
                    .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                    .errorMessage("Failed to update timeline: " + e.getMessage())
                    .build();
        }
    }

    @Override
    public ServiceResponse<Boolean> deleteTimeline(String id, UUID userId) {
        try {
            Timeline timeline = timelineRepository.findByIdAndIsDeletedFalse(id)
                    .orElseThrow(() -> new ResourceNotFoundException("Timeline not found with id: " + id));
            
            if (!timeline.getUserId().equals(userId)) {
                return ServiceResponse.<Boolean>builder()
                        .success(false)
                        .statusCode(HttpStatus.FORBIDDEN.value())
                        .errorMessage("You are not authorized to delete this timeline")
                        .build();
            }
            
            timeline.setDeleted(true);
            timeline.setUpdatedAt(LocalDateTime.now());
            timelineRepository.save(timeline);
            
            return ServiceResponse.<Boolean>builder()
                    .success(true)
                    .data(true)
                    .statusCode(HttpStatus.OK.value())
                    .successMessage("Timeline deleted successfully")
                    .build();
        } catch (ResourceNotFoundException e) {
            return ServiceResponse.<Boolean>builder()
                    .success(false)
                    .statusCode(HttpStatus.NOT_FOUND.value())
                    .errorMessage(e.getMessage())
                    .build();
        } catch (Exception e) {
            return ServiceResponse.<Boolean>builder()
                    .success(false)
                    .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                    .errorMessage("Failed to delete timeline: " + e.getMessage())
                    .build();
        }
    }
}