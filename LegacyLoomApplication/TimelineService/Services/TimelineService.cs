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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public TimelineService(AppMongoRepository mongoRepository, IMapper mapper, ISortHelper<Timeline> sortHelper, IImageService imageService)
        {
            _mongoRepository = mongoRepository;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<(ServiceResponse<IEnumerable<TimelineDTO>>, MetaData)> GetAll(TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(s => !s.IsDeleted && s.Visibility != TimelineVisibility.PRIVATE).ToListAsync();
                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).Take(timelineRequestParameters.PageSize).ToList();

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
                

                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var n_result = orderedTimelines.Skip(((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize));
                var result = n_result.Take(timelineRequestParameters.PageSize).ToList();

                foreach(var timeline in result)
                {
                    timeline.Story.Content = new String(timeline.Story.Content.Take(150).ToArray());
                }

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                var listOfTimeline = _mapper.Map<IEnumerable<Timeline>>(pagedListTimelines);
                var listOfTimelineDto = _mapper.Map<List<TimelineDTO>>(listOfTimeline);

                
                for (int i = 0; i < result.Count; i++)
                {
                    listOfTimelineDto[i].IsLikedByMe = result[i].Likes == null ? false : result[i].Likes.Contains(userId);
                    listOfTimelineDto[i].IsDislikedByMe = result[i].Dislikes == null ? false : result[i].Dislikes.Contains(userId);
                }

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

        public async Task<(ServiceResponse<IEnumerable<TimelineLookupDTO>>, MetaData)> GetAllPublicTimelinesLookup(string? userId, TimelineRequestParameters timelineRequestParameters)
        {
            try
            {
                if (userId == null || !Guid.TryParse(userId, out Guid _userId))
                    return
                        (
                            ServiceResponse<IEnumerable<TimelineLookupDTO>>.Failure("You are unauthorized", (int)HttpStatusCode.Unauthorized),
                            new MetaData()
                        );
                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timelines = await _timelineCollection.Find(timeline => timeline.Visibility != TimelineVisibility.PRIVATE && !timeline.IsDeleted).ToListAsync();
                var orderedTimelines = _sortHelper.ApplySort(timelines.AsQueryable<Timeline>(), timelineRequestParameters.OrderBy);

                var count = timelines.Count;
                var result = orderedTimelines.Skip(((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize)).Take(timelineRequestParameters.PageSize).ToList();

                var pagedListTimelines = PagedList<Timeline>.ToPagedList(result, count, timelineRequestParameters.PageNumber, timelineRequestParameters.PageSize);
                var timelineDTOs = _mapper.Map<List<TimelineLookupDTO>>(pagedListTimelines);

                for (int i = 0; i < result.Count; i++)
                {
                    timelineDTOs[i].IsLikedByMe = result[i].Likes == null ? false : result[i].Likes.Contains(userId);
                    timelineDTOs[i].IsDislikedByMe = result[i].Dislikes == null ? false : result[i].Dislikes.Contains(userId);
                }

                return
                    (
                        ServiceResponse<IEnumerable<TimelineLookupDTO>>.SuccessResult
                        (
                            timelineDTOs,
                            (int)HttpStatusCode.OK
                        ),
                        pagedListTimelines.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<TimelineLookupDTO>>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<ServiceResponse<TimelineDTO>> GetPublicTimelineByTimelineId(string timelineId, string? userId)
        {
            try
            {
                if (timelineId == null || !ObjectId.TryParse(timelineId, out ObjectId _id))
                {
                    return ServiceResponse<TimelineDTO>.Failure("Invalid timeline id", (int)HttpStatusCode.BadRequest);
                }

                if(userId == null || !Guid.TryParse(userId, out Guid _userId))
                {
                    return ServiceResponse<TimelineDTO>.Failure("Invalid user id", (int)HttpStatusCode.BadRequest);
                }

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && timeline.Visibility != TimelineVisibility.PRIVATE && !timeline.IsDeleted).FirstOrDefaultAsync();
                if(timeline == null)
                {
                    return ServiceResponse<TimelineDTO>.Failure("No such timeline found or deleted", (int)HttpStatusCode.NotFound);
                }

                var timelineDTO = _mapper.Map<TimelineDTO>(timeline);

                timelineDTO.IsLikedByMe = timeline.Likes == null ? false : timeline.Likes.Contains(userId);
                timelineDTO.IsDislikedByMe = timeline.Dislikes == null ? false : timeline.Dislikes.Contains(userId);

                return ServiceResponse<TimelineDTO>.SuccessResult(timelineDTO, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDTO>.Failure("Error while retireving the timeline details", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
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
                var result = orderedTimelines.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).Take(timelineRequestParameters.PageSize).ToList();

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
                var result = orderedTimlinesByUserId.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).Take(timelineRequestParameters.PageSize).ToList();
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
                var result = orderedTimlinesByUserId.Skip((timelineRequestParameters.PageNumber - 1) * timelineRequestParameters.PageSize).Take(timelineRequestParameters.PageSize).ToList();
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
                if (files != null && files.Count != 0)
                {
                    if(files.Count > 4)
                    {
                        return ServiceResponse<TimelineDTO>.Failure("You can upload 4 images", (int)HttpStatusCode.BadRequest);
                    }
                    var results = await _imageService.UploadImagesAsync(files);
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
                    Story = story,
                    Likes = new HashSet<string>(),
                    Dislikes = new HashSet<string>()
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

                if(timeline.Story.Medias != null && timeline.Story.Medias.Images != null && timeline.Story.Medias.Images.Count != 0)
                {
                    var images = timeline.Story.Medias.Images;
                    var publicIds = new List<string>();
                    foreach(var image in images)
                    {
                        publicIds.Add(image.Notation);
                    }
                    var iamgeDeletionResults = _imageService.DeleteImagesAsync(publicIds);
                }


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

                var timelineDTO = _mapper.Map<TimelineDTO>(timeline);

                timelineDTO.IsLikedByMe = timeline.Likes == null ? false : timeline.Likes.Contains(userId);
                timelineDTO.IsDislikedByMe = timeline.Dislikes == null ? false : timeline.Dislikes.Contains(userId);

                return ServiceResponse<TimelineDTO>.SuccessResult(timelineDTO, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TimelineDTO>.Failure("Error while finding the timelien try after some time", new List<string>() {ex.Message}, (int)HttpStatusCode.InternalServerError);
            }
        }

        //public async Task<ServiceResponse<bool>> LikeToggle(string? userId, string timelineId)
        //{
        //    try
        //    {
        //        if (userId == null || !Guid.TryParse(userId, out Guid _userId) || !ObjectId.TryParse(timelineId, out ObjectId _id))
        //        {
        //            return ServiceResponse<bool>.Failure("Not a valid timeline id or user id", (int)HttpStatusCode.BadRequest);
        //        }

        //        var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
        //        var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();

        //        if (timeline == null)
        //        {
        //            return ServiceResponse<bool>.Failure("No timeline found", (int)HttpStatusCode.NotFound);
        //        }

        //        // the response in the boolean state represent wheather the timeline is liked or not
        //        if(timeline.Likes != null)
        //        {
        //            if (timeline.Likes.Contains(userId))
        //            {
        //                timeline.Likes.Remove(userId);
        //                var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
        //                return ServiceResponse<bool>.SuccessResult(false, (int)HttpStatusCode.OK);
        //            } 
        //            else
        //            {
        //                if(timeline.Dislikes != null && timeline.Dislikes.Contains(userId))
        //                {
        //                    timeline.Dislikes.Remove(userId);
        //                }
        //                timeline.Likes.Add(userId);
        //                var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
        //                return ServiceResponse<bool>.SuccessResult(true, (int)HttpStatusCode.OK);
        //            }
        //        }
        //        else
        //        {
        //            if (timeline.Dislikes != null && timeline.Dislikes.Contains(userId))
        //            {
        //                timeline.Dislikes.Remove(userId);
        //            }
        //            timeline.Likes = new HashSet<string>();
        //            timeline.Likes.Add(userId);
        //            var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
        //            return ServiceResponse<bool>.SuccessResult(true, (int)HttpStatusCode.OK);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return ServiceResponse<bool>.Failure("Error while finding the timelien try after some time", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
        //    }
        //}

        //public async Task<ServiceResponse<bool>> DislikeToggle(string? userId, string timelineId)
        //{
        //    try
        //    {
        //        if (userId == null || !Guid.TryParse(userId, out Guid _userId) || !ObjectId.TryParse(timelineId, out ObjectId _id))
        //        {
        //            return ServiceResponse<bool>.Failure("Not a valid timeline id or user id", (int)HttpStatusCode.BadRequest);
        //        }

        //        var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
        //        var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();

        //        if (timeline == null)
        //        {
        //            return ServiceResponse<bool>.Failure("No timeline found", (int)HttpStatusCode.NotFound);
        //        }

        //        // the response in the boolean state represent wheather the timeline is disliked or not
        //        if(timeline.Dislikes != null)
        //        {
        //            if (timeline.Dislikes.Contains(userId))
        //            {
        //                timeline.Dislikes.Remove(userId);
        //                var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);

        //                return ServiceResponse<bool>.SuccessResult(false, (int)HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                if(timeline.Likes != null && timeline.Likes.Contains(userId))
        //                {
        //                    timeline.Likes.Remove(userId);
        //                }
        //                timeline.Dislikes.Add(userId);
        //                var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);

        //                return ServiceResponse<bool>.SuccessResult(true, (int)HttpStatusCode.OK);
        //            }
        //        }
        //        else
        //        {
        //            if (timeline.Likes != null && timeline.Likes.Contains(userId))
        //            {
        //                timeline.Likes.Remove(userId);
        //            }
        //            timeline.Dislikes = new HashSet<string>();
        //            timeline.Dislikes.Add(userId);
        //            var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);

        //            return ServiceResponse<bool>.SuccessResult(true, (int)HttpStatusCode.OK);
        //        }
                

        //    }
        //    catch (Exception ex)
        //    {
        //        return ServiceResponse<bool>.Failure("Error while finding the timelien try after some time", new List<string>() { ex.Message }, (int)HttpStatusCode.InternalServerError);
        //    }
        //}


        public async Task<ServiceResponse<LikeResponse>> LikeToggle(string? userId, string timelineId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid _userId) || !ObjectId.TryParse(timelineId, out ObjectId _id))
                {
                    return ServiceResponse<LikeResponse>.Failure("Not a valid timeline id or user id", (int)HttpStatusCode.BadRequest);
                }

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(t => t.Id == timelineId && !t.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null)
                {
                    return ServiceResponse<LikeResponse>.Failure("No timeline found", (int)HttpStatusCode.NotFound);
                }

                timeline.Likes = timeline.Likes ?? new HashSet<string>();
                timeline.Dislikes = timeline.Dislikes ?? new HashSet<string>();

                bool isLiked = timeline.Likes.Contains(userId);
                var updateBuilder = Builders<Timeline>.Update;

                if (isLiked)
                {
                    // Unlike: Remove user from Likes
                    var update = updateBuilder.Pull(t => t.Likes, userId);
                    await _timelineCollection.UpdateOneAsync(t => t.Id == timelineId, update);
                }
                else
                {
                    // Like: Add user to Likes, remove from Dislikes
                    var update = updateBuilder
                        .AddToSet(t => t.Likes, userId)
                        .Pull(t => t.Dislikes, userId);
                    await _timelineCollection.UpdateOneAsync(t => t.Id == timelineId, update);
                }

                // Fetch updated timeline to get accurate counts
                var updatedTimeline = await _timelineCollection.Find(t => t.Id == timelineId).FirstOrDefaultAsync();
                return ServiceResponse<LikeResponse>.SuccessResult(
                    new LikeResponse
                    {
                        IsLiked = !isLiked,
                        Likes = updatedTimeline.Likes?.Count ?? 0,
                        Dislikes = updatedTimeline.Dislikes?.Count ?? 0
                    },
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                return ServiceResponse<LikeResponse>.Failure(
                    "Error while updating the timeline, try again later",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ServiceResponse<DislikeResponse>> DislikeToggle(string? userId, string timelineId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid _userId) || !ObjectId.TryParse(timelineId, out ObjectId _id))
                {
                    return ServiceResponse<DislikeResponse>.Failure("Not a valid timeline id or user id", (int)HttpStatusCode.BadRequest);
                }

                var _timelineCollection = await _mongoRepository.GetTimelineCollectionContext();
                var timeline = await _timelineCollection.Find(t => t.Id == timelineId && !t.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null)
                {
                    return ServiceResponse<DislikeResponse>.Failure("No timeline found", (int)HttpStatusCode.NotFound);
                }

                timeline.Likes = timeline.Likes ?? new HashSet<string>();
                timeline.Dislikes = timeline.Dislikes ?? new HashSet<string>();

                bool isDisliked = timeline.Dislikes.Contains(userId);
                var updateBuilder = Builders<Timeline>.Update;

                if (isDisliked)
                {
                    // Undislike: Remove user from Dislikes
                    var update = updateBuilder.Pull(t => t.Dislikes, userId);
                    await _timelineCollection.UpdateOneAsync(t => t.Id == timelineId, update);
                }
                else
                {
                    // Dislike: Add user to Dislikes, remove from Likes
                    var update = updateBuilder
                        .AddToSet(t => t.Dislikes, userId)
                        .Pull(t => t.Likes, userId);
                    await _timelineCollection.UpdateOneAsync(t => t.Id == timelineId, update);
                }

                // Fetch updated timeline to get accurate counts
                var updatedTimeline = await _timelineCollection.Find(t => t.Id == timelineId).FirstOrDefaultAsync();
                return ServiceResponse<DislikeResponse>.SuccessResult(
                    new DislikeResponse
                    {
                        IsDisliked = !isDisliked,
                        Likes = updatedTimeline.Likes?.Count ?? 0,
                        Dislikes = updatedTimeline.Dislikes?.Count ?? 0
                    },
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                return ServiceResponse<DislikeResponse>.Failure(
                    "Error while updating the timeline, try again later",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }
    }
}
