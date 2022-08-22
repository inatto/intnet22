using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using intnet22.lib.general;
using intnet22.lib.member;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace intnet22.lib.associate
{
    /// <summary>
    /// Interaction logic for AssociatesWindow.xaml
    /// </summary>
    public partial class AssociatesWindow : IGrid
    {

        //
        private readonly MySqlConnection? _conn;


        public AssociatesWindow() // teste
        {
            //
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();
            ControlModule.ComboLoad(_conn, ComboUfRes, "select tag_uf, nome from uf order by nome;", "tag_uf", "tag_uf");

            //
            TextLocalizar.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //
            LoadGrid();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) button_Click(sender, e);
        }

        public void LoadGrid()
        {
            //
            var uf = "";
            if (ComboUfRes.SelectedValue is not null)
                uf = ComboUfRes.SelectedValue.ToString();

            //
            var search = TextLocalizar.Text;
            var sql = $" select * from vw_associado " +
                      $" where 1 " +
                      $" and (" +
                      $"    nome like '%{search}%' or matriculaOrgao like '%{search}%' or tag_statusCarreira like '%{search}%'  " +
                      $" ) " +
                      $" and (ufResidencial = '{uf}' or '{uf}' = \"\")  " +
                      $" order by nome "; // +


            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var count = 0;
            var vos = new List<VoMember>();
            while (reader.Read())
            {
                //
                count++;
                var vo = new VoMember();
                vo.Active = MySqlModule.FromStringToBool(reader["active"].ToString());
                vo.ActiveAlt = MySqlModule.FromBoolToHuman(vo.Active);
                vo.IdMembro = (int)(uint)reader["id_membro"];

                //
                vo.VoMembroPai = new VoMember();
                vo.VoMembroPai.VoPerson = new VoPerson();
                vo.VoMembroPai.VoPerson.Nome = reader["nomePai"].ToString();

                //
                vo.VoPerson = new VoPerson();
                vo.VoPerson.VoEnderecoResidencial = new VoAddress();
                vo.VoPerson.Nome = reader["nome"].ToString();
                vo.VoPerson.Cpf = reader["cpf"].ToString();
                vo.VoPerson.EmailPrincipal = reader["emailPrincipal"].ToString();
                vo.VoPerson.VoEnderecoResidencial.TagUf = reader["ufResidencial"].ToString();

                //
                vos.Add(vo);
            }

            //
            reader.Close();
            MainDataGrid.ItemsSource = vos;
            Title = $"Associados - {count}";
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            //
            ControlModule.OpenWindow(this, new AssociateWindow(Constants.Insert, 0));
        }

        private void MainDataGrid_MouseDoubleClick(object? sender = null, MouseButtonEventArgs? e = null)
        {
            //
            ControlModule.OpenWindow(this, new AssociateWindow(Constants.Update, MainDataGrid.FirstId()));
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