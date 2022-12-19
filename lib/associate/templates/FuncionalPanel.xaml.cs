using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class FuncionalPanel
    {

        public FuncionalPanel()
        {
            //
            InitializeComponent();

        }

        private void DataIngressoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataIngresso.Text = DataIngressoPicker.Text;
        }

        private void DataAposentadoriaPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataAposentadoria.Text = DataAposentadoriaPicker.Text;
        }
    }
}