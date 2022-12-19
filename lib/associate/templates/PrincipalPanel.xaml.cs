using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using intnet22.lib.general;

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class PrincipalPanel
    {
        //
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        // private readonly MySqlConnection? _conn;

        public PrincipalPanel()
        {
            //
            InitializeComponent();

            //
            // _conn = MySqlModule.Connectt();
        }

        public void CheckVinculoAtivo_OnClick(object? sender = null, RoutedEventArgs? e = null)
        {
            // LblVinculoAtivo.Foreground = Brushes.;
            LblVinculoAtivo.Foreground = CheckVinculoAtivo.IsChecked == true ? Brushes.Green : Brushes.Red;
        }


        private void ComboStatusAssociativo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
            var combo = (ComboBox)sender;
            var item = (ItemCombo)combo.SelectedItem;

            //
            if (item.texto != null && item.texto.IndexOf("falec", StringComparison.Ordinal) > 0)
            {
                CheckFalecido.IsChecked = true;
                CheckFalecido.IsEnabled = false;
                MaskDataObito.Focus();
                MaskDataObito.IsManipulationEnabled = true;
            }
            else
            {
                CheckFalecido.IsChecked = false;
                CheckFalecido.IsEnabled = true;
                MaskDataObito.IsManipulationEnabled = false;
            }

            //
            if (item.texto != null && item.texto.IndexOf("desfiliado", StringComparison.Ordinal) > 0)
            {
                MaskDataDesfiliacao.Focus();
                MaskDataDesfiliacao.IsManipulationEnabled = true;
            }
            else
            {
                MaskDataDesfiliacao.IsManipulationEnabled = false;
            }
        }

        private void CheckFalecido_OnChecked(object sender, RoutedEventArgs e)
        {
            ComboStatusAssociativo.SelectedValue = 3;
        }

        private void DataObitoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataObito.Text = DataObitoPicker.Text;
        }

        private void DataNascimentoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataNascimento.Text = DataNascimentoPicker.Text;
        }

        private void RgEmissaoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskRgEmissao.Text = RgEmissaoPicker.Text;
        }

        private void DataFiliacaoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataFiliacao.Text = DataFiliacaoPicker.Text;
        }

        private void DataDesfiliacaoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataDesfiliacao.Text = DataDesfiliacaoPicker.Text;
        }


    }
}