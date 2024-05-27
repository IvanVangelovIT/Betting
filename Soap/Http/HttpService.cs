namespace Soap.Http;

public class HttpService
{
    public async Task<string> FetchXmlDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            
            var responseBody = await response.Content.ReadAsStringAsync();
            
            return responseBody;
        }
    }
}