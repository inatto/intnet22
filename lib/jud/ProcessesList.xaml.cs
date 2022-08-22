using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using intnet22.lib.financial;
using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace intnet22.lib.jud
{
    /// <summary>
    /// Interaction logic for AssociatesWindow.xaml
    /// </summary>
    public partial class ProcessesList : IGrid
    {
        //
        private readonly MySqlConnection? _conn;


        public ProcessesList()
        {
            //
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();

            //
            LoadGrid();
        }

        public void LoadGrid()
        {
            //
            var search = TextLocalizar.Text;
            var sql = @" select * from processo_jud where 1 ";
            sql += $" and nrProcessoExecucao like '%{search}%' or nrProcessoOriginario like '%{search}%' or tempVaraJud like '%{search}%' or tempAutorJud like '%{search}%'";

            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var count = 0;
            var vos = new List<VoProcessoJud>();
            while (reader.Read())
            {
                //
                count++;
                var vo = new VoProcessoJud();
                vo.IdProcessoJud = (int)(uint)reader["id_processoJud"];
                vo.NrProcessoExecucao = reader["nrProcessoExecucao"].ToString();
                vo.NrProcessoOriginario = reader["nrProcessoOriginario"].ToString();
                vo.DataAjuizamento =  MySqlModule.ToDateTime(reader["dataAjuizamento"].ToString());
                vo.DataTransitoJulgado =  MySqlModule.ToDateTime(reader["dataTransitoJulgado"].ToString());
                vo.TempVaraJud = reader["tempVaraJud"].ToString();
                vo.TempEscritorioAdv = reader["tempEscritorioAdv"].ToString();
                vo.ObjetoJud = reader["ObjetoJud"].ToString();

                //
                vo.MetaTitle = reader["metaTitle"].ToString();

                //
                vos.Add(vo);
            }

            //
            reader.Close();
            MainDataGrid.ItemsSource = vos;
            Title = $"Processos - {count}";
        }

        private void MainDataGrid_MouseDoubleClick(object? sender = null, MouseButtonEventArgs? e = null)
        {
            //
            if (MainDataGrid.SelectedCells.Count <= 0) return;

            //
            var id = ((VoProcessoJud)MainDataGrid.SelectedCells[0].Item).IdProcessoJud;
            ControlModule.OpenWindow(this, new ProcesseForm(Constants.Update, id));
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // desabilita enter indo para baixo
            if (e.Key != Key.Enter) return;
            e.Handled = true;

            //
            MainDataGrid_MouseDoubleClick();
        }

        private void ExtratoButton_Click(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }

        private void TextProcesso_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoadGrid();
        }
    }
}