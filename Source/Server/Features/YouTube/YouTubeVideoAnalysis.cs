using Shared.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Features.YouTube
{
    public class YouTubeVideoAnalysis
    {
        public Guid Id { get; set; }
        public string VideoName { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public List<VideoComment> Comments { get; set; }

        public YouTubeVideoAnalysisDTO ToDTO()
        {
            return new YouTubeVideoAnalysisDTO
            {
                VideoName = VideoName,
                Url = Url,
                Thumbnail = Thumbnail,
                VideoComments = Comments.Select(c => new VideoCommentDTO { LikeCount = c.LikeCount, Text = c.Text }).ToList()
            };
        }
    }
}
