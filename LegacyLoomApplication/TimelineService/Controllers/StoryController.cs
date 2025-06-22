using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceResponseShared;
using TimelineService.Models;
using TimelineService.Services;

namespace TimelineService.Controllers
{
    [Route("api/timelines/{timelineId}")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly IMapper _mapper;

        public StoryController(IStoryService storyService, IMapper mapper)
        {
            _storyService = storyService;
            _mapper = mapper;
        }

        [HttpPatch("story")]
        public async Task<ActionResult<ServiceResponse<ReplaceOneResult>>> UpdateStoryInTimeline([FromRoute]string timelineId, [FromBody]Story story)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _storyService.UpdateStoryInTimeline(timelineId, story, userIdRetrieviedFromTokenHeader: userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
