using MongoDB.Driver;
using RequestFeatureShared;
using ServiceResponseShared;
using TimelineService.DTOs;
using TimelineService.Models;
using TimelineService.RequestFeatures;

namespace TimelineService.Services
{
    public interface ITimelineService
    {
        Task<ServiceResponse<Timeline>> Create(CreateTimelineDTO createTimelineDTO);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetAll(TimelineRequestParameters timelineRequestParameters);
        Task<ServiceResponse<Timeline>> GetById(string id);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetByCreatedBy(Guid userId, TimelineRequestParameters timelineRequestParameters);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetAllPublicTimelines(TimelineRequestParameters timelineRequestParameters);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetAllSharedTimelines(TimelineRequestParameters timelineRequestParameters);
        Task<ServiceResponse<ReplaceOneResult>> ShareTimeline(string timelineId, List<Guid> userGuids, string? userIdRetrieviedFromToken);
        Task<ServiceResponse<ReplaceOneResult>> SetTimelinePrivate(string timelineId, string? userIdRetrieviedFromToken);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetSharedTimelinesForUserId(Guid userId, TimelineRequestParameters timelineRequestParameters);
    }
}