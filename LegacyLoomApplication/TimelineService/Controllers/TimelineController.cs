using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestFeatureShared.Constants;
using System.Text.Json;
using TimelineService.DTOs;
using TimelineService.Models;
using TimelineService.RequestFeatures;
using TimelineService.Services;

namespace TimelineService.Controllers
{
    [ApiController]
    [Route("api/timelines")]
    public class TimelineController: ControllerBase
    {
        private readonly ITimelineService _timelineService;

        public TimelineController(ITimelineService timelineService)
        {
            _timelineService = timelineService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetAll([FromQuery] TimelineRequestParameters timelineRequestParameters)
        {
            var (serviceResponseOfPagedList, metadata) = await _timelineService.GetAll(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(serviceResponseOfPagedList.StatusCode, serviceResponseOfPagedList);
        }

        [HttpGet("my-timelines")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetCreatorTimelines([FromQuery] TimelineRequestParameters timelineRequestParameters)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var (serviceResponseOfPagedList, metadata) = await _timelineService.GetCreatorTimelines(userId, timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(serviceResponseOfPagedList.StatusCode, serviceResponseOfPagedList);
        }

        [HttpGet("my-timelines/{timelineId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TimelineDTO>> GetUserTimelineCreatedByUser([FromRoute] string timelineId)
        {
            string? userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.GetUserCreatedTimelineByUser(userId, timelineId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<TimelineDTO>> GetById([FromRoute]string id)
        {
            var response = await _timelineService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("user:{userId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetTimelinesByUserId([FromRoute]Guid userId, [FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (response, metadata) = await _timelineService.GetByCreatedBy(userId, timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("public")]
        [Authorize(Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetPublicTimelines([FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (respoonse, metadata) = await _timelineService.GetAllPublicTimelines(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(respoonse.StatusCode, respoonse);
        }
        
        [HttpGet("shared")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetSharedTimelines([FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (respoonse, metadata) = await _timelineService.GetAllSharedTimelines(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(respoonse.StatusCode, respoonse);
        }
        
        [HttpPut("{timelineId}/share")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ShareTimeline([FromRoute]string timelineId, [FromBody]List<Guid> userGuids)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.ShareTimeline(timelineId, userGuids, userIdRetrieviedFromToken: userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{timelineId}/visibility:{visibility}")]
        [Authorize]
        public async Task<IActionResult> SetTimelineVisibility([FromRoute]string timelineId, [FromRoute]string visibility)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.SetTimelineVisibility(timelineId, visibility, userIdRetrieviedFromToken: userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("shared/{userId}")]
        [Authorize(Roles = "User,Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<TimelineDTO>>> GetTimelinesSharedToUserId([FromRoute] Guid userId, TimelineRequestParameters timelineRequestParameters)
        {
            var userIdGotFromTokenHeader = User.FindFirst("UserId")?.Value;
            var (response, metadata) = await _timelineService.GetSharedTimelinesForUserId(userId, userIdGotFromTokenHeader, timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TimelineDTO>> Create([FromForm]CreateTimelineDTO createTimelineDTO)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.Create(createdBy: userId, createTimelineDTO, createTimelineDTO.Files);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{timelineId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteTimeline([FromRoute]string timelineId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.DeleteTimeline(timelineId, userIdRetrieviedFromTokenHeader: userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("permanent-delete:{timelineId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PermanentDeleteTimeline([FromRoute]string timelineId)
        {
            var response = await _timelineService.PermanentDeleteTimeline(timelineId);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpDelete("admin-delete:{timelineId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteTimelineByAdmin([FromRoute]string timelineId)
        {
            var response = await _timelineService.DeleteTimelineByAdmin(timelineId);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpDelete("delete-all-user-deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PermanentDeleteAllUserDeletedTimelines()
        {
            var response = await _timelineService.DeleteAllUserDeletedTimelines();
            return StatusCode(response.StatusCode, response);
        }
    }
}
