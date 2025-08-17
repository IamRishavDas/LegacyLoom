using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestFeatureShared.Constants;
using System.Text.Json;
using TimelineService.DTOs;
using TimelineService.RequestFeatures;
using TimelineService.Services;

namespace TimelineService.Controllers
{
    [ApiController]
    [Route("api/drafts")]
    public class DraftController: ControllerBase
    {

        private readonly ITimelineDraftService _draftService;
        public DraftController(ITimelineDraftService draftService)
        {
            _draftService = draftService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SaveDraft([FromForm]CreateTimelineDraft createTimelineDraft)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _draftService.SaveDraft(userId, createTimelineDraft);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<TimelineDraftLookupDTO>>> GetAll([FromQuery]TimelineDraftRequestParameters timelineDraftRequestParameters)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var (response, metadata) = await _draftService.GetDrafts(userId, timelineDraftRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TimelineDraftDTO>> Get([FromRoute]string id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _draftService.GetDraft(userId, id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TimelineDraftDTO>> UpdateDraft([FromRoute]string id, [FromForm]UpdateTimelineDraft updateTimelineDraft)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _draftService.UpdateDraft(userId, id, updateTimelineDraft);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _draftService.DeleteDraft(userId, id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
