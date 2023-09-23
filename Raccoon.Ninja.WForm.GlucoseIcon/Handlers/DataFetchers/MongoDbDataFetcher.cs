using MongoDB.Driver;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.Builders;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Handlers.DataFetchers;

public class MongoDbDataFetcher: IDataFetcher
{
    public event Action<DataFetchResult> OnDataFetched;
    
    private readonly IMongoCollection<NightScoutMongoDocument> _collection;

    public MongoDbDataFetcher(string connectionString, string databaseName, string collectionName)
    {
        _collection = new MongoCollectionBuilder()
            .AddConnectionString(connectionString)
            .AddDatabaseName(databaseName)
            .AddCollectionName(collectionName)
            .Build<NightScoutMongoDocument>();
    }


    public async Task FetchDataAsync()
    {
        try
        {
            GlucoseReading latestDoc = _collection.GetLatestDocument();

            if (latestDoc is null)
            {
                OnDataFetched?.Invoke(DataFetchResult.FromError("No data found in MongoDb"));
                return;
            }
            
            OnDataFetched?.Invoke(new DataFetchResult(latestDoc.Value, latestDoc.Trend));
        }
        catch (Exception e)
        {
            OnDataFetched?.Invoke(DataFetchResult.FromError(e.Message));
        }
    }

    
}