using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RequestFeatureShared;
using RequestFeatureShared.Constants;
using ServiceResponseShared;
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
        public async Task<ActionResult<IEnumerable<Timeline>>> GetAll([FromQuery] TimelineRequestParameters timelineRequestParameters)
        {
            var (serviceResponseOfPagedList, metadata) = await _timelineService.GetAll(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(serviceResponseOfPagedList.StatusCode, serviceResponseOfPagedList);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Timeline>> GetById([FromRoute]string id)
        {
            var response = await _timelineService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("user:{userId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetTimelinesByUserId([FromRoute]Guid userId, [FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (response, metadata) = await _timelineService.GetByCreatedBy(userId, timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("public")]
        [Authorize(Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetPublicTimelines([FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (respoonse, metadata) = await _timelineService.GetAllPublicTimelines(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(respoonse.StatusCode, respoonse);
        }
        
        [HttpGet("shared")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetSharedTimelines([FromQuery]TimelineRequestParameters timelineRequestParameters)
        {
            var (respoonse, metadata) = await _timelineService.GetAllSharedTimelines(timelineRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(respoonse.StatusCode, respoonse);
        }
        
        [HttpPatch("{timelineId}/share")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<ReplaceOneResult>>> ShareTimeline([FromRoute]string timelineId, [FromBody]List<Guid> userGuids)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.ShareTimeline(timelineId, userGuids, userIdRetrieviedFromToken: userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{timelineId}/private")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<ReplaceOneResult>>> SetTimelinePrivate([FromRoute]string timelineId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _timelineService.SetTimelinePrivate(timelineId, userIdRetrieviedFromToken: userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Timeline>> Create(CreateTimelineDTO createTimelineDTO)
        {
            var response = await _timelineService.Create(createTimelineDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
