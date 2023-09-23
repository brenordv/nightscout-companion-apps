using Raccoon.Ninja.Domain.Core.Models;
using Refit;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;

public interface IApiClient
{
    [Post("")]
    Task<ApiResponse<GlucoseReadingResponse>> PostDataAsync([Body] GetDataRequest payload, [Query][AliasAs("code")] string code);
}