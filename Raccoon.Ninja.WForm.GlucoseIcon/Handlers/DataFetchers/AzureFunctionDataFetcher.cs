using Raccoon.Ninja.Domain.Core.Models;
using Raccoon.Ninja.Extensions.Desktop.Logging;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;
using Refit;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Handlers.DataFetchers;

public class AzureFunctionDataFetcher : IDataFetcher
{
    public event Action<DataFetchResult> OnDataFetched;

    private readonly string _functionKey;
    private readonly GetDataRequest _postPayload;
    private readonly IApiClient _apiClient;

    public AzureFunctionDataFetcher(string functionUrl, string functionKey, GetDataRequest postPayload)
    {
        Logger.LogTrace("Initializing AzureFunctionDataFetcher instance");
        _functionKey = functionKey;
        _postPayload = postPayload;
        _apiClient = RestService.For<IApiClient>(functionUrl);
    }

    public async Task FetchDataAsync()
    {
        try
        {
            Logger.LogTrace("Fetching data from Azure Function");
            // Make the API call
            var response = await _apiClient.PostDataAsync(_postPayload, _functionKey);

            // Check if the response was successful
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("Error while fetching data from Azure Function. Reason: {ReasonPhrase}", null,
                    response.ReasonPhrase);
                OnDataFetched?.Invoke(DataFetchResult.FromError(response.ReasonPhrase));
                return;
            }

            // Save content in a variable for easy access
            var body = response.Content;

            // Check if the body is null
            if (body is null)
            {
                Logger.LogError("Fetch data was successful, but no body was returned", null);
                OnDataFetched?.Invoke(DataFetchResult.FromError("No body returned from the API."));
                return;
            }

            // Success!
            Logger.LogTrace("Data fetched successfully. Value: {Value}, Trend: {Trend}", body.Value, body.Trend);
            OnDataFetched?.Invoke(new DataFetchResult(body.Value, body.Trend));
        }
        catch (Exception ex)
        {
            OnDataFetched?.Invoke(DataFetchResult.FromError(ex.Message));
        }
    }
}