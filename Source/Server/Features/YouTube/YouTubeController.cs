using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace Server.Features.YouTube
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouTubeController : ControllerBase
    {
        private VideoAnalyzer videoAnalyzer;
        
        public YouTubeController(VideoAnalyzer videoAnalyzer)
        {
            this.videoAnalyzer = videoAnalyzer;
        }

        [HttpGet("comments/{url}")]
        public async Task<ActionResult<List<VideoComment>>> GetComments(string url)
        {
            var urlDecoded = HttpUtility.UrlDecode(url);
            var comments = await videoAnalyzer.AnalyzeYouTubeVideo(urlDecoded);
            return Ok(comments);
        }
    }
}
