using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimelineService.Models;
using TimelineService.Settings;

namespace TimelineService.MongoRepository
{
    public class AppMongoRepository
    {
        private readonly IMongoCollection<Timeline> _timelineCollection;

        public AppMongoRepository(IOptions<TimelineDbSettings> timelineDbSettings, IMongoClient client)
        {
            var db = client.GetDatabase(timelineDbSettings.Value.DatabaseName);
            _timelineCollection = db.GetCollection<Timeline>(timelineDbSettings.Value.TimelinesCollectionName);
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

        public async Task<IMongoCollection<Timeline>> GetTimelineCollectionContext()
        {
            return await AddIndex(this._timelineCollection);
        }
    }
}
