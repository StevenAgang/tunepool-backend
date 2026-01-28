using Azure.Core;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace tunepool.Repository.Configuration.Helper
{
    public class LinkExtractor
    {
        private HttpClient _httpClient;

        public LinkExtractor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Domain(string link)
        {
            var pattern = new Dictionary<string, string> {

                {@"youtube\.com","Youtube"},
                {@"music\.youtube\.com","Youtube Music"},
                {@"open\.spotify\.com","Spotify"},
                {@"soundcloud\.com","Sound Cloud"},
                {@"music\.apple\.com","Apple Music"},
                {@"deezer\.com","Deezer"},
                {@"tidal\.com","Tidal"},
                {@"music\.amazon\.com","Amazon Music"},
                {@"(mixcloud\.com)","MixCloud"}

            };
            foreach(var dom in pattern)
            {
                if(Regex.IsMatch(link, dom.Key, RegexOptions.IgnoreCase))
                {
                    return dom.Value;
                }
            }
            throw new Exception("Unsupported Platform");
        }

        public string ExtractPlaylistID(string link, string platform)
        {
            Uri uri = new Uri(link);

            if (platform == "Youtube")
            {
                var query = HttpUtility.ParseQueryString(uri.Query);
                return query["list"]!;
            }
            if(platform == "Spotify")
            {
                string[] segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                return segments[^1];
            }

            return string.Empty;
           
        }

        public async Task<string> Thumbnails(string link, string platform)
        {
            string key = "";
            string request = "";
            string id = ExtractPlaylistID(link,platform);
            if (platform == "Youtube")
            {
                key = Environment.GetEnvironmentVariable("YTAPIKEY")!;
                request = "https://youtube.googleapis.com/youtube/v3/playlists?part=snippet%2CcontentDetails&id=";
                request += id + "&key=" + key;
                return await GetYoutubeThumbnails(request);
            }
            throw new Exception("Unsupported Platform");
        }

        public async Task<string> GetYoutubeThumbnails(string request)
        {
            var result = await _httpClient.GetAsync(request);
            result.EnsureSuccessStatusCode();
            string body = await result.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body);
            string thumbail = doc.RootElement
                .GetProperty("items")[0]
                .GetProperty("snippet")
                .GetProperty("thumbnails")
                .GetProperty("maxres")
                .GetProperty("url")
                .GetString()!;

            return thumbail;
        }
    }
}
