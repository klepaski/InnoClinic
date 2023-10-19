using DocumentsAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DocumentsAPI.Services
{
    public interface IDocumentService
    {
        public Task<List<Document>> GetAll();
        public Task<Document> GetById(string id);
        public Task Create(Document document);
        public Task Delete(string id);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IMongoCollection<Document> _documentCollection;

        public DocumentService(IOptions<MongoDbSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _documentCollection = database.GetCollection<Document>("Documents");
        }

        public async Task<List<Document>> GetAll()
        {
            return await _documentCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Document> GetById(string id)
        {
            return await _documentCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Document document)
        {
            await _documentCollection.InsertOneAsync(document);
        }

        public async Task Delete(string id)
        {
            FilterDefinition<Document> filter = Builders<Document>.Filter.Eq("Id", id);
            await _documentCollection.DeleteOneAsync(filter);
        }
    }
}
