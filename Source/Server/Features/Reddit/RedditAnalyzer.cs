using Reddit;

namespace Server.Features.Reddit
{
    public class RedditAnalyzer
    {
        private RedditClient redditClient;

        public RedditAnalyzer()
        {
            redditClient = new RedditClient();
        }
    }
}
