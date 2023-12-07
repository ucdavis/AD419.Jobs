using System;

namespace AD419.Jobs.Core.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string titleCase)
    {
        if (string.IsNullOrWhiteSpace(titleCase))
        {
            return string.Empty;
        }

        var words = titleCase.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
        var pascalCase = string.Join(string.Empty, words.Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
        return pascalCase;
    }
}


