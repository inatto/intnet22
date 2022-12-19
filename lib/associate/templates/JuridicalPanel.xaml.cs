using System.Web;
using System.Windows;
using intnet22.lib.general;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class JuridicalPanel : IGrid
    {
        //
        // private MySqlConnection? _conn;
        private long? _idMembroOwner;
        private long? _idProcesso;

        public long? IdMembroOwner
        {
            get => _idMembroOwner;
            set
            {
                _idMembroOwner = value;
                _idMembroOwner = value;
                // LoadGrid();
            }
        }

        public long? IdProcesso
        {
            get => _idProcesso;
            set
            {
                _idProcesso = value;
                _idProcesso = value;
                // LoadGrid();
            }
        }


        public JuridicalPanel()
        {
            //
            InitializeComponent();

        }

        public void LoadGrid()
        {
            //
            /*_conn = MySqlModule.Connectt();

            //
            var sql = " select * from vw_parteJud where 1 ";
            if (_idMembroOwner is not null) sql += $" and id_membro = {_idMembroOwner} ";
            if (_idProcesso is not null) sql += $" and id_processoJud = {_idProcesso} ";
            sql += " order by nome desc limit 10 ";

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
            MainDataGrid.ItemsSource = vos;*/
        }


        private void ReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            LinkReport(this._idMembroOwner);
        }

        private static void LinkReport(long? idMembro)
        {
            //
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("d9b796a10b33c99189e5747c03ee02cfc778587e", idMembro.ToString());

            //
            GeneralModule.OpenUrl("https://anpprev.org.br/jud/processReport?" + queryString);
        }

    }
}