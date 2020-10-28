using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SM.Libs.Utils
{
    public static class DefaultHttpUtility 
    {
        public static Task<string> GetHtmlStringAsync(string url)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();
                string httpsUrl = url;
                if (url.StartsWith("http://"))
                {
                    httpsUrl = url.Replace("http://", "https://");
                }

                if (!httpsUrl.StartsWith("https://"))
                {
                    httpsUrl = "https://" + url;
                }
                HttpResponseMessage hsm = httpClient.GetAsync(new Uri(httpsUrl)).Result;
                return hsm.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SMation");
            httpClient.Timeout = TimeSpan.FromMinutes(3);
            return httpClient;
        }
    }
}