using System.Collections.Generic;
using System.Windows.Input;
using intnet22.lib.financial;
using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.jud.templates
{
    /// <summary>
    /// </summary>
    public partial class PartsGrid : IGrid
    {
        //
        private MySqlConnection? _conn;
        private long? _idMembroOwner;
        private long? _idProcesso;

        public long? IdMembroOwner
        {
            get => _idMembroOwner;
            set
            {
                _idMembroOwner = value;
                LoadGrid();
            }
        }

        public long? IdProcesso
        {
            get => _idProcesso;
            set
            {
                _idProcesso = value;
                LoadGrid();
            }
        }


        public PartsGrid()
        {
            //
            InitializeComponent();

            //
            // GeneralModule.BucketIconFc(TabIcon, "users_3");
        }

        public void LoadGrid()
        {
            //
            _conn = MySqlModule.Connectt();

            //
            var sql = " select * from vw_parteJud where 1 ";
            if (_idMembroOwner is not null) sql += $" and id_membro = {_idMembroOwner} ";
            if (_idProcesso is not null) sql += $" and id_processoJud = {_idProcesso} ";
            // sql += " order by nome ";
            // sql += " limit 200 ";

            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var vos = new List<VoPartJud>();
            while (reader.Read())
            {
                //
                var vo = JudModel.VoParteFromReader(reader);
                vos.Add(vo);
            }

            //
            reader.Close();
            MainDataGrid.ItemsSource = vos;
        }

        private void MainDataGrid_OnMouseDoubleClick(object? sender = null, MouseButtonEventArgs? e = null)
        {
            //
            //ControlModule.OpenModalWindow(this, new DependentWindow(Constants.Update, _idMembroOwner, MainDataGrid.FirstId()));
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // desabilita enter indo para baixo
            if (e.Key != Key.Enter) return;
            e.Handled = true;

            //
            MainDataGrid_OnMouseDoubleClick();
        }
    }
}