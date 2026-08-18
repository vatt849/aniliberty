using aniliberty.Api.Exceptions;
using aniliberty.Api.Responses;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace aniliberty.Helpers;

public class HttpService
{
    private readonly HttpClient _client;

    public HttpService()
    {
        SocketsHttpHandler handler = new()
        {
            AutomaticDecompression = DecompressionMethods.All,
            AllowAutoRedirect = true,
            ConnectTimeout = TimeSpan.FromSeconds(30),
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        };

        _client = new HttpClient(handler);
        //{
        //    Timeout = TimeSpan.FromSeconds(30)
        //};
    }

    public void SetBearer(string bearer)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
    }

    public async Task<HttpResponse> GetAsync(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return new HttpResponse()
            {
                Content = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            };
        }
        catch (TaskCanceledException e)
        {
            return new HttpResponse()
            {
                Content = e.Message,
                StatusCode = HttpStatusCode.RequestTimeout
            };
        }
        catch (HttpRequestException e)
        {
            return new HttpResponse()
            {
                Content = e.Message,
                StatusCode = e.StatusCode ?? HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<HttpResponse> PostAsync(string uri, string data, string contentType)
    {
        using HttpContent content = new StringContent(data, Encoding.UTF8, contentType);

        HttpRequestMessage requestMessage = new()
        {
            Content = content,
            Method = HttpMethod.Post,
            RequestUri = new Uri(uri)
        };

        using HttpResponseMessage response = await _client.SendAsync(requestMessage);

        return new HttpResponse()
        {
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = response.StatusCode
        };
    }

    public string ToQueryString(NameValueCollection nvc)
    {
        var array = (
            from key in nvc.AllKeys
            from value in nvc.GetValues(key)
            select string.Format(
                "{0}={1}",
                key,
                value
            )
            //HttpUtility.UrlEncode(key),
            //HttpUtility.UrlEncode(value))
            ).ToArray();

        return "?" + string.Join("&", array);
    }
}

public class HttpResponse
{
    public string Content { get; set; } = "";
    public HttpStatusCode StatusCode { get; set; }

    public void Validate()
    {
        if (StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        Debugger.WriteLine($"[ERR] {Content}");
        string message;
        try
        {
            var errResp = JsonSerializer.Deserialize<ErrorResponse>(Content);

            message = errResp?.Message ?? "Unknown error";
        }
        catch
        {
            message = Content;
        }


        throw new ApiException(message, (int)StatusCode);
    }
}
