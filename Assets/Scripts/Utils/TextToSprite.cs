using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public static class TextToSprite
{
    public static Dictionary<char, int> SpriteBindings = new Dictionary<char, int>
    {
        { 'x', 10 },
        { '*', 10 },
        { 'k', 11 },
        { 'K', 11 },
        { 'p', 12 },
        { 'P', 12 },
        { '%', 13 },
        { 'd', 14 },
        { 'D', 14 },
        { 'c', 15 },
        { 'C', 15 },
        { 'h', 16 },
        { 'H', 16 },
        { '♥', 16 },
        { ',', 17 },
        { '.', 17 }
    };

    public static void SetSpriteText(this TMP_Text text, string number)
    {
        text.text = ConvertToSprite(number);
    }

    public static string ConvertToSprite(string number)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char c in number)
        {
            if (SpriteBindings.ContainsKey(c))
            {
                builder.Append($"<sprite={SpriteBindings[c]}/>");
            }
            else
            {
                int.TryParse(c.ToString(), out int result);
                if (0 <= result && result <= 9)
                {
                    builder.Append($"<sprite={c}/>");
                }
            }
        }

        return builder.ToString();
    }
}
