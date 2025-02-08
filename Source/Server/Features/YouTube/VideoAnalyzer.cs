using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Server.Features.YouTube.APIObjects;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;

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
            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={videoId}&key={GoogleAPIKey}";
            var response = await httpClient.GetStringAsync(url);

            using JsonDocument json = JsonDocument.Parse(response);
            var items = json.RootElement.GetProperty("items");

            if (items.GetArrayLength() > 0)
            {
                var snippet = items[0].GetProperty("snippet");
                string title = snippet.GetProperty("title").GetString();
                string thumbnail = snippet.GetProperty("thumbnails").GetProperty("high").GetProperty("url").GetString();

                return (title, thumbnail);
            }

            return ("Video not found", "");
        }

        public async Task<List<YouTubeVideoAnalysis>> SearchYouTubeVideos(string query)
        {
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeDataString(query)}&maxResults=5&type=video&key={GoogleAPIKey}";

            var response = await httpClient.GetStringAsync(url);

            var root = JsonSerializer.Deserialize<Root1>(response);

            var videos = new List<YouTubeVideoAnalysis>();
            foreach (var item in root.items.Select(i => (i.id.videoId, i.snippet)))
            {
                var t = new YouTubeVideoAnalysis()
                {
                    Url = $"https://www.youtube.com/watch?v={item.videoId}",
                    VideoName = item.snippet.title
                };

                videos.Add(t);
            }

            return videos;
        }
    }

    public class Default
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class High
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Id
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public Id id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Medium
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class Root1
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item> items { get; set; }
    }

    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string liveBroadcastContent { get; set; }
        public DateTime publishTime { get; set; }
    }

    public class Thumbnails
    {
        public Default @default { get; set; }
        public Medium medium { get; set; }
        public High high { get; set; }
    }
}
