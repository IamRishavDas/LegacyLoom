using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimelineService.Models;
using TimelineService.Settings;

namespace TimelineService.MongoRepository
{
    public class AppMongoRepository
    {
        private readonly IMongoCollection<Timeline> _timelineCollection;
        private readonly IMongoCollection<TimelineDraft> _timelineDraftCollection;

        public AppMongoRepository(IOptions<TimelineDbSettings> timelineDbSettings, IOptions<TimelineDraftDbSettings> timelineDraftDbSettings, IMongoClient client)
        {
            var db = client.GetDatabase(timelineDbSettings.Value.DatabaseName);
            _timelineCollection = db.GetCollection<Timeline>(timelineDbSettings.Value.TimelinesCollectionName);

            var dbDraft = client.GetDatabase(timelineDraftDbSettings.Value.DatabaseName);
            _timelineDraftCollection = dbDraft.GetCollection<TimelineDraft>(timelineDraftDbSettings.Value.TimelineDraftsCollectionName);
        }

        private static async Task<IMongoCollection<Timeline>> AddIndex(IMongoCollection<Timeline> timelineCollectionContext)
        {

            var indexDefs = new List<IndexKeysDefinition<Timeline>>
            {
                Builders<Timeline>.IndexKeys.Ascending(timeline => timeline.Id),
                Builders<Timeline>.IndexKeys.Ascending(timeline => timeline.CreatedBy),
                Builders<Timeline>.IndexKeys.Ascending(timeline => timeline.SharedWith),
                Builders<Timeline>.IndexKeys.Ascending(timeline => timeline.Story.Title),
                Builders<Timeline>.IndexKeys.Ascending(timeline => timeline.Story.Content)
            };
            var createIndexModels = indexDefs.Select(key => new CreateIndexModel<Timeline>(key));

            await timelineCollectionContext.Indexes.CreateManyAsync(createIndexModels);
            return timelineCollectionContext;
        }

        private static async Task<IMongoCollection<TimelineDraft>> AddIndexToTimelineDraftCollection(IMongoCollection<TimelineDraft> timelineDraftCollectionContext)
        {
            var indexDefs = new List<IndexKeysDefinition<TimelineDraft>>()
            {
                Builders<TimelineDraft>.IndexKeys.Ascending(timelineDraft => timelineDraft.Id),
                Builders<TimelineDraft>.IndexKeys.Ascending(timelineDraft => timelineDraft.Title),
                Builders<TimelineDraft>.IndexKeys.Ascending(timelineDraft => timelineDraft.Content)
            };

            var createIndexModels = indexDefs.Select(key => new CreateIndexModel<TimelineDraft>(key));

            await timelineDraftCollectionContext.Indexes.CreateManyAsync(createIndexModels);
            return timelineDraftCollectionContext;
        }

        public async Task<IMongoCollection<Timeline>> GetTimelineCollectionContext()
        {
            return await AddIndex(this._timelineCollection);
        }

        public async Task<IMongoCollection<TimelineDraft>> GetTimelineDraftCollectionContext()
        {
            return await AddIndexToTimelineDraftCollection(this._timelineDraftCollection);
        }
    }
}
