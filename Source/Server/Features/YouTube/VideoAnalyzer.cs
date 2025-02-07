using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Server.Features.YouTube.APIObjects;

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
    }
}
