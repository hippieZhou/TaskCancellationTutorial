namespace CoreLib;

public class MyHttpClient
{
    private const string RequestUri = "https://www.bing.com/";

    private readonly HttpClient _client;

    public MyHttpClient(HttpClient client)
    {
        _client = client;
    }

    public string GetHtmlString() => GetHtmlStringAsync().Result;
    public async Task<string> GetHtmlStringAsync() => await _client.GetStringAsync(RequestUri);
}

