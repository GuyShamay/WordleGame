using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Wordle.Model
{
    public class Word : View

    {
        public Letter[] Letters { get; set; }
        public Word()
        {
            Letters = new Letter[5]
            {
                new Letter(), new Letter(), new Letter(), new Letter(), new Letter()
            };
        }

        public bool Validate(char[] answer)
        {
            int validLetters = 0;
            for (int i = 0; i < Letters.Length; i++)
            {
                if (Letters[i].Value == answer[i])
                {
                    Letters[i].Color = WordExtensions.InPlace;
                    validLetters++;
                }
                else if (answer.Contains(Letters[i]))
                {
                    Letters[i].Color = WordExtensions.InWord;
                }
                else
                {
                    Letters[i].Color = WordExtensions.Wrong;
                }
            }
            return validLetters == answer.Length;
        }
    }

    public static class WordExtensions
    {
        public static Color InPlace = Color.ForestGreen;
        public static Color Wrong = Color.LightSlateGray;
        public static Color InWord = Color.Goldenrod;
        public static bool Contains(this char[] word, Letter letter)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter.Value)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Contains(this Letter[] word, Letter letter)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Value == letter.Value)
                {
                    return true;
                }
            }
            return false;
        }
        public static string Print(this char[] word)
        {
            return new string(word);
        }
    }
}
