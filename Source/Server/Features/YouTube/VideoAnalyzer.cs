using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Server.Features.YouTube.APIObjects;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Server.Features.YouTube
{
    public class VideoAnalyzer
    {
        private string GoogleAPIKey;
        private HttpClient httpClient;

        public VideoAnalyzer(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            GoogleAPIKey = configuration["GoogleKey"];
            httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<VideoComment>> AnalyzeYouTubeVideo(string url)
        {
            var videoComments = new List<VideoComment>();
            var nextPageToken = string.Empty;

            do
            {
                string apiUrl = $"https://www.googleapis.com/youtube/v3/commentThreads?part=snippet&videoId={url}&key={GoogleAPIKey}&maxResults=100&pageToken={nextPageToken}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    break;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var root = JsonSerializer.Deserialize<Root>(jsonResponse);

                if (root?.items != null)
                {
                    videoComments.AddRange(root.items
                        .Select(i => i.snippet.topLevelComment.snippet)
                        .Select(t => new VideoComment() { LikeCount = t.likeCount, Text = t.textDisplay })
                        .ToList());
                }

                nextPageToken = root?.nextPageToken ?? null;

            } while (!string.IsNullOrEmpty(nextPageToken));

            return videoComments.OrderByDescending(c => c.LikeCount).ToList();
        }

        public async Task<(string name, string thumbnail)> GetYouTubeVideoTitle(string videoId)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = GoogleAPIKey,
                ApplicationName = "YouTubeAPI-Example"
            });

            var request = youtubeService.Videos.List("snippet");
            request.Id = videoId;

            var response = await request.ExecuteAsync();
            if (response.Items.Count > 0)
            {
                return (response.Items[0].Snippet.Title, response.Items[0].Snippet.Thumbnails.High.Url);
            }
            return ("Video not found", "");
        }
    }
}
