using Raccoon.Ninja.Domain.Core.Models;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;
using Refit;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Handlers.DataFetchers;

public class AzureFunctionDataFetcher: IDataFetcher
{
    public event Action<DataFetchResult> OnDataFetched;
   
    private readonly string _functionKey;
    private readonly GetDataRequest _postPayload;
    private readonly IApiClient _apiClient;

    public AzureFunctionDataFetcher(string functionUrl, string functionKey, GetDataRequest postPayload)
    {
        _functionKey = functionKey;
        _postPayload = postPayload;
        _apiClient = RestService.For<IApiClient>(functionUrl);
    }

    public async Task FetchDataAsync()
    {
        try
        {
            // Make the API call
            var response = await _apiClient.PostDataAsync(_postPayload, _functionKey);

            // Check if the response was successful
            if (!response.IsSuccessStatusCode)
            {
                OnDataFetched?.Invoke(DataFetchResult.FromError(response.ReasonPhrase));
                return;
            }
            
            // Save content in a variable for easy access
            var body = response.Content;

            // Check if the body is null
            if (body is null)
            {
                OnDataFetched?.Invoke(DataFetchResult.FromError("No body returned from the API."));
                return;
            }
            
            // Success!
            OnDataFetched?.Invoke(new DataFetchResult(body.Value, body.Trend));
        }
        catch (Exception ex)
        {
            OnDataFetched?.Invoke(DataFetchResult.FromError(ex.Message));
        }
    }
    
}