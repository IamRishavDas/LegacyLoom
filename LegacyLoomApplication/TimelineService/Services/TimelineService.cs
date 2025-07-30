using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RequestFeatureShared;
using RequestFeatureShared.SortHelper;
using ServiceResponseShared;
using System.Net;
using TimelineService.DTOs;
using TimelineService.Models;
using TimelineService.MongoRepository;
using TimelineService.RequestFeatures;

namespace TimelineService.Services
{
    public class TimelineService : ITimelineService
    {
        private readonly AppMongoRepository _mongoRepository;
        private readonly ISortHelper<Timeline> _sortHelper;
        private readonly IImageUploadService _imageUploadService;
        private readonly IMapper _mapper;

        public TimelineService(AppMongoRepository mongoRepository, IMapper mapper, ISortHelper<Timeline> sortHelper, IImageUploadService imageUploadService)
        {
            _mongoRepository = mongoRepository;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetAll(TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(s => !s.IsDeleted && s.Visibility != TimelineVisibility.PRIVATE).ToListAsync();
                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1 * timelineRequestParameters.PageSize)).ToList();

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                var listOfTimeline = _mapper.Map<IEnumerable<Timeline>>(pagedListTimelines);
                var listOfTimelineDto = _mapper.Map<List<TimelineDTO>>(listOfTimeline);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult(listOfTimelineDto, (int)HttpStatusCode.OK),
                        pagedListTimelines.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetCreatorTimelines(string? userId, TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                if(userId == null && !Guid.TryParse(userId, out Guid creatorId))
                {
                    return
                        (
                            ServiceResponse<IEnumerable<TimelineDTO>>.Failure("You are unauthorized", (int)HttpStatusCode.Unauthorized),
                            new MetaData()
                        );
                }

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(s => !s.IsDeleted && s.CreatedBy == userId).ToListAsync();
                
                foreach(var timeline in timelines)
                {
                    timeline.Story.Content = new String(timeline.Story.Content.Take(150).ToArray());
                }

                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1 * timelineRequestParameters.PageSize)).ToList();

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                var listOfTimeline = _mapper.Map<IEnumerable<Timeline>>(pagedListTimelines);
                var listOfTimelineDto = _mapper.Map<List<TimelineDTO>>(listOfTimeline);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult(listOfTimelineDto, (int)HttpStatusCode.OK),
                        pagedListTimelines.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetAllPublicTimelines(TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(timeline => timeline.Visibility == TimelineVisibility.PUBLIC && !timeline.IsDeleted).ToListAsync();
                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1 * timelineRequestParameters.PageSize)).ToList();

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult
                        (
                            _mapper.Map<IEnumerable<TimelineDTO>>(pagedListTimelines),
                            (int)HttpStatusCode.OK
                        ),
                        pagedListTimelines.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetAllSharedTimelines(TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(timeline => timeline.Visibility == TimelineVisibility.SHARED && !timeline.IsDeleted).ToListAsync();
                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1 * timelineRequestParameters.PageSize)).ToList();

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult
                        (
                            _mapper.Map<IEnumerable<TimelineDTO>>(pagedListTimelines),
                            (int)HttpStatusCode.OK
                        ),
                        pagedListTimelines.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<ServiceResponse<ReplaceOneResult>> ShareTimeline(string timelineId, List<Guid> userGuids, string? userIdRetrieviedFromToken)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse<ReplaceOneResult>.Failure($"TimelineId: {timelineId} is not valid object id", (int)HttpStatusCode.BadRequest);
                if (userIdRetrieviedFromToken == null) return ServiceResponse<ReplaceOneResult>.Failure("You have no priviledge to do that", (int)HttpStatusCode.Unauthorized);
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();
                if (timeline == null) return ServiceResponse<ReplaceOneResult>.Failure($"No such timeline found or deleted", (int)HttpStatusCode.NotFound);
                if (timeline.CreatedBy != userIdRetrieviedFromToken)
                    return ServiceResponse<ReplaceOneResult>.Failure("You have no access to share this timeline", (int)HttpStatusCode.Unauthorized);
                if (timeline.Visibility == TimelineVisibility.PRIVATE)
                    return ServiceResponse<ReplaceOneResult>.Failure($"Timeline is private make it public to share with others", (int)HttpStatusCode.BadRequest);

                if (timeline.SharedWith == null)
                    timeline.SharedWith = new List<string>();

                timeline.SharedWith.AddRange(userGuids.Select(s => s.ToString()).ToList());
                timeline.Visibility = TimelineVisibility.SHARED;
                timeline.LastModified = DateTime.UtcNow;

                ReplaceOneResult result = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse<ReplaceOneResult>.SuccessResult(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReplaceOneResult>.Failure("Error while sharing timeline with others", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ReplaceOneResult>> RemoveUsersFromSharedTimeline(string timelineId, List<Guid> userGuids, string? userIdRetrieviedFromToken)
        {
            try
            {
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse<ReplaceOneResult>.Failure($"TimelineId: {timelineId} is not valid object id", (int)HttpStatusCode.BadRequest);
                if (userIdRetrieviedFromToken == null) return ServiceResponse<ReplaceOneResult>.Failure("You have no priviledge to do that", (int)HttpStatusCode.Unauthorized);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null) return ServiceResponse<ReplaceOneResult>.Failure($"No such timeline found or deleted", (int)HttpStatusCode.NotFound);
                if (timeline.SharedWith == null) return ServiceResponse<ReplaceOneResult>.Failure("Timeline is not shared with anyone", (int)HttpStatusCode.BadRequest);
                if (timeline.CreatedBy != userIdRetrieviedFromToken)
                    return ServiceResponse<ReplaceOneResult>.Failure("You have no access to share this timeline", (int)HttpStatusCode.Unauthorized);
                if (timeline.Visibility == TimelineVisibility.PRIVATE)
                    return ServiceResponse<ReplaceOneResult>.Failure($"Timeline is private make it public to share with others", (int)HttpStatusCode.BadRequest);

                foreach (var userId in userGuids)
                {
                    timeline.SharedWith.Remove(userId.ToString());
                }

                if (timeline.SharedWith.Count == 0)
                {
                    timeline.Visibility = TimelineVisibility.PUBLIC;
                }
                else if (timeline.SharedWith.Count > 0 && timeline.Visibility == TimelineVisibility.PUBLIC)
                {
                    timeline.Visibility = TimelineVisibility.SHARED;
                }
                    
                timeline.LastModified = DateTime.UtcNow;

                ReplaceOneResult result = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse<ReplaceOneResult>.SuccessResult(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReplaceOneResult>.Failure("Error while sharing timeline with others", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ReplaceOneResult>> SetTimelineVisibility(string timelineId, string visibility, string? userIdRetrieviedFromToken)
        {
            try
            {
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse<ReplaceOneResult>.Failure($"TimelineId: {timelineId} is not valid object id", (int)HttpStatusCode.BadRequest);
                if (userIdRetrieviedFromToken == null) return ServiceResponse<ReplaceOneResult>.Failure("You have no priviledge to do that", (int)HttpStatusCode.Unauthorized);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null) return ServiceResponse<ReplaceOneResult>.Failure($"No such timeline found or deleted", (int)HttpStatusCode.NotFound);
                if (timeline.CreatedBy != userIdRetrieviedFromToken)
                    return ServiceResponse<ReplaceOneResult>.Failure("You have no access to make this timeline private", (int)HttpStatusCode.Unauthorized);

                var visibilityList = new List<string>()
                { 
                    TimelineVisibility.PRIVATE.ToString(), 
                    TimelineVisibility.PUBLIC.ToString() 
                };
                var visibilityIndex = visibilityList.IndexOf(visibility);

                if (visibilityIndex == -1)
                    return ServiceResponse<ReplaceOneResult>.Failure("Invalid visibility parameter", (int)HttpStatusCode.BadRequest);

                if(visibilityIndex == 0)
                {
                    timeline.Visibility = TimelineVisibility.PRIVATE;
                } 
                else if(visibilityIndex == 1)
                {
                    timeline.Visibility = TimelineVisibility.PUBLIC;
                }
                timeline.LastModified = DateTime.UtcNow;

                ReplaceOneResult result = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse<ReplaceOneResult>.SuccessResult(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReplaceOneResult>.Failure("Error while sharing timeline with others", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<TimelineDTO>> GetById(string id)
        {
            try
            {
                ObjectId objectId;
                bool isObjectId = ObjectId.TryParse(id, out objectId);
                if (!isObjectId) return ServiceResponse<TimelineDTO>.Failure($"Invalid object id: {id}", (int)HttpStatusCode.BadRequest);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(s => s.Id == id && s.Visibility == TimelineVisibility.PUBLIC && !s.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null) return ServiceResponse<TimelineDTO>.Failure($"Timeline with id: {id} not found", (int)HttpStatusCode.NotFound);
                return ServiceResponse<TimelineDTO>.SuccessResult(_mapper.Map<TimelineDTO>(timeline), (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDTO>.Failure("Error while retrieving the Timeline", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetByCreatedBy(Guid userId, TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelinesByUser = await _timelineCollection.Find(timeline => timeline.CreatedBy == userId.ToString() && timeline.Visibility != TimelineVisibility.PRIVATE && !timeline.IsDeleted).ToListAsync();
                var orderedTimlinesByUserId = _sortHelper.ApplySort(timelinesByUser.AsQueryable(), timelineRequestParameters.OrderBy).ToList();
                var count = timelinesByUser.Count;
                var result = orderedTimlinesByUserId.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).ToList();
                var pagedList = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult(_mapper.Map<IEnumerable<TimelineDTO>>(pagedList), (int)HttpStatusCode.OK),
                        pagedList.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retrieving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetSharedTimelinesForUserId(Guid userId, string? userIdRetrieviedFromTokenHeader, TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                if (userIdRetrieviedFromTokenHeader == null || userId.ToString() != userIdRetrieviedFromTokenHeader)
                    return
                        (
                            ServiceResponse<IEnumerable<TimelineDTO>>.Failure("You are not authorized to get this details", (int)HttpStatusCode.Unauthorized),
                            new MetaData()
                        );

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelinesByUser =
                    await _timelineCollection
                    .Find(timeline => timeline.SharedWith != null && timeline.SharedWith.Contains(userId.ToString()) && timeline.Visibility == TimelineVisibility.PUBLIC && !timeline.IsDeleted).ToListAsync();
                var orderedTimlinesByUserId = _sortHelper.ApplySort(timelinesByUser.AsQueryable(), timelineRequestParameters.OrderBy).ToList();
                var count = timelinesByUser.Count;
                var result = orderedTimlinesByUserId.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).ToList();
                var pagedList = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.SuccessResult(_mapper.Map<IEnumerable<TimelineDTO>>(pagedList), (int)HttpStatusCode.OK),
                        pagedList.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineDTO>>.Failure("Error while retrieving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<ServiceResponse<TimelineDTO>> Create(string? createdBy, CreateTimelineDTO createTimelineDTO, IFormFileCollection? files)
        {
            try
            {
                if (createdBy == null || !Guid.TryParse(createdBy, out Guid userId))
                {
                    return ServiceResponse<TimelineDTO>.Failure("Unauthorized to perform this operation", (int)HttpStatusCode.Unauthorized);
                }

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();

                var images = new List<Image>();
                if (files != null)
                {
                    var results = await _imageUploadService.UploadImagesAsync(files);
                    if (results == null || results.Count == 0) throw new Exception("There are some problem while uploading medias, try again later");
                    foreach(var result in results)
                    {
                        images.Add(new Image()
                        {
                            Name = result.FileName,
                            Notation = result.PublicId,
                            Data = result.PublicUrl,
                            Size = result.FileSize
                        });
                    }
                }


                var story = new Story()
                {
                    Title = createTimelineDTO.Story.Title,
                    Content = createTimelineDTO.Story.Content,
                    WordCount = createTimelineDTO.Story.WordCount,
                    Medias = files != null ? new Medias()
                    {
                        Images = images
                    } : null
                };

                var timeline = new Timeline()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CreatedBy = createdBy,
                    Story = story
                };
                await _timelineCollection.InsertOneAsync(timeline);
                return ServiceResponse<TimelineDTO>.SuccessResult(_mapper.Map<TimelineDTO>(timeline), (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDTO>.Failure("Error while creating timeline", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteTimeline(string timelineId, string? userIdRetrieviedFromTokenHeader)
        {
            try
            {
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse.Failure($"Invalid timeline id: {timelineId}", (int)HttpStatusCode.BadRequest);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();
                if (timeline == null) return ServiceResponse.Failure($"Requested timeline is not found or already deleted, id: {timelineId}", (int)HttpStatusCode.NotFound);

                if (userIdRetrieviedFromTokenHeader == null || timeline.CreatedBy != userIdRetrieviedFromTokenHeader)
                    return ServiceResponse.Failure("You are not authorized to perform this opeartion", (int)HttpStatusCode.Unauthorized);

                timeline.IsDeleted = true;
                timeline.SharedWith = new List<string>();
                timeline.Visibility = TimelineVisibility.PRIVATE;
                timeline.LastModified = DateTime.UtcNow;

                await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK, "Timeline deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while deleting the timeline, please try again later", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteTimelineByAdmin(string timelineId)
        {
            try
            {
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse.Failure($"Invalid timeline id: {timelineId}", (int)HttpStatusCode.BadRequest);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();
                if (timeline == null) return ServiceResponse.Failure($"Requested timeline is not found or already deleted, id: {timelineId}", (int)HttpStatusCode.NotFound);

                timeline.IsDeleted = true;
                timeline.SharedWith = new List<string>();
                timeline.Visibility = TimelineVisibility.PRIVATE;
                timeline.LastModified = DateTime.UtcNow;

                await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK, "Timeline deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while deleting the timeline, please try again later", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> PermanentDeleteTimeline(string timelineId)
        {
            try
            {
                if (!ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse.Failure($"Invalid timeline id: {timelineId}", (int)HttpStatusCode.BadRequest);

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId).FirstOrDefaultAsync();
                if (timeline == null) return ServiceResponse.Failure($"Requested timeline is not found or already deleted, id: {timelineId}", (int)HttpStatusCode.NotFound);

                await _timelineCollection.DeleteOneAsync(timeline => timeline.Id == timelineId);
                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK, "Timeline is permanently deleted");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while deleting the timeline, please try again later", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<DeleteResult>> DeleteAllUserDeletedTimelines()
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var deleteResult = await _timelineCollection.DeleteManyAsync(timeline => timeline.IsDeleted);
                if (deleteResult == null)
                    return ServiceResponse<DeleteResult>.Failure("No timeline is there for delete", (int)HttpStatusCode.NotFound);
                return ServiceResponse<DeleteResult>.SuccessResult(deleteResult, (int)HttpStatusCode.OK, "Timelines deleted permanently from db");
            }
            catch (Exception ex)
            {
                return ServiceResponse<DeleteResult>.Failure("Error while deleting try after some time", new List<string>() {ex.Message}, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<TimelineDTO>> GetUserCreatedTimelineByUser(string? userId, string timelineId)
        {
            try
            {
                if(userId == null || !ObjectId.TryParse(timelineId, out ObjectId _id) || !Guid.TryParse(userId, out Guid user))
                {
                    return ServiceResponse<TimelineDTO>.Failure("Not a valid timeline id or user id", (int)HttpStatusCode.BadRequest);
                }
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && timeline.CreatedBy == userId && !timeline.IsDeleted).FirstOrDefaultAsync();

                if(timeline == null)
                {
                    return ServiceResponse<TimelineDTO>.Failure("No timeline found", (int)HttpStatusCode.NotFound);
                }

                return ServiceResponse<TimelineDTO>.SuccessResult(_mapper.Map<TimelineDTO>(timeline), (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDTO>.Failure("Error while finding the timelien try after some time", new List<string>() {ex.Message}, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
