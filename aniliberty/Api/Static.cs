using Flurl;

namespace aniliberty.Api
{
    internal class Static
    {
        private const string STATIC_URL = @"https://anilibria.top";

        public static string ToFullUrl(string staticUrl)
        {
            return new Url(STATIC_URL).AppendPathSegment(staticUrl);
        }
    }
}
