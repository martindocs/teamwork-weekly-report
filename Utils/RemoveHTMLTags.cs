using System.Text.RegularExpressions;

namespace TeamworkWeeklyReport.Utils
{
    public static class RemoveTags
    {
        public static string TagRemover(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                return Regex.Replace(tag, "<.*?>", string.Empty);
                // < — match the opening angle bracket.
                // .*? — match any characters, . means any character, * means zero or more times,
                // makes it non-greedy, so it stops as soon as it finds the next part of the pattern > tag
            }

            return null;
        }
    }
}