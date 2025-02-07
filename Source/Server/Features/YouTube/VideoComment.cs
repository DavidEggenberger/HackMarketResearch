using System;

namespace Server.Features.YouTube
{
    public class VideoComment
    {
        public Guid Id { get; set; }
        public int? LikeCount { get; set; }
        public string Text { get; set; }
    }
}
