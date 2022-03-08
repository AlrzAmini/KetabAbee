
using System.Text.RegularExpressions;
using AngleSharp.Text;

namespace KetabAbee.Application.Extensions
{
    public static class BlogExtensions
    {
        public static int GetBlogReadTime(this string value)
        {
            var array = value.SplitSpaces();
            var minutes = array.Length / 200;
            return minutes == 0 ? 1 : minutes;
        }
    }
}
