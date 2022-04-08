using Microsoft.Toolkit.Mvvm.ComponentModel;
using Xamarin.Forms;

namespace Wordle.Model
{
    public partial class Letter : ObservableObject
    {
        [ObservableProperty]
        private char value;

        [ObservableProperty]
        private Color color;
        public Letter()
        {

            this.color = Color.WhiteSmoke;
        }public Letter(char ch)
        {
            value = ch;
            this.color = Color.WhiteSmoke;
        }
    }
}
