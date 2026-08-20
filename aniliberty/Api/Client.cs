using aniliberty.Api.Data.Releases;
using aniliberty.Api.Data.Schedule;
using aniliberty.Api.Requests;
using aniliberty.Api.Responses;
using aniliberty.Helpers;
using Flurl;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace aniliberty.Api;

internal class Client
{
    const string API_BASE = @"https://aniliberty.top/api/v1";
    //const string API_BASE = @"https://anilibria.top/api/v1";

    private readonly HttpService service = new();

    public async Task<List<ReleaseLatest>> GetLatestReleases(int Limit = 5)
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/anime/releases/latest")
            .SetQueryParams(new
            {
                limit = Limit
            });

        Debugger.WriteLine($"get latest releases by limit: {url}", DebuggerCategory.API);

        var response = await service.GetAsync(url);

        response.Validate();

        return JsonSerializer.Deserialize<List<ReleaseLatest>>(response.Content);
    }

    public async Task<LoginResponse> SignIn(string username, string password)
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/accounts/users/auth/login");

        Debugger.WriteLine($"login user: {username}", DebuggerCategory.API);

        var data = JsonSerializer.Serialize(new LoginRequest
        {
            Login = username,
            Password = password
        });

        var response = await service.PostAsync(url, data, "application/json");

        response.Validate();

        return JsonSerializer.Deserialize<LoginResponse>(response.Content);
    }

    public async Task<ScheduleNow> GetScheduleNow()
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/anime/schedule/now");

        Debugger.WriteLine($"get schedule for now: {url}", DebuggerCategory.API);

        var response = await service.GetAsync(url);

        response.Validate();

        return JsonSerializer.Deserialize<ScheduleNow>(response.Content);
    }

    public async Task<ReleaseDetail> GetRelease(int id)
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/anime/releases")
            .AppendPathSegment(id);

        Debugger.WriteLine($"get release by id: {url}", DebuggerCategory.API);

        var response = await service.GetAsync(url);

        response.Validate();

        return JsonSerializer.Deserialize<ReleaseDetail>(response.Content);
    }

    public async Task<ReleaseDetail> GetRelease(string alias)
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/anime/releases")
            .AppendPathSegment(alias);

        Debugger.WriteLine($"get release by alias: {url}", DebuggerCategory.API);

        var response = await service.GetAsync(url);

        response.Validate();

        return JsonSerializer.Deserialize<ReleaseDetail>(response.Content);
    }

    public async Task<bool> GetStatus()
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/app/status");

        var response = await service.GetAsync(url);

        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<List<Release>> Search(string queryString)
    {
        var url = new Url(API_BASE)
            .AppendPathSegment("/app/search/releases")
            .SetQueryParams(new
            {
                query = queryString
            });

        var response = await service.GetAsync(url);

        response.Validate();

        return JsonSerializer.Deserialize<List<Release>>(response.Content);
    }
}
