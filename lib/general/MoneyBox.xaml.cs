using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace intnet22.lib.general;


public partial class MoneyBox
{
    public MoneyBox()
    {
        InitializeComponent();
    }

            private void txtValorBruto_PreviewTextInput(object sender, TextCompositionEventArgs e)
            {
                var regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }

            /// <summary>
            /// Máscara para campo moeda
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void txtValorBruto_TextChanged(object sender, TextChangedEventArgs e)
            {
                // nao tem input, faz nada
                if (Text.Length <= 0) return;

                //
                var val =Text.Replace(",", "");
                if (val.Length >= 3)
                {
                    Text = val[..^2] + ',' + val.Substring(val.Length - 2, 2);
                    Text = $"{double.Parse(Text):#,##0.00}";
                }
                else // centavos, divide por 100
                {
                    Text = $"{double.Parse(Text) / 100:#,##0.00}";
                }

                //
                SelectionStart = Text.Length + 1;
            }


            private void txtValorBruto_GotFocus(object sender, RoutedEventArgs e)
            {
                this.SelectAll();
            }

}