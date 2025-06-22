using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceResponseShared;
using System.Net;
using TimelineService.Models;
using TimelineService.Settings;

namespace TimelineService.Services
{
    public class StoryService : IStoryService
    {
        private readonly IMongoCollection<Timeline> _timelineCollection;

        public StoryService(IOptions<TimelineDbSettings> timelineDbSettings, IMongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase(timelineDbSettings.Value.DatabaseName);
            _timelineCollection = db.GetCollection<Timeline>(timelineDbSettings.Value.TimelinesCollectionName);
        }

        public async Task<ServiceResponse<ReplaceOneResult>> UpdateStoryInTimeline(string timelineId, Story story, string? userIdRetrieviedFromTokenHeader)
        {
            try
            {
                if (ObjectId.TryParse(timelineId, out ObjectId objectId))
                    return ServiceResponse<ReplaceOneResult>.Failure($"Not a valid timeline id: {timelineId}", (int)HttpStatusCode.BadRequest);

                var timeline = await _timelineCollection.Find(timeline => timeline.Id == timelineId && !timeline.IsDeleted).FirstOrDefaultAsync();

                if (timeline == null)
                    return ServiceResponse<ReplaceOneResult>.Failure($"No such timeline found with given id: {timelineId}", (int)HttpStatusCode.NotFound);

                if (userIdRetrieviedFromTokenHeader == null || timeline.CreatedBy != userIdRetrieviedFromTokenHeader)
                    return ServiceResponse<ReplaceOneResult>.Failure("You have no permission to perform this operation", (int)HttpStatusCode.Unauthorized);

                timeline.Story.Title = story.Title.Trim();
                timeline.Story.Content = story.Content.Trim();

                if (story.Medias != null)
                {
                    if (timeline.Story.Medias != null)
                    {
                        if (story.Medias.Images != null)
                        {
                            timeline.Story.Medias.Images = story.Medias.Images;
                        }
                    }
                    else
                    {
                        timeline.Story.Medias = story.Medias;
                    }
                }

                timeline.Story.WordCount = story.WordCount;
                timeline.LastModified = DateTime.UtcNow;

                var replaceOneResult = await _timelineCollection.ReplaceOneAsync(timeline => timeline.Id == timelineId, timeline);
                return ServiceResponse<ReplaceOneResult>.SuccessResult(replaceOneResult, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReplaceOneResult>.Failure("Error while updating the story", new List<string> { ex.Message }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
