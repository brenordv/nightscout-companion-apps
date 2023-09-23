using MongoDB.Driver;

namespace Raccoon.Ninja.Extensions.MongoDb.Builders;

public class MongoCollectionBuilder
{
    private string _connectionString;
    private string _databaseName;
    private string _collectionName;

    public MongoCollectionBuilder AddConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));

        _connectionString = connectionString;
        return this;
    }

    public MongoCollectionBuilder AddDatabaseName(string databaseName)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseName));
        
        _databaseName = databaseName;
        return this;
    }
    
    public MongoCollectionBuilder AddCollectionName(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(collectionName));
        
        _collectionName = collectionName;
        return this;
    }
    
    public IMongoCollection<T> Build<T>()
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        return database.GetCollection<T>(_collectionName);
    }
    
}