using System.Collections.Generic;
using System.Web;
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
            LoadGrid();
        }

        public void LoadGrid()
        {
            //
            var sql = $" select id_lancamento, descricao, valorBruto, valorLiquido, cf.nome, " +
                      $" DATE_FORMAT(dataBaixa, '%d/%m/%Y') as dataBaixa, DATE_FORMAT(dataVencimento, '%d/%m/%Y') as dataVencimento " +
                      $" from lancamento " +
                      $" left join conta_financeira cf on lancamento.id_contaFinanceira = cf.id_contaFinanceira " +
                      $" where operacao = '{MyOperacao()}' " +
                      // $" and ifnull(id_contaFinanceira, 0) = {ComboConta.SelectedValue ?? 0} " +
                      $" order by dataVencimento";

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
                vo.OutConta = reader["nome"].ToString();
                vo.OutBaixa = reader["dataBaixa"].ToString();
                vo.OutVencimento = reader["dataVencimento"].ToString();
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

        public static void LinkExtrato(string idConta, string dtInicio, string dtFim)
        {
            //
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //
            if (idConta == "null" || dtInicio == "" || dtFim == "")
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
            GeneralModule.OpenUrl("https://dev.anpprev.org.br/anp/financial/extrato?" + queryString.ToString());
        }
    }
}