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

namespace intnet22.lib.jud
{
    /// <summary>
    /// Interaction logic for AssociatesWindow.xaml
    /// </summary>
    public partial class PartsList : IGrid
    {
        //
        private readonly MySqlConnection? _conn;


        public PartsList()
        {
            //
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();

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
            var search = TextLocalizar.Text;

            //
            var filtro = "";
            if (CheckExecucoes.IsChecked == true) filtro += " and ifnull(numeroExecucao,'') <> '' ";
            if (CheckEmbargos.IsChecked == true) filtro += " and ifnull(numeroEmbargos,'') <> '' ";

            var sql = $" select pj.*, p.nome from parte_jud pj " +
                               $" left join membro m on m.id_membro = pj.id_membro " +
                               $" left join pessoa p on p.id_pessoa = m.id_pessoa " +
                               $" where 1 " + filtro +
                               $" and ( 1=2 " +
                               $"  or p.nome like '%{search}%' or correspondenciaEnviada like '%{search}%' or termoAcordoRecebido like '%{search}%'  " +
                               $"  or calculoHexagonRecebido like '%{search}%' or avisoRecebimento like '%{search}%' or termoAssinadoRecebido like '%{search}%'  " +
                               $"  or emailEnviado like '%{search}%' or percentualAplicado like '%{search}%' or enviadoAoMotaParaContato like '%{search}%'  " +
                               $"  or alegacaoLitispendencia like '%{search}%' or pediramDesistencia like '%{search}%' " +
                               $" ) ";

            //
            var command = new MySqlCommand(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var count = 0;
            var vos = new List<VoPartJud>();
            while (reader.Read())
            {
                //
                count++;
                var vo = new VoPartJud();

                //
                vo.IdPartJud = (int)(uint)reader["id_parteJud"];

                //
                vo.IdMembro = (int)(uint)reader["id_membro"];
                vo.VoMember = new VoMember();
                vo.VoMember.VoPerson = new VoPerson();
                vo.VoMember.VoPerson.Nome = reader["nome"].ToString();

                //
                vo.Ref1993 = reader["nome"].ToString();
                vo.CorrespondenciaEnviada = reader["correspondenciaEnviada"].ToString();
                vo.TermoAcordoRecebido = reader["termoAcordoRecebido"].ToString();
                vo.CalculoHexagonRecebido = reader["calculoHexagonRecebido"].ToString();
                vo.AvisoRecebimento = reader["avisoRecebimento"].ToString();
                vo.TermoAssinadoRecebido = reader["termoAssinadoRecebido"].ToString();
                vo.EmailEnviado = reader["emailEnviado"].ToString();
                vo.PercentualAplicado = reader["percentualAplicado"].ToString();
                vo.EnviadoAoMotaParaContato = reader["enviadoAoMotaParaContato"].ToString();
                vo.AlegacaoLitispendencia = reader["alegacaoLitispendencia"].ToString();
                vo.PediramDesistencia = reader["pediramDesistencia"].ToString();

                //
                vo.NumeroEmbargos = reader["numeroEmbargos"].ToString();
                vo.NumeroExecucao = reader["numeroExecucao"].ToString();

                //
                vos.Add(vo);
            }

            //
            reader.Close();
            MainDataGrid.ItemsSource = vos;
            Title = $"Partes do processo - {count}";
        }

        private void MainDataGrid_MouseDoubleClick(object? sender = null, MouseButtonEventArgs? e = null)
        {
            //
            // ControlModule.OpenWindow(this, new AssociateWindow(Constants.Update, MainDataGrid.FirstId()));
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