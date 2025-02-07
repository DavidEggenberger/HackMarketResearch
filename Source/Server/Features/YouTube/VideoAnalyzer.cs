using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using YoutubeDLSharp;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Server.Features.YouTube
{
    public class VideoAnalyzer
    {
        private readonly ILogger _logger;

        public VideoAnalyzer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VideoAnalyzer>();
        }

        public async Task<List<VideoComment>> AnalyzeYouTubeVideo(string url)
        {
            var ytdl = new YoutubeDL();
            var videoResult = await ytdl.RunVideoDataFetch(url, fetchComments: true);
            if (videoResult.Success is false)
            {
                _logger.LogCritical(string.Join(", ", videoResult.ErrorOutput));
                return null;
            }

            var comments = videoResult.Data.Comments.Select(c => new VideoComment { Text = c.Text, LikeCount = c.LikeCount }).ToList();
            return comments;

        }
    }
}
