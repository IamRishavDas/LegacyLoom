package legacyloom.timelineservice.controller;

import legacyloom.timelineservice.dto.request.TimelineCreateDTO;
import legacyloom.timelineservice.dto.request.TimelineUpdateDTO;
import legacyloom.timelineservice.dto.response.ServiceResponse;
import legacyloom.timelineservice.dto.response.TimelineDTO;
import legacyloom.timelineservice.service.TimelineService;
import lombok.RequiredArgsConstructor;

import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import io.swagger.v3.oas.annotations.Operation;
// import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;

import java.util.UUID;
import java.util.List;

@RestController
@RequestMapping("/api/timelines")
@RequiredArgsConstructor
@Tag(name = "Timeline", description = "Timeline management APIs")
public class TimelineController {

    private final TimelineService timelineService;

    @Operation(
        summary = "Create a new timeline",
        description = "Create a new timeline with the given details"
    )
    @ApiResponses({
        @ApiResponse(
            responseCode = "201", 
            description = "Timeline created successfully",
            content = @Content(schema = @Schema(implementation = TimelineDTO.class))),
        @ApiResponse(
            responseCode = "400", 
            description = "Invalid input",
            content = @Content),
        @ApiResponse(
            responseCode = "500", 
            description = "Internal server error",
            content = @Content)
    })
    @PostMapping
    public ResponseEntity<ServiceResponse<TimelineDTO>> createTimeline(
            @RequestPart TimelineCreateDTO timelineCreateDTO,
            @RequestPart(required = false) MultipartFile[] mediaFiles,
            @RequestHeader("X-User-Id") UUID userId) {
        
        if (mediaFiles != null && mediaFiles.length > 0) {
            timelineCreateDTO.setMediaFiles(List.of(mediaFiles));
        }
        
        ServiceResponse<TimelineDTO> response = timelineService.createTimeline(timelineCreateDTO, userId);
        return ResponseEntity.status(response.getStatusCode()).body(response);
    }

    @GetMapping
    public ResponseEntity<ServiceResponse<Page<TimelineDTO>>> getUserTimelines(
            @RequestParam UUID userId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int limit) {
        
        ServiceResponse<Page<TimelineDTO>> response = timelineService.getUserTimelines(userId, page, limit);
        return ResponseEntity.status(response.getStatusCode()).body(response);
    }

    @GetMapping("/{id}")
    public ResponseEntity<ServiceResponse<TimelineDTO>> getTimelineById(@PathVariable String id) {
        ServiceResponse<TimelineDTO> response = timelineService.getTimelineById(id);
        return ResponseEntity.status(response.getStatusCode()).body(response);
    }

    @PutMapping("/{id}")
    public ResponseEntity<ServiceResponse<Boolean>> updateTimeline(
            @PathVariable String id,
            @RequestBody TimelineUpdateDTO timelineUpdateDTO,
            @RequestHeader("X-User-Id") UUID userId) {
        
        ServiceResponse<Boolean> response = timelineService.updateTimeline(id, timelineUpdateDTO, userId);
        return ResponseEntity.status(response.getStatusCode()).body(response);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<ServiceResponse<Boolean>> deleteTimeline(
            @PathVariable String id,
            @RequestHeader("X-User-Id") UUID userId) {
        
        ServiceResponse<Boolean> response = timelineService.deleteTimeline(id, userId);
        return ResponseEntity.status(response.getStatusCode()).body(response);
    }
}