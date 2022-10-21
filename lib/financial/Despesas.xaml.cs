using System.Collections.Generic;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable UseObjectOrCollectionInitializer

namespace intnet22.lib.financial
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public partial class Despesas : IGrid
    {
        //
        private readonly MySqlConnection? _conn;

        //
        public Despesas()
        {
            //
            InitializeComponent();

            //
            GridPrincipal.IsReadOnly = true;
            _conn = MySqlModule.Connectt();

            //
            Load();
        }

        protected virtual string MyOperacao()
        {
            //
            this.Title = "Despesas";
            return "debito";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ControlModule.WindowEscape(sender, e, this);
        }

        //
        private void Load()
        {
            //
            ControlModule.ComboLoad(_conn, ComboConta, "select id_contaFinanceira, nome from conta_financeira where active = 1", "id_ContaFinanceira", "nome");

            //
            if (MyOperacao() == "debito")
                ControlModule.ComboLoad(_conn, ComboGrupo, "select id_grupoContabil, nome from grupo_contabil where ehDespesa = 1 order by nome;", "id_grupoContabil", "nome");
            else
                ControlModule.ComboLoad(_conn, ComboGrupo, "select id_grupoContabil, nome from grupo_contabil where ehReceita = 1 order by nome;", "id_grupoContabil", "nome");

            //
            LoadGrid();
        }

        public void LoadGrid()
        {
            //
            var sql = $" select id_lancamento, descricao, valorBruto, valorLiquido, cf.nome, " +
                      $" DATE_FORMAT(dataBaixa, '%d/%m/%Y') as dataBaixa, DATE_FORMAT(dataVencimento, '%d/%m/%Y') as dataVencimento " +
                      $" from lancamento " +
                      $" left join conta_financeira cf on lancamento.id_contaFinanceira = cf.id_contaFinanceira " +
                      $" where operacao = '{MyOperacao()}' ";
                      // $" and ifnull(lancamento.id_contaFinanceira, 0) = {ComboConta.SelectedValue ?? 0} " +
                      // $" order by dataVencimento";

            //
            if (ComboConta.SelectedIndex > 0)
                sql += $" and ifnull(lancamento.id_contaFinanceira, 0) = {ComboConta.SelectedValue ?? 0} ";

            //
            if (ComboGrupo.SelectedIndex > 0)
                sql += $" and ifnull(lancamento.id_grupoContabil, 0) = {ComboGrupo.SelectedValue ?? 0} ";

            //
            sql += " order by dataVencimento ";

            //
            MySqlCommand command = new(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var vos = new List<VoLancamento>();
            while (reader.Read())
            {
                //
                var vo = new VoLancamento();

                //
                vo.IdLancamento = (int)(uint)reader["id_lancamento"];
                vo.Descricao = reader["descricao"].ToString();
                // vo.DataBaixa = MySqlModule.ToDateTime(reader["dataBaixa"].ToString());
                // vo.DataVencimento = MySqlModule.ToDateTime(reader["dataVencimento"].ToString());
                vo.ValorBruto = MySqlModule.FromStringToFloat(reader["valorBruto"].ToString());
                vo.ValorLiquido = MySqlModule.FromStringToFloat(reader["valorLiquido"].ToString());
                reader["nome"].ToString();
                reader["dataBaixa"].ToString();
                reader["dataVencimento"].ToString();
                vos.Add(vo);
            }

            //
            reader.Close();
            GridPrincipal.ItemsSource = vos;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            //
            LancamentoForm window = new(Constants.Insert, MyOperacao(), 0);
            ControlModule.OpenWindow(this, window);
        }

        private void DataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
            if (GridPrincipal.SelectedCells.Count <= 0) return;
            var idLancamento = ((VoLancamento)GridPrincipal.SelectedCells[0].Item).IdLancamento;

            //
            LancamentoForm window = new(Constants.Update, MyOperacao(), idLancamento);
            ControlModule.OpenWindow(this, window);
        }

        private void btnExtrato_Click(object sender, RoutedEventArgs e)
        {
            //
            var idConta = ComboConta.SelectedItem is null ? "null" : ((ItemCombo)ComboConta.SelectedItem).valor + "";
            LinkExtrato(idConta, MaskInicio.Text, MaskFim.Text);
        }

        private void btnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            //
            var idConta = ComboConta.SelectedItem is null ? "" : ((ItemCombo)ComboConta.SelectedItem).valor + "";
            var idGrupo = ComboGrupo.SelectedItem is null ? "" : ((ItemCombo)ComboGrupo.SelectedItem).valor + "";
            LinkRelatorio(idConta, idGrupo, MaskInicio.Text, MaskFim.Text, MyOperacao());
        }

        private static void LinkExtrato(string idConta, string dtInicio, string dtFim)
        {
            //
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //
            if (idConta == "null" || dtInicio == "__/__/____" || dtFim == "__/__/____")
            {
                MessageBox.Show("Escolha a conta, inicio e fim");
                return;
            }

            //
            queryString.Add("byPassLog", "1");
            queryString.Add("dti", dtInicio);
            queryString.Add("dtf", dtFim);
            queryString.Add("idc", idConta);

            //
            GeneralModule.OpenUrl("https://dev.anpprev.org.br/anp/financial/extrato?" + queryString);
        }

        private static void LinkRelatorio(string idConta, string idGrupo, string dtInicio, string dtFim, string operacao, bool relatorioTotal = false)
        {
            //
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //
            if (dtInicio == "__/__/____" || dtFim == "__/__/____")
            {
                MessageBox.Show("Por favor defina o período (início e fim).");
                return;
            }

            //
            queryString.Add("byPassLog", "1");
            queryString.Add("c501b0455cd401f7b98fecad1fdbd16d2017244e", idConta);
            queryString.Add("14208be12f170dfdfb4e5abe6c1729d53b40d17a", idGrupo);
            queryString.Add("a6eec75e4670d97460f76bb282ecce68aed2592e", dtInicio);
            queryString.Add("7e98faacc9ce65cff4c42b76aa71bd2f2df44c9d", dtFim);
            queryString.Add("66c15890e84c39c4d8c2d3ebd07ba11c906fb1ee", operacao);

            //
            if (relatorioTotal)
                queryString.Add("366ac7983f9f5ccc371743590ceee53bbc81d0fc", "True");

            //
            GeneralModule.OpenUrl("https://dev.anpprev.org.br/fin/entryReport?" + queryString);
        }

        private void ComboConta_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGrid();
        }

        private void ComboGrupo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGrid();
        }

        private void MaskInicio_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // LoadGrid();
        }

        private void MaskFim_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // LoadGrid();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //
            var idConta = ComboConta.SelectedItem is null ? "" : ((ItemCombo)ComboConta.SelectedItem).valor + "";
            var idGrupo = ComboGrupo.SelectedItem is null ? "" : ((ItemCombo)ComboGrupo.SelectedItem).valor + "";
            LinkRelatorio(idConta, idGrupo, MaskInicio.Text, MaskFim.Text, MyOperacao(), true);
        }
    }
}