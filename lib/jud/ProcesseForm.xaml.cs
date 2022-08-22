using System;
using System.Windows;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using intnet22.lib.jud;
using MySql.Data.MySqlClient;

// ReSharper disable All

namespace intnet22.lib.financial
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public partial class ProcesseForm
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly long _id;

        //
        private readonly VoProcessoJud? _vo;

        public ProcesseForm(string action, long id)
        {
            //
            InitializeComponent();

            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _id = id;

            //
            Title = "Processo " + (_id > 0 ? _id : "");

            //
            if (_action == Constants.Update && _id == 0)
                throw new Exception("id nao pode ser zero ao alterar");

            //
            _conn = MySqlModule.Connectt();

            //
            if (_action == Constants.Update)
                _vo = JudModel.ProcessoFromReader(_conn, id);

            //
            Load();
            Fill();
        }

        //
        private void Load()
        {
            //
            // ControlModule.ComboLoad(_conn, ComboConta, "select id_contaFinanceira, nome from conta_financeira where active = 1", "id_ContaFinanceira", "nome");
        }

        private void Fill()
        {
            //
            if (_vo is null) return;
            if (_action != Constants.Update) return;

            //
            Fill(_vo);
        }

        private void Fill(VoProcessoJud vo)
        {
            //
            // var sql = $"select id_grupoContabil, id_contaFinanceira, valorLiquido, valorBruto, descricao" +
            // $", ifnull(dataVencimento,'') as dataVencimento, ifnull(dataBaixa,'') as dataBaixa" +
            // $", ifnull(mesAnoReferencia,'') as mesAnoReferencia from lancamento where id_lancamento = {idLancamento}";

            //
            WpfModule.ControlFill(TextProcesso, vo.NrProcessoExecucao);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            //
            ((IGrid)Owner).LoadGrid();
            this.Close();
        }
    }
}