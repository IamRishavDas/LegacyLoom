using MongoDB.Driver;
using ServiceResponseShared;
using TimelineService.Models;

namespace TimelineService.Services
{
    public interface IStoryService
    {
        Task<ServiceResponse<ReplaceOneResult>> UpdateStoryInTimeline(string timelineId, Story story, string? userIdRetrieviedFromTokenHeader);
    }
}