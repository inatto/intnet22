using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using intnet22.lib.general.wpf;
using MySql.Data.MySqlClient;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable LoopCanBeConvertedToQuery

namespace intnet22.lib.general
{
    public static class ControlModule
    {
        public static void NumberOnly(System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void WindowEscape(object sender, System.Windows.Input.KeyEventArgs e, Window window)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                window.Close();
                //Application.Current.MainWindow.Focus();
            }
        }

        public static void OpenWindow(Window? owner, Window window)
        {
            //
            if (owner is null) Application.Current.Shutdown(17261);

            //
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Normal;
            window.Owner = owner;
            window.Show();
        }

        public static void OpenModalWindow(FrameworkElement parentControl, GeneralWindow window)
        {
            //
            window.OwnerControl = parentControl;
            window.Owner = Window.GetWindow(parentControl);

            //
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Normal;
            window.ShowDialog();
        }


        public static void ComboLoad(MySqlConnection? conn, ComboBox combo, string sql, string value, string text)
        {
            //
            if (conn is null) throw new Exception("Null connection.");

            //
            var command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            //
            combo.SelectedValuePath = "valor";
            combo.Items.Add(new ItemCombo() { valor = "", texto = "" });
            while (reader.Read())
                combo.Items.Add(new ItemCombo() { valor = reader[value].ToString(), texto = reader[text].ToString() });

            //
            reader.Close();
        }

        public static void ComboLoadArray(IEnumerable<ComboBox> combos, MySqlConnection? conn, string sql, string value, string text)
        {
            //
            if (conn is null) throw new Exception("Null connection.");

            //
            var command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            //
            foreach (var combo in combos)
            {
                combo.SelectedValuePath = "valor";
                combo.Items.Add(new ItemCombo() { valor = "", texto = "" });
            }

            //
            while (reader.Read())
                foreach (var combo in combos)
                    combo.Items.Add(new ItemCombo() { valor = reader[value].ToString(), texto = reader[text].ToString() });

            //
            reader.Close();
        }

        public static string ToMaskDate(DateTime? dt)
        {
            return dt != null ? ((DateTime)dt).ToShortDateString() : "";
        }

        public static string ToMaskDateRef(DateTime? dt)
        {
            return dt != null ? ((DateTime)dt).ToString("MM/yyyy") : "";
        }


    }
}