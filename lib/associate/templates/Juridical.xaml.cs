using System.Collections.Generic;
using System.Windows.Input;
using intnet22.lib.general;
using intnet22.lib.jud;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class Juridical : IGrid
    {
        //
        private MySqlConnection? _conn;
        private long _idMembroOwner;

        public long IdMembroOwner
        {
            get => _idMembroOwner;
            set
            {
                _idMembroOwner = value;
                LoadGrid();
            }
        }

        public Juridical()
        {
            //
            InitializeComponent();

            //
            GeneralModule.BucketIconFc(TabIcon, "users_3");
        }

        public void LoadGrid()
        {
            //
            _conn = MySqlModule.Connectt();

            //
            string sql = $" select * from vw_parteJud " +
                         $" where 1=1 " +
                         $" and id_membro = {_idMembroOwner}" +
                         $" order by nome desc " +
                         $" limit 10 ";

            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var vos = new List<VoProcessoJud>();
            while (reader.Read())
            {
                //
                var vo = new VoProcessoJud();
                vo.NrProcessoExecucao = reader["nrProcessoExecucao"].ToString();
                vo.TempVaraJud = reader["tempVaraJud"].ToString();
                vo.DataAjuizamento = MySqlModule.ToDateTime(reader["dataAjuizamento"].ToString());

                //
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