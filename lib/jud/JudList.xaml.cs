using System.Collections.Generic;
using System.Windows.Input;
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
    public partial class JudList : IGrid
    {
        //
        private readonly MySqlConnection? _conn;


        public JudList()
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
            const string sql = $" select * from processo_jud " +
                               $" where 1 ";

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
                vo.TempVaraJud = reader["tempVaraJud"].ToString();
                vo.DataAjuizamento =  MySqlModule.MyDateToDateTime(reader["dataAjuizamento"].ToString());
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
            ControlModule.OpenWindow(this, new PartsList());
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // desabilita enter indo para baixo
            if (e.Key != Key.Enter) return;
            e.Handled = true;

            //
            MainDataGrid_MouseDoubleClick();
        }
    }
}