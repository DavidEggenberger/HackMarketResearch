using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using YoutubeDLSharp;
using System.Linq;
using System.Collections.Generic;

namespace Server.Features.YouTube
{
    public class VideoAnalyzer
    {
        public async Task<List<VideoComment>> AnalyzeYouTubeVideo(string url)
        {
            var ytdl = new YoutubeDL();
            var videoResult = await ytdl.RunVideoDataFetch(url, fetchComments: true);
            if (videoResult.Success is false)
            {
                throw new Exception();
            }

            var comments = videoResult.Data.Comments.Select(c => new VideoComment { Text = c.Text, LikeCount = c.LikeCount }).ToList();
            return comments;

        }
    }
}
