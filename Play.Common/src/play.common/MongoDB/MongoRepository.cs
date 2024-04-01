
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Play.common.MongoDB;
public class MongoRepository<T> : IRepository<T> where T: IEntity
{
    private readonly IMongoCollection<T> dbconnection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
    public MongoRepository(IMongoDatabase database, string collectionName )
    {
        dbconnection = database.GetCollection<T>(collectionName);
    }
    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await dbconnection.Find(filterBuilder.Empty).ToListAsync();
    }
    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await dbconnection.Find(filter).ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await dbconnection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task<T> GetAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
        return await dbconnection.Find(filter).FirstOrDefaultAsync();
    }


    public async Task CreateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentException();
        await dbconnection.InsertOneAsync(entity);
    }


    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentException();
        FilterDefinition<T> filter = filterBuilder.Eq(exisingEntity => exisingEntity.Id, entity.Id);
        await dbconnection.ReplaceOneAsync(filter, entity);
    }


    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
        await dbconnection.DeleteOneAsync(filter);
    }

   
}

