using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Data
{
  public class MobileContext
  {
    IMongoDatabase database; 
    IGridFSBucket gridFS;   

    public MobileContext()
    {
      string connectionString = "mongodb://localhost:27017/mobilestore";
      var connection = new MongoUrlBuilder(connectionString);
      MongoClient client = new MongoClient(connectionString);
      database = client.GetDatabase(connection.DatabaseName);
      gridFS = new GridFSBucket(database);
    }
   
    private IMongoCollection<Phone> Phones
    {
      get { return database.GetCollection<Phone>("Phones"); }
    }
   
    public async Task<IEnumerable<Phone>> GetPhones(int? minPrice, int? maxPrice, string name)
    {

      var builder = new FilterDefinitionBuilder<Phone>();
      var filter = builder.Empty; 
                                  
      if (!String.IsNullOrWhiteSpace(name))
      {
        filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
      }
      if (minPrice.HasValue)  
      {
        filter = filter & builder.Gte("Price", minPrice.Value);
      }
      if (maxPrice.HasValue)  
      {
        filter = filter & builder.Lte("Price", maxPrice.Value);
      }

      return await Phones.Find(filter).ToListAsync();
    }

    public async Task<Phone> GetPhone(string id)
    {
      return await Phones.Find(new BsonDocument("_id", new ObjectId(id)))
        .FirstOrDefaultAsync();
    }
    
    public async Task Update(Phone p)
    {
      await Phones.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
    }
    
    public async Task Remove(string id)
    {
      await Phones.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<byte[]> GetImage(string id)
    {
      return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
    }
    
    public async Task StoreImage(string id, Stream imageStream, string imageName)
    {
      Phone p = await GetPhone(id);
      if (p.HasImage())
      {
        await gridFS.DeleteAsync(new ObjectId(p.ImageId));
      }
      
      ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
      
      p.ImageId = imageId.ToString();
      var filter = Builders<Phone>.Filter.Eq("_id", new ObjectId(p.Id));
      var update = Builders<Phone>.Update.Set("ImageId", p.ImageId);
      await Phones.UpdateOneAsync(filter, update);
    }
  }
}
