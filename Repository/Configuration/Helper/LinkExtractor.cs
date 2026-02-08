using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using tunepool.Repository.Interface.serviceProviderTokenInterface;

namespace tunepool.Repository.Configuration.Helper
{
    // This class validate playlist services and extract its thumbnail
    public class LinkExtractor
    {
        private HttpClient _httpClient;
        private IServiceProviderToken _serviceProviderToken;

        public LinkExtractor(HttpClient httpClient, IServiceProviderToken serviceProviderToken)
           
        {
            _httpClient = httpClient;
            _serviceProviderToken = serviceProviderToken;
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

            switch (platform)
            {
                case "Youtube":
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    return query["list"]!;
                case "Spotify":
                case "Deezer":
                case "Tidal":
                    string[] segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    return segments[^1];
                default:
                    return string.Empty;
            }
           
        }

        public async Task<string> Thumbnails(string link, string platform)
        {
            Random rand = new Random();
            string[] defaultThumbnail =
            {
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688753/7082431_dsxuee.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688495/autumn-scene-street-food-stall-with-customers_bzmrmq.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688490/asdasdasd_jzdmvb.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688487/couple-bus-sunset_myzpgw.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688485/nature-landscape-hawaii-with-digital-art-style_a8ojtg.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688485/lifestyle-scene-with-people-doing-regular-tasks-anime-style_lqocag.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769689018/8-bit-graphics-pixels-scene-with-person-bench-sunset_rdxrjg.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688485/anime-style-house-architecture_b4nr2t.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688481/7082421_suxim7.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688481/6884605_yysz29.jpg"
            };
            int index = rand.Next(0, defaultThumbnail.Length);
            string request = "";
            string id = ExtractPlaylistID(link,platform);
            string thumbanail = "";

            if (platform == "Youtube" || platform == "Youtube Music")
            {
                string key = Environment.GetEnvironmentVariable("YTAPIKEY")!;
                request = "https://youtube.googleapis.com/youtube/v3/playlists?part=snippet%2CcontentDetails&id=";
                request += id + "&key=" + key;
                thumbanail = await GetYoutubeThumbnail(request);
            }
            if (platform == "Sound Cloud")
            {
                string clientId = Environment.GetEnvironmentVariable("SCCLIENTID")!;
                string secretId = Environment.GetEnvironmentVariable("SCSECRETID")!;

                string escapeUrl = Uri.EscapeDataString(link);
                request = $"https://api.soundcloud.com/resolve?url={escapeUrl}" ;
                thumbanail = await GetSoundCloudThumbnail(request, clientId, secretId, platform);

            }
            if(platform == "Deezer")
            {
                request = $"https://api.deezer.com/playlist/{id}";
                thumbanail = await GetDeezerThumbnail(request);
            }
            if(platform == "Tidal")
            {
                string clientId = Environment.GetEnvironmentVariable("TLCLIENTID")!;
                string secretId = Environment.GetEnvironmentVariable("TLSECRETID")!;
                request = $"https://openapi.tidal.com/v2/playlists/{id}?include=coverArt";
                thumbanail = await GetTidalThumbnail(request,clientId,secretId,platform);
            }
            if (platform == "Spotify")
            {
                string bearer = Environment.GetEnvironmentVariable("SPTYKEY")!;
                request = $"https://api.spotify.com/v1/playlists/{id}";
                thumbanail = await GetSpotifyThumbnail(request, bearer, platform, defaultThumbnail, index);
            }
            
            if (string.IsNullOrEmpty(thumbanail))
            {
                return defaultThumbnail[index];
            }
            return thumbanail;
        }

        public async Task<string> GetYoutubeThumbnail(string request)
        {
            var result = await _httpClient.GetAsync(request);
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

        public async Task<string> GetSoundCloudThumbnail(string request, string clientId, string secretId, string platform)
        {

            var soundCloud = await _serviceProviderToken.GetAccessToken(platform);
            string authString = clientId + ":" + secretId;
            string authBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));
            var accessToken = soundCloud.FirstOrDefault() ?? new Model.serviceProviderToken.ServiceProviderToken();
            if(accessToken?.accessToken == null)
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://secure.soundcloud.com/oauth/token");
                tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
                tokenRequest.Content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type","client_credentials")
                });
                var response = await _httpClient.SendAsync(tokenRequest);
                accessToken = await _serviceProviderToken.AddSoundCloudAccessToken(response, platform);
            }
            if (DateTime.UtcNow > accessToken.expiresIn)
            {
                var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://secure.soundcloud.com/oauth/token");
                refreshTokenRequest.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type","refresh_token"),
                    new KeyValuePair<string, string>("client_id",clientId),
                    new KeyValuePair<string, string>("client_secret",secretId),
                    new KeyValuePair<string, string>("refresh_token",accessToken.refreshToken!)
                });

                var response = await _httpClient.SendAsync(refreshTokenRequest);
                accessToken = await _serviceProviderToken.RefreshSoundCloudAccessToken(response, accessToken);
            }

            var apiRequest = new HttpRequestMessage(HttpMethod.Get, request);
            apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.accessToken);
            var result = await _httpClient.SendAsync(apiRequest);

            // If 302 redirect, follow manually
            if (result.StatusCode == System.Net.HttpStatusCode.Redirect || result.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
            {
                var redirectUrl = result.Headers.Location.ToString();
                var redirectRequest = new HttpRequestMessage(HttpMethod.Get, redirectUrl);
                redirectRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.accessToken);

                result = await _httpClient.SendAsync(redirectRequest);
            }

            var json = await result.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var thumbnail = doc.RootElement.GetProperty("artwork_url").GetString();
            return thumbnail;
        }

        public async Task<string> GetDeezerThumbnail(string request)
        {
            var result = await _httpClient.GetAsync(request);
            var json = await result.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var thumbnail = doc.RootElement.GetProperty("picture_xl").GetString();
            return thumbnail;
        }

        public async Task<string> GetTidalThumbnail(string request, string clientId, string secretId,string platform)
        {
            var tidal = await _serviceProviderToken.GetAccessToken(platform);
            string authString = clientId + ":" + secretId;
            string authBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));
            var accessToken = tidal.FirstOrDefault() ?? new Model.serviceProviderToken.ServiceProviderToken();

            if (accessToken.accessToken == null)
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://auth.tidal.com/v1/oauth2/token");
                tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
                tokenRequest.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("grant_type","client_credentials")
                });
                var response = await _httpClient.SendAsync(tokenRequest);
                accessToken = await _serviceProviderToken.AddTidalAccessToken(response, platform);
            }
            if(DateTime.UtcNow > accessToken.expiresIn)
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://auth.tidal.com/v1/oauth2/token");
                tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
                tokenRequest.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("grant_type","client_credentials")
                });
                var response = await _httpClient.SendAsync(tokenRequest);
                accessToken = await _serviceProviderToken.RefreshTidalAccessToken(response, accessToken);
            }

            var apiRequest = new HttpRequestMessage(HttpMethod.Get, request);
            apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.accessToken);
            var result = await _httpClient.SendAsync(apiRequest);

            var json = await result.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var thumbnail = doc.RootElement
              .GetProperty("included")[0]
              .GetProperty("attributes")
              .GetProperty("files")[0]
              .GetProperty("href")
              .GetString();

            return thumbnail;
        }

        public async Task<string> GetSpotifyThumbnail(string request, string bearer, string platform, string[] defaultThumbnail, int randomNumber)
        {

            var apiReuqest = new HttpRequestMessage(HttpMethod.Get, request);
            apiReuqest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var result = await _httpClient.SendAsync(apiReuqest);
            var json = await result.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            bool exist = doc.RootElement.TryGetProperty("images", out JsonElement images);
            if (exist)
            {
                return doc.RootElement.GetProperty("images")[0].GetProperty("url").GetString();
            }
            return defaultThumbnail[randomNumber];
        }

        // For future updates, some of this is not available yet and some is need to be bought/enrolled
        public async Task<string> GetAppleMusicThumbnail()
        {
            return string.Empty;
        }

        public async Task<string> GetAmazonMusicThumbnail()
        {
            return string.Empty;
        }
    }
}
