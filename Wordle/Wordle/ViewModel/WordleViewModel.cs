using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Wordle.Model;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Wordle.ViewModel
{
    public partial class WordleViewModel : ObservableObject
    {
        [ObservableProperty]
        private Word[] rows;

        private int rowIndex;
        private int colIndex;
        private char[] answer;
        public Letter[] Keyboard1 { get; }
        public Letter[] Keyboard2 { get; }
        public Letter[] Keyboard3 { get; }

        public WordleViewModel()
        {
            //Keyboard1 = "QWERTYUIOP<".ToCharArray();
            //Keyboard2 = "ASDFGHJKL".ToCharArray();
            //Keyboard3 = "ZXCVBNM".ToCharArray();
            rows = new Word[6] { new Word(), new Word(), new Word(), new Word(), new Word(), new Word() };
            Keyboard1 = new Letter[10] { new Letter('Q'), new Letter('W'), new Letter('E'), new Letter('R'), new Letter('T'), new Letter('Y'), new Letter('U'), new Letter('I'), new Letter('O'), new Letter('P') };
            Keyboard2 = new Letter[9] { new Letter('A'), new Letter('S'), new Letter('D'), new Letter('F'), new Letter('G'), new Letter('H'), new Letter('J'), new Letter('K'), new Letter('L') };
            Keyboard3 = new Letter[8] { new Letter('Z'), new Letter('X'), new Letter('C'), new Letter('V'), new Letter('B'), new Letter('N'), new Letter('M'), new Letter('<') };
            colorKeyboard();
            generateAnswer();
            if (answer == null)
            {
                answer = "SNACK".ToCharArray();
            }
        }

        private void colorKeyboard()
        {
            foreach (Letter letter in Keyboard1)
            {
                letter.Color = Color.LightGray;
            }
            foreach (Letter letter in Keyboard2)
            {
                letter.Color = Color.LightGray;
            }
            foreach (Letter letter in Keyboard3)
            {
                letter.Color = Color.LightGray;
            }
        }

        [ICommand]
        public void Enter(object obj)
        {
            if (colIndex < 5) { return; }

            bool isCorrect = Rows[rowIndex].Validate(answer);
            validateKeyboardColors(Rows[rowIndex].Letters);
            if (isCorrect)
            {
                App.Current.MainPage.DisplayAlert("Awesome!", "You win!", "OK");
                resetGame();
                return;
            }

            if (rowIndex == 5)
            {
                App.Current.MainPage.DisplayAlert("Oh No!", $"Game Over! The Answer is '{answer.Print()}'.", "Reset Game");
                resetGame();
                return;
            }
            rowIndex++;
            colIndex = 0;
        }

        private void resetGame()
        {
            colorKeyboard();
            foreach (Word word in Rows)
            {
                foreach (Letter letter in word.Letters)
                {
                    letter.Value = ' ';
                    letter.Color = Color.White;
                }
            }
            rowIndex = 0;
            colIndex = 0;
            generateAnswer();
        }

        private async void generateAnswer()
        {
            string[] words;
            HttpClient client = new HttpClient();
            while (true)
            {
                HttpResponseMessage response = await client.GetAsync("https://random-word-api.herokuapp.com/word?number=50");
                Debug.WriteLine("GET");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    words = content.Split(new char[] { ',', '\"', '[', ']' });

                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 5)
                        {
                            answer = words[i].ToUpper().ToCharArray();
                            return;
                        }
                    }

                }
            }
        }

        [ICommand]
        public void AddLetter(Letter letter)
        {
            if (letter.Value == '<') // delete letter 
            {
                if (colIndex == 0)
                    return;
                colIndex--;
                Rows[rowIndex].Letters[colIndex].Value = ' ';

                return;
            }

            if (colIndex == 5)
                return;

            Rows[rowIndex].Letters[colIndex].Value = letter.Value;
            colIndex++;
        }

        private void validateKeyboardColors(Letter[] word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Color == WordExtensions.Wrong)
                {
                    foreach (Letter letter in Keyboard1)
                    {
                        if (letter.Value == word[i].Value)
                        {
                            letter.Color = WordExtensions.Wrong;
                        }
                    }
                    foreach (Letter letter in Keyboard2)
                    {
                        if (letter.Value == word[i].Value)
                        {
                            letter.Color = WordExtensions.Wrong;
                        }
                    }
                    foreach (Letter letter in Keyboard3)
                    {
                        if (letter.Value == word[i].Value)
                        {
                            letter.Color = WordExtensions.Wrong;
                        }
                    }
                }
            }
        }
    }
}
