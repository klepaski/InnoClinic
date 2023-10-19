using DocumentsAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DocumentsAPI.Services
{
    public interface IPhotoService
    {
        public Task<List<Photo>> GetAll();
        public Task<Photo> GetById(string id);
        public Task Create(Photo photo);
        public Task Delete(string id);
    }

    public class PhotoService : IPhotoService
    {
        private readonly IMongoCollection<Photo> _photoCollection;

        public PhotoService(IOptions<MongoDbSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _photoCollection = database.GetCollection<Photo>("Photos");
        }

        public async Task<List<Photo>> GetAll()
        {
            return await _photoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Photo> GetById(string id)
        {
            return await _photoCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Photo photo)
        {
            await _photoCollection.InsertOneAsync(photo);
        }

        public async Task Delete(string id)
        {
            FilterDefinition<Photo> filter = Builders<Photo>.Filter.Eq("Id", id);
            await _photoCollection.DeleteOneAsync(filter);
        }
    }
}
