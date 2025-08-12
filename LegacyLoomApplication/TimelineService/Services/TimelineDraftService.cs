using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RequestFeatureShared;
using RequestFeatureShared.SortHelper;
using ServiceResponseShared;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TimelineService.DTOs;
using TimelineService.Models;
using TimelineService.MongoRepository;
using TimelineService.RequestFeatures;

namespace TimelineService.Services
{
    public class TimelineDraftService : ITimelineDraftService
    {

        private readonly AppMongoRepository _mongoRepository;
        private readonly ISortHelper<TimelineDraft> _sort;
        private readonly IMapper _mapper;

        public TimelineDraftService(AppMongoRepository mongoRepository, ISortHelper<TimelineDraft> sort, IMapper mapper)
        {
            _mongoRepository = mongoRepository;
            _sort = sort;
            _mapper = mapper;
        }


        public async Task<ServiceResponse> SaveDraft(string? userId, [Required] CreateTimelineDraft createTimelineDraft)
        {
            if (userId == null || !Guid.TryParse(userId, out Guid _userId))
            {
                return ServiceResponse.Failure("Invalid userId", (int)HttpStatusCode.BadRequest);
            }

            if((string.IsNullOrEmpty(createTimelineDraft.Title) || string.IsNullOrWhiteSpace(createTimelineDraft.Title)) && (string.IsNullOrEmpty(createTimelineDraft.Content) || string.IsNullOrWhiteSpace(createTimelineDraft.Content)))
            {
                return ServiceResponse.Failure("Blank draft is not allowed", (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var _timelineDraftCollection = await _mongoRepository.GetTimelineDraftCollectionContext();
                var timelineDraft = new TimelineDraft()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = createTimelineDraft.Title == null ? createTimelineDraft.Title : createTimelineDraft.Title.Trim(),
                    Content = createTimelineDraft.Content == null ? createTimelineDraft.Content : createTimelineDraft.Content.Trim(),
                    CreatedBy = userId,
                };

                await _timelineDraftCollection.InsertOneAsync(timelineDraft);
                return ServiceResponse.SuccessResult((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while creating the draft", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDraftLookupDTO>>, MetaData)> GetDrafts(string? userId, TimelineDraftRequestParameters timelineDraftRequestParams)
        {
            if (userId == null || !Guid.TryParse(userId, out Guid _userId))
            {
                return (ServiceResponse<IEnumerable<TimelineDraftLookupDTO>>.Failure("Invalid userId", (int)HttpStatusCode.BadRequest), new MetaData());
            }
            try
            {
                var _timelineDraftCollection = await _mongoRepository.GetTimelineDraftCollectionContext();
                var drafts = await _timelineDraftCollection.Find(timelineDraft => timelineDraft.CreatedBy == userId && !timelineDraft.IsDeleted).ToListAsync();
                var orderedDrafts = _sort.ApplySort(drafts.AsQueryable<TimelineDraft>(), timelineDraftRequestParams.OrderBy);

                var count = drafts.Count;
                var result = orderedDrafts.Skip((timelineDraftRequestParams.PageNumber - 1) * timelineDraftRequestParams.PageSize).Take(timelineDraftRequestParams.PageSize).ToList();
                var pagedDrafts = PagedList<TimelineDraft>.ToPagedList(result, count, timelineDraftRequestParams.PageNumber, timelineDraftRequestParams.PageSize);

                var lookups = _mapper.Map<List<TimelineDraftLookupDTO>>(pagedDrafts);
                return (ServiceResponse<IEnumerable<TimelineDraftLookupDTO>>.SuccessResult(lookups, (int)HttpStatusCode.OK), pagedDrafts.MetaData);
            }
            catch (Exception ex)
            {
                return (ServiceResponse<IEnumerable<TimelineDraftLookupDTO>>.Failure("Error while retrieving drafts", [ex.Message], (int)HttpStatusCode.InternalServerError), new MetaData());
            }
        }

        public async Task<ServiceResponse<TimelineDraftDTO>> UpdateDraft(string? userId, string draftId, UpdateTimelineDraft updateTimelineDraft)
        {
            if (userId == null || !Guid.TryParse(userId, out Guid _userId))
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Invalid userId", (int)HttpStatusCode.BadRequest);
            }

            if (draftId == null || !ObjectId.TryParse(draftId, out ObjectId _id))
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Invalid draft id", (int)HttpStatusCode.BadRequest);
            }

            if ((string.IsNullOrEmpty(updateTimelineDraft.Title) || string.IsNullOrWhiteSpace(updateTimelineDraft.Title)) && (string.IsNullOrEmpty(updateTimelineDraft.Content) || string.IsNullOrWhiteSpace(updateTimelineDraft.Content)))
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Blank draft is not allowed", (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var _timelineDraftCollection = await _mongoRepository.GetTimelineDraftCollectionContext();
                var filter = Builders<TimelineDraft>.Filter.Eq(draft => draft.Id, draftId) &
                             Builders<TimelineDraft>.Filter.Eq(draft => draft.IsDeleted, false) &
                             Builders<TimelineDraft>.Filter.Eq(draft => draft.CreatedBy, userId);

                if (filter == null)
                {
                    return ServiceResponse<TimelineDraftDTO>.Failure("Draft not found", (int)HttpStatusCode.NotFound);
                }

                var update = Builders<TimelineDraft>.Update
                    .Set(draft => draft.Title, updateTimelineDraft.Title)
                    .Set(draft => draft.Content, updateTimelineDraft.Content)
                    .Set(draft => draft.LastModified, DateTime.UtcNow);

                var updatedDraft = await _timelineDraftCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<TimelineDraft>()
                {
                    ReturnDocument = ReturnDocument.After
                });

                if (updatedDraft == null)
                {
                    return ServiceResponse<TimelineDraftDTO>.Failure("Draft not found or not authorized", (int)HttpStatusCode.NotFound);
                }

                return ServiceResponse<TimelineDraftDTO>.SuccessResult(_mapper.Map<TimelineDraftDTO>(updatedDraft), (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Error while retrieving draft", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<TimelineDraftDTO>> GetDraft(string? userId, string draftId)
        {
            if (userId == null || !Guid.TryParse(userId, out Guid _userId))
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Invalid userId", (int)HttpStatusCode.BadRequest);
            }

            if (draftId == null || !ObjectId.TryParse(draftId, out ObjectId _id))
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Invalid draft id", (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var _timelineDraftCollection = await _mongoRepository.GetTimelineDraftCollectionContext();
                var draft = await _timelineDraftCollection.Find(draft => draft.Id == draftId && !draft.IsDeleted && draft.CreatedBy == userId).FirstOrDefaultAsync();

                if (draft == null)
                {
                    return ServiceResponse<TimelineDraftDTO>.Failure("Draft not found", (int)HttpStatusCode.NotFound);
                }

                return ServiceResponse<TimelineDraftDTO>.SuccessResult(_mapper.Map<TimelineDraftDTO>(draft), (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDraftDTO>.Failure("Error while retrieving draft", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteDraft(string? userId, string draftId)
        {
            if (userId == null || !Guid.TryParse(userId, out Guid _userId))
            {
                return ServiceResponse.Failure("Invalid userId", (int)HttpStatusCode.BadRequest);
            }

            if (draftId == null || !ObjectId.TryParse(draftId, out ObjectId _id))
            {
                return ServiceResponse.Failure("Invalid draft id", (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var _timelineDraftCollection = await _mongoRepository.GetTimelineDraftCollectionContext();
                var filter = Builders<TimelineDraft>.Filter.Eq(draft => draft.Id, draftId) &
                             Builders<TimelineDraft>.Filter.Eq(draft => draft.IsDeleted, false) &
                             Builders<TimelineDraft>.Filter.Eq(draft => draft.CreatedBy, userId);

                if (filter == null)
                {
                    return ServiceResponse.Failure("Draft not found", (int)HttpStatusCode.NotFound);
                }

                var update = Builders<TimelineDraft>.Update.Set(draft => draft.IsDeleted, true);
                await _timelineDraftCollection.UpdateOneAsync(filter, update);

                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while retrieving draft", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}