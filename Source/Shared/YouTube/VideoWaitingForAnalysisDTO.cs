using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.YouTube
{
    public class VideoWaitingForAnalysisDTO
    {
        public Guid Id { get; set; }
        public string YoutubeId { get; set; }
        public string ProductDescription { get; set; }
        public List<VideoCommentDTO> VideoComments { get; set; }
    }
}
