using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer

namespace intnet22.lib.financial
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public partial class GruposContabeis : IGrid
    {
        //
        private readonly MySqlConnection? _conn;

        //
        public GruposContabeis()
        {
            //
            InitializeComponent();

            //
            GridPrincipal.IsReadOnly = true;
            _conn = MySqlModule.Connectt();

            //
            LoadGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ControlModule.WindowEscape(sender, e, this);
        }

        public void LoadGrid()
        {
            //
            var sql = $@" select id_grupoContabil, nome, ehReceita, ehDespesa
                          from grupo_contabil 
                          order by nome";

            //
            MySqlCommand command = new(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var vos = new List<VoGrupoContabil>();
            while (reader.Read())
            {
                //
                var vo = new VoGrupoContabil();

                //
                vo.Id = (int)(uint)reader["id_grupoContabil"];
                vo.Nome = reader["nome"].ToString();
                vo._EhDespesa = MySqlModule.FromStringToBool(reader["ehDespesa"].ToString()) ? "X" : "";
                vo._EhReceita = MySqlModule.FromStringToBool(reader["ehReceita"].ToString()) ? "X" : "";
                vos.Add(vo);
            }

            //
            reader.Close();
            GridPrincipal.ItemsSource = vos;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            //
            GrupoContabilForm window = new(Constants.Insert, 0);
            ControlModule.OpenWindow(this, window);
        }

        private void DataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
            if (GridPrincipal.SelectedCells.Count <= 0) return;
            var id = ((VoGrupoContabil)GridPrincipal.SelectedCells[0].Item).Id;

            //
            GrupoContabilForm window = new(Constants.Update, id);
            ControlModule.OpenWindow(this, window);
        }

    }
}