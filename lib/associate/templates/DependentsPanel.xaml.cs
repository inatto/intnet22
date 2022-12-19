using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using intnet22.lib.general;
using intnet22.lib.member;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class DependentsPanel : IGrid
    {
        //
        private MySqlConnection? _conn;
        private long _idPessoaOwner;
        private long _idMembroPai;

        public long IdPessoaOwner
        {
            set
            {
                _idPessoaOwner = value;
                LoadGrid();
            }
        }

        public long IdMembroPai
        {
            set
            {
                _idMembroPai = value;
                LoadGrid();
            }
        }

        public DependentsPanel()
        {
            //
            InitializeComponent();
        }

        public void LoadGrid()
        {
            //
            _conn = MySqlModule.Connectt();

            //
            var sql = $@"   select * from vw_membro 
                            where 1 
                            and (id_pessoa_owner = {_idPessoaOwner} or id_membro_pai = {_idMembroPai}) 
                            order by nome desc 
                        ";

            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var vos = new List<VoMember>();
            while (reader.Read())
            {
                //
                var vo = new VoMember();
                vo.IdMembro = (int)(uint)reader["id_membro"];
                vo.IdMembroPai =  reader["id_membro_pai"] != null ? null : (int)(uint)reader["id_membro_pai"];

                //
                vo.Parentesco = reader["parentesco"].ToString();
                vo.VoPerson = new VoPerson();
                vo.VoPerson.Nome = reader["nome"].ToString();
                vo.VoPerson.Cpf = reader["cpf"].ToString();
                vo.VoPerson.EmailPrincipal = reader["emailPrincipal"].ToString();

                //
                if (vo.IdMembroPai != null) vo._StrPensionista = "Sim";

                //
                vos.Add(vo);


            }

            //
            reader.Close();
            MainDataGrid.ItemsSource = vos;
        }

        private void InsertButton_OnClick(object sender, RoutedEventArgs e)
        {
            ControlModule.OpenModalWindow(this, new DependentForm(Constants.Insert, _idPessoaOwner));
        }

        private void MainDataGrid_OnMouseDoubleClick(object? sender = null, MouseButtonEventArgs? e = null)
        {
            ControlModule.OpenModalWindow(this, new DependentForm(Constants.Update, _idPessoaOwner, MainDataGrid.FirstId()));
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