using System;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using intnet22.lib.associate;
using intnet22.lib.financial;
using intnet22.lib.jud;

// ReSharper disable All

namespace intnet22.lib.main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly RoutedCommand AssociatesOpen = new RoutedCommand();

        public MainWindow()
        {
            //
            InitializeComponent();

            //
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = WindowState.Maximized;

            //
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"https://inat.to/farm/backs/anp.jpg", UriKind.Absolute);
            bitmap.EndInit();

            //
            var brush = new ImageBrush(bitmap);
            brush.Stretch = Stretch.Fill;
            this.Background = brush;

            //
            MenuEnable();
        }

        private void MenuEnable()
        {
            // localiza variavel global json (criada no login)
            var prop = Application.Current.Properties["jsonDoc"];
            if (prop == null) throw new Exception("Invalid general doc property.");

            //
            var doc = (JsonDocument)prop;
            var username = doc.RootElement.GetProperty("username").ToString();

            //
            if (username == "desenv@inatto.com" || username == "support@inatto.com")
            {
                AssociadosMenu.IsEnabled = true;
                JuridicoMenu.IsEnabled = true;
                FinanceiroMenu.IsEnabled = true;
            }
            else if (username == "beth@anpprev.org.br")
            {
                AssociadosMenu.IsEnabled = true;
                JuridicoMenu.IsEnabled = true;
                FinanceiroMenu.IsEnabled = true;
            }
            else if (username == "talitabastos.anpprev@gmail.com" || username == "andre@anpprev.org.br" || username == "samela@anpprev.org.br")
            {
                JuridicoMenu.IsEnabled = true;
            }
            else if (username == "darleide@anpprev.org.br")
            {
                FinanceiroMenu.IsEnabled = true;
            }
        }

        private void MenuAssociados_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            //
            AssociateList window = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal,
                //
                Owner = this
            };

            window.Show();
        }

        private void MenuJuridico_Click(object? sender, RoutedEventArgs? e)
        {
            //
            PartsList window = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal,
                //
                Owner = this
            };

            window.Show();
        }

        private void MenuProcessos_Click(object? sender, RoutedEventArgs? e)
        {
            //
            ProcessesList window = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal,
                //
                Owner = this
            };

            window.Show();
        }

        private void MenuDespesas_Click(object sender, RoutedEventArgs e)
        {
            //
            ExpenseList window = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal,
                //
                Owner = this
            };

            window.Show();
        }

        private void MenuReceitas_Click(object sender, RoutedEventArgs e)
        {
            //
            RevenueList window = new()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal,
            };

            window.Show();
        }

        private void MenuPendencias_Click(object sender, RoutedEventArgs e)
        {
            //
            // Pendencias window = new()
            // {
            // Owner = this,
            // WindowStartupLocation = WindowStartupLocation.CenterScreen,
            // WindowState = WindowState.Normal,
            // };

            // window.Show();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuAssociados_Click();
        }

        private void MenuGrupos_Click(object sender, RoutedEventArgs e)
        {
            //
            CategoryList window = new()
            {
                Owner = this,
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Show();
        }

        private void MenuSair_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}