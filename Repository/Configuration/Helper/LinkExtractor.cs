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
        private readonly IServiceScopeFactory _scopeFactory;

        public LinkExtractor(HttpClient httpClient, IServiceScopeFactory scopeFactory)
           
        {
            _httpClient = httpClient;
            _scopeFactory = scopeFactory;
        }

        public string Domain(string link)
        {
            var pattern = new Dictionary<string, string> {

                {@"^https:\/\/www\.youtube\.com\/playlist","Youtube"},
                {@"^https:\/\/music\.youtube\.com\/playlist","Youtube Music"},
                {@"^https:\/\/open\.spotify\.com\/playlist","Spotify"},
                {@"^https:\/\/soundcloud\.com","Sound Cloud"},
                {@"^https:\/\/music\.apple\.com(?:\/[a-z]{2})?\/playlist","Apple Music"},
                {@"^https:\/\/www\.deezer\.com(?:\/[a-z]{2})?\/playlist","Deezer"},
                {@"tidal\.com\/playlist","Tidal"},
                {@"music\.amazon\.com\/user-playlist","Amazon Music"},

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
                case "Youtube Music":
                case "Youtube":
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    return query["list"]!;
                case "Spotify":
                case "Deezer":
                case "Tidal":
                case "Amazon Music":
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
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769688481/6884605_yysz29.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189824/a_hand_holding_a_cassette_tape_m6ucx2.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189818/a_person_standing_on_a_street_at_night_nx1m8f.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189816/b910o5hgqe791_bacigc.webp",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189815/a_cartoon_of_a_girl_sitting_on_a_bed_with_a_guitar_p6824d.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189814/a_cartoon_of_a_lake_with_rocks_and_flags_e4v37u.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189813/a_cartoon_of_a_girl_holding_a_glass_in_a_kitchen_xwrqqm.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189813/a_cartoon_of_a_space_ship_and_a_man_standing_on_a_rocky_surface_nj521q.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189813/a_cartoon_of_a_cat_01_igugtw.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189812/a_cartoon_of_a_person_with_headphones_q2ft7d.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189811/a_black_cup_with_liquid_in_it_aozz2v.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189811/a_cartoon_of_a_cat_in_a_glass_rqrucq.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189810/a_cartoon_of_a_cat_j9heps.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189811/a_cartoon_of_a_rabbit_and_a_baby_rabbit_ppycyj.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189811/a_cartoon_of_a_cat_02_dzlrxs.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189810/a_cartoon_of_a_car_fxczgb.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189810/a_blue_background_with_cartoon_characters_mn4kaz.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778189810/a_room_with_a_large_window_and_plants_bkbi7r.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1769689018/8-bit-graphics-pixels-scene-with-person-bench-sunset_rdxrjg.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190266/a_face_in_a_blue_light_wouje0.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190263/a_map_of_a_city_zaw8on.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190261/a_drawing_of_a_sun_and_a_ball_akzhkd.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190261/a_blue_and_orange_background_uliape.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190259/a_mountain_with_a_lake_in_the_background_r7orrl.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190258/a_group_of_buildings_with_blue_awnings_nrxjdm.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190257/a_statue_of_a_woman_with_wings_and_a_plant_t9cnby.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190257/a_statue_of_a_woman_with_wings_and_wings_k7o28y.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190256/a_man_sitting_in_a_chair_jefvhy.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190255/a_painting_of_a_building_in_a_dark_landscape_worhem.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190255/a_man_and_woman_eating_food_c8ambx.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190255/a_colorful_swirls_of_paint_u0ronx.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190254/a_group_of_white_lines_on_a_black_background_dahtyd.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190253/a_group_of_potted_plants_m9xb5d.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190253/a_group_of_bubbles_on_a_white_background_sxkob5.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190252/a_drawing_of_an_astronaut_holding_a_rocket_ugvrh9.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190251/a_colorful_background_with_different_shapes_and_patterns_qrr8u9.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190251/a_close_up_of_a_person_with_wings_w14jeu.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190251/a_chair_on_a_platform_with_a_halo_above_it_nn3eot.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190250/a_black_and_white_picture_of_mountains_and_trees_zvbj58.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190249/a_black_and_white_logo_a5bpog.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190250/a_black_liquid_floating_in_the_air_hzkhsf.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190249/a_woman_with_long_hair_wearing_sunglasses_h3jauu.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190248/a_black_and_white_drawing_of_a_person_with_a_blindfold_udf8iv.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190249/a_black_and_white_maze_valgkd.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190249/two_black_and_white_images_of_mountains_akifcj.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190248/a_black_and_white_image_of_a_group_of_people_pdyorj.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190248/a_black_and_white_drawing_of_a_woman_in_a_black_and_white_dress_girr6v.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190247/a_tree_and_a_rock_tggtn5.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190247/a_white_drawing_of_a_person_with_a_mask_on_his_head_noeoc2.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190247/a_white_swirly_circle_on_a_black_background_ierzb6.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190247/a_white_line_drawing_of_a_group_of_trees_exgcsz.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190247/a_white_lines_on_a_black_background_jrdhcq.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190246/a_white_building_with_balconies_dqphqw.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190246/a_painting_of_a_man_with_a_dripping_face_pexwy1.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190244/a_painting_of_a_building_in_the_desert_yvagyc.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190245/a_skeleton_standing_on_a_pile_of_skulls_oa4uu4.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190245/a_tree_in_the_snow_qgpuud.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190245/a_painting_of_a_mountain_xrgl41.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190244/a_pyramids_with_palm_trees_zcxjo1.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190244/a_pink_and_green_object_with_circles_and_dots_r8u39h.png",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190244/a_screenshot_of_a_computer_vlqbdg.jpg",
                "https://res.cloudinary.com/dwwjrds1j/image/upload/v1778190244/a_painting_of_a_man_with_a_face_on_his_head_sklzpz.png"
            };
            int index = rand.Next(0, defaultThumbnail.Length);
            string request = "";
            string id = ExtractPlaylistID(link,platform);
            string thumbanail = "";

            using var scope = _scopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();
            var apikey = configuration?.GetSection("APIKEY").Get<APIKEY>();

            if (platform == "Youtube" || platform == "Youtube Music")
            {
                string key = apikey.YTAPIKEY;
                request = "https://youtube.googleapis.com/youtube/v3/playlists?part=snippet%2CcontentDetails&id=";
                request += id + "&key=" + key;
                thumbanail = await GetYoutubeThumbnail(request);
            }
            if (platform == "Sound Cloud")
            {
                string clientId = apikey.SCCLIENTID;
                string secretId = apikey.SCSECRETID;

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
                string clientId = apikey.TLCLIENTID;
                string secretId = apikey.TLSECRETID;
                request = $"https://openapi.tidal.com/v2/playlists/{id}?include=coverArt";
                thumbanail = await GetTidalThumbnail(request,clientId,secretId,platform);
            }
            if (platform == "Spotify")
            {
                string bearer = apikey.SPTYKEY!;
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
            using var scope = _scopeFactory.CreateScope();
            var serviceProviderToken = scope.ServiceProvider.GetService<IServiceProviderToken>();

            var soundCloud = await serviceProviderToken!.GetAccessToken(platform);
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
                accessToken = await serviceProviderToken.AddSoundCloudAccessToken(response, platform);
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
                accessToken = await serviceProviderToken.RefreshSoundCloudAccessToken(response, accessToken);
            }

            var apiRequest = new HttpRequestMessage(HttpMethod.Get, request);
            apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.accessToken);
            var result = await _httpClient.SendAsync(apiRequest);

            // If 302 redirect, follow manually
            if (result.StatusCode == System.Net.HttpStatusCode.Redirect || result.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
            {
                var redirectUrl = result?.Headers?.Location?.ToString();
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
            using var scope = _scopeFactory.CreateScope();
            var serviceProviderToken = scope.ServiceProvider.GetService<IServiceProviderToken>();

            var tidal = await serviceProviderToken!.GetAccessToken(platform);
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
                accessToken = await serviceProviderToken.AddTidalAccessToken(response, platform);
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
                accessToken = await serviceProviderToken.RefreshTidalAccessToken(response, accessToken);
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
