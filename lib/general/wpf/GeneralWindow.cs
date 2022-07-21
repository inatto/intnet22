using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace intnet22.lib.general.wpf;

public class GeneralWindow : Window
{
    //
    public Control? OwnerControl { get; set; }

    protected GeneralWindow()
    {
    }

    protected void Window_KeyDown(object sender, KeyEventArgs e)
    {
        ControlModule.WindowEscape(sender, e, this);
    }

}