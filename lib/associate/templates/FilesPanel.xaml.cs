using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using intnet22.lib.general;
using Microsoft.Win32;

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class FilesPanel
    {
        public long IdMember { get; set; }

        public FilesPanel()
        {
            //
            InitializeComponent();
        }

        public void FilesLoad()
        {
            //
            var fileData = AppDomain.CurrentDomain.BaseDirectory + "\\file_data\\" + IdMember;
            if (!Directory.Exists(fileData)) return;

            //
            FilesListBox.Items.Clear();

            //
            foreach (var file in Directory.GetFiles(fileData))
            {
                var fileName = Path.GetFileName(file);

                //
                var item = new ItemCombo();
                item.valor = file;
                item.texto = fileName;

                //
                FilesListBox.Items.Add(item);
                // MessageBox.Show(fileName);
            }
        }

        private void FilesListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
            var item = (ItemCombo)((ListBox)sender).SelectedItem;
            if (item.valor == null) return;

            //
            OpenWithDefaultProgram(item.valor);
        }

        private static void OpenWithDefaultProgram(string path)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path);
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void FileUploadButton_OnClick(object sender, RoutedEventArgs e)
        {
            //
            var fileDialog = new OpenFileDialog();
            // fileDialog.DefaultExt = ".txt"; // Required file extension
            // fileDialog.Filter = "Text documents (.txt)|*.txt"; // Optional file extensions
            fileDialog.Multiselect = true;

            var a = fileDialog.ShowDialog();
            if (a.HasValue && a.Value)
            {
                // verifica/cria diretorio upload
                var fileData = AppDomain.CurrentDomain.BaseDirectory + "\\file_data\\" + IdMember;
                if (!Directory.Exists(fileData)) Directory.CreateDirectory(fileData);

                //
                var index = 0;
                foreach (var file in fileDialog.FileNames)
                {
                    var newPath = fileData + "\\" + fileDialog.SafeFileNames[index];
                    if (!File.Exists(newPath)) File.Copy(file, newPath);
                    index++;
                }

                //
                // var sr = new StreamReader(fileDialog.FileName);
                // MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
                // MessageBox.Show(sr.ReadToEnd());
                // sr.Close();
            }

            FilesLoad();
        }




        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
        private void FileDeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            //
            var item = (ItemCombo)(FilesListBox).SelectedItem;
            var result = MessageBox.Show($@"Remover o arquivo {item.texto}?", "Confirmar remoção", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            //
            switch (result)
            {
                case MessageBoxResult.Yes:

                    //
                    if (item.valor == null) return;

                    // remove se arquivo existir
                    if (File.Exists(item.valor)) File.Delete(item.valor);

                    // recarrega
                    FilesLoad();

                    break;
                case MessageBoxResult.No:
                    // User pressed No
                    break;
            }
        }
    }
}