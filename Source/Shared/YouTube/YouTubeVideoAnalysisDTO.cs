using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.YouTube
{
    public class YouTubeVideoAnalysisDTO
    {
        public string VideoName { get; set; }
        public string Url { get; set; }
        public string AnalyzationResult { get; set; }
        public string Thumbnail { get; set; }
        public YouTubeVideoAnalysisStateDTO State { get; set; }
        public List<VideoCommentDTO> VideoComments { get; set; } = new List<VideoCommentDTO>();
    }
}
