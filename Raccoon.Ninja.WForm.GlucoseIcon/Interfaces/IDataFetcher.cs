using Raccoon.Ninja.WForm.GlucoseIcon.Models;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;

public interface IDataFetcher
{
    Task FetchDataAsync();
    event Action<DataFetchResult> OnDataFetched;
}