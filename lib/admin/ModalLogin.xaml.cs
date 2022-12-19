using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using intnet22.lib.main;

namespace intnet22.lib.admin
{
    public partial class ModalLogin
    {
        public ModalLogin()
        {
            //
            InitializeComponent();

            //
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"https://inat.to/farm/backs/anp.jpg", UriKind.Absolute);
            bitmap.EndInit();

            //
            var brush = new ImageBrush(bitmap);
            brush.Stretch = Stretch.Fill;
            Background = brush;

            //
            UserText.Focus();
        }

        private void ModalLogin_OnClosed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ModalLogin_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Application.Current.Shutdown();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async void Login()
        {
            //
            try
            {
                // verifica se preencheu os campos
                if (UserText.Text.Length == 0 || PassText.Password.Length == 0)
                    throw new ValidationException("Informe o nome do usuário e senha.");

                //
                var httpclient = new HttpClient();
                var url = "https://anpprev.org.br/anp/login/appLogin";
                var parameters = new Dictionary<string, string> { { "username", UserText.Text }, { "criptPass", PassText.Password }, { "empresaId", "2" } };
                var encodedContent = new FormUrlEncodedContent(parameters);

                //
                var response = await httpclient.PostAsync(url, encodedContent).ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception(response.StatusCode.ToString());

                //
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jdoc = JsonDocument.Parse(responseContent);

                //
                var loginOk = jdoc.RootElement.GetProperty("lgnOk").GetBoolean();
                if (!loginOk) throw new Exception("Login inválido.");
                LoginSessionCreate(jdoc);

                Dispatcher.Invoke(() =>
                {
                    var main = new MainWindow();
                    Hide();
                    main.Show();
                });
            }
            catch (JsonException e)
            {
                MessageBox.Show("Formato inválido." + e.Message, "Login", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (ValidationException e)
            {
                MessageBox.Show(e.Message, "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                if (UserText.Text.Length == 0) UserText.Focus();
                else if (PassText.Password.Length == 0) PassText.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void LoginSessionCreate(JsonDocument doc)
        {
            // limpa e adiciona dicionario global
            Application.Current.Properties.Clear();
            Application.Current.Properties.Add("jsonDoc", doc);
            // var username = doc.RootElement.GetProperty("username").ToString();
        }

        // ReSharper disable once UnusedMember.Local
        private static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes); // .NET 5 +
        }
    }
}