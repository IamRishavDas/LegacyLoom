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
        Task<ServiceResponse<Timeline>> Create(string? createdBy, CreateTimelineDTO createTimelineDTO);/*createdBy is Guid*/
        Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetAll(TimelineRequestParameters timelineRequestParameters);
        Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetCreatorTimelines(string? userId, TimelineRequestParameters timelineRequestParameters);
        Task<ServiceResponse<Timeline>> GetById(string id);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetByCreatedBy(Guid userId, TimelineRequestParameters timelineRequestParameters);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetAllPublicTimelines(TimelineRequestParameters timelineRequestParameters);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetAllSharedTimelines(TimelineRequestParameters timelineRequestParameters);
        Task<ServiceResponse<ReplaceOneResult>> ShareTimeline(string timelineId, List<Guid> userGuids, string? userIdRetrieviedFromToken);
        Task<ServiceResponse<ReplaceOneResult>> SetTimelineVisibility(string timelineId, string visibility, string? userIdRetrieviedFromToken);
        Task<(ServiceResponse<IEnumerable<Timeline>>, MetaData)> GetSharedTimelinesForUserId(Guid userId, string? userIdRetrieviedFromTokenHeader, TimelineRequestParameters timelineRequestParameters);
        Task<ServiceResponse> DeleteTimeline(string timelineId, string? userIdRetrieviedFromTokenHeader);
        Task<ServiceResponse> PermanentDeleteTimeline(string timelineId);
        Task<ServiceResponse<DeleteResult>> DeleteAllUserDeletedTimelines();
        Task<ServiceResponse> DeleteTimelineByAdmin(string timelineId);
    }
}