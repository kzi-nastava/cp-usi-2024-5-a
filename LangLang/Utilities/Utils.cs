
namespace LangLang.Utilities
{
    public class Utils
    {

        public static string ReplacePlaceholders(string template, string[] replacements)
        {
            for (int i = 0; i < replacements.Length; i++)
                template = template.Replace($"{{{i}}}", replacements[i]);
            return template;
        }
    }
}
