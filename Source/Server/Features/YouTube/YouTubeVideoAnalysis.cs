using Shared.YouTube;
using System;
using System.Collections.Generic;

namespace Server.Features.YouTube
{
    public class YouTubeVideoAnalysis
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public List<VideoComment> Comments { get; set; }

        public YouTubeVideoAnalysisDTO ToDTO()
        {
            return new YouTubeVideoAnalysisDTO
            {
                Url = Url
            };
        }
    }
}
