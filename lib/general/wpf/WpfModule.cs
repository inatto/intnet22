// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable LoopCanBeConvertedToQuery

using System;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
// ReSharper disable UseStringInterpolation

namespace intnet22.lib.general.wpf
{
    public static class WpfModule
    {
        public static void ControlFill(Control control, object? param)
        {
            switch (control)
            {
                //
                case MaskedTextBox { Mask: "9999999999" } c:
                    c.Text = param is null ? "" : param.ToString();
                    break;
                case MaskedTextBox { Mask: "00/00/0000" } c:
                    c.Text = param is null ? "" : ControlModule.ToMaskDate((DateTime)param);
                    break;
                // case MaskedTextBox { Mask: "00/0000" } c:
                    // c.Text = param is null ? "" : ControlModule.ToMaskDateRef((DateTime)param);
                    // break;
                case MaskedTextBox c:
                    c.Text = param is null ? "" : param.ToString();
                    break;
                case CheckBox c:
                    c.IsChecked = param is not null && (bool)param;
                    break;
                case TextBox c:

                    //
                    if (control is MoneyBox moneyBox)
                        moneyBox.Text = param is null ? "" : string.Format("{0:.00}", param);
                    else
                        c.Text = param is null ? "" : param.ToString();

                    //
                    break;
                case ComboBox c:
                    c.SelectedValue = param;
                    break;
            }
        }
    }
}