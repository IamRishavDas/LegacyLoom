using RequestFeatureShared;
using ServiceResponseShared;
using System.ComponentModel.DataAnnotations;
using TimelineService.DTOs;
using TimelineService.RequestFeatures;

namespace TimelineService.Services
{
    public interface ITimelineDraftService
    {
        Task<ServiceResponse> DeleteDraft(string? userId, string draftId);
        Task<ServiceResponse<TimelineDraftDTO>> GetDraft(string? userId, string draftId);
        Task<(ServiceResponse<IEnumerable<TimelineDraftLookupDTO>>, MetaData)> GetDrafts(string? userId, TimelineDraftRequestParameters timelineDraftRequestParams);
        Task<ServiceResponse> SaveDraft(string? userId, [Required] CreateTimelineDraft createTimelineDraft);
        Task<ServiceResponse<TimelineDraftDTO>> UpdateDraft(string? userId, string draftId, UpdateTimelineDraft updateTimelineDraft);
    }
}