﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using MySql.Data.MySqlClient;

namespace intnet22.lib.financial
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public partial class LancamentoForm
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly string _operacao;
        private readonly long _idLancamento;

        //
        private readonly VoLancamento? _vo;

        public LancamentoForm(string action, string operacao, long idLancamento)
        {
            //
            InitializeComponent();

            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _operacao = operacao ?? throw new ArgumentNullException(nameof(operacao));
            _idLancamento = idLancamento;

            //
            Title = "Lançamento " + (idLancamento > 0 ? idLancamento : "");

            //
            if (_action == Constants.Update && _idLancamento == 0)
                throw new Exception("id lancamento nao pode ser zero ao alterar");

            //
            _conn = MySqlModule.Connectt();

            //
            if (_action == Constants.Update)
                _vo = FinancialModel.LancamentoFromReader(_conn, idLancamento);

            //
            Load();
            Fill();
        }

        //
        private void Load()
        {
            ControlModule.ComboLoad(_conn, ComboGrupoContabil, "select id_grupoContabil, nome from grupo_contabil order by nome;", "id_grupoContabil", "nome");
            ControlModule.ComboLoad(_conn, ComboConta, "select id_contaFinanceira, nome from conta_financeira where active = 1", "id_ContaFinanceira", "nome");
        }

        private void Fill()
        {
            //
            if (_vo is null) return;
            if (_action != Constants.Update) return;

            //
            Fill(_vo);
        }

        private void Fill(VoLancamento vo)
        {
            //
            // var sql = $"select id_grupoContabil, id_contaFinanceira, valorLiquido, valorBruto, descricao" +
            // $", ifnull(dataVencimento,'') as dataVencimento, ifnull(dataBaixa,'') as dataBaixa" +
            // $", ifnull(mesAnoReferencia,'') as mesAnoReferencia from lancamento where id_lancamento = {idLancamento}";

            //
            WpfModule.ControlFill(ComboGrupoContabil, vo.IdGrupoContabil);
            WpfModule.ControlFill(ComboConta, vo.IdContaFinanceira);
            WpfModule.ControlFill(TextValorLiquido, vo.ValorLiquido);
            WpfModule.ControlFill(TextValorBruto, vo.ValorBruto);
            WpfModule.ControlFill(TextDescricao, vo.Descricao);
            WpfModule.ControlFill(MaskVencimento, vo.DataVencimento);
            WpfModule.ControlFill(MaskBaixa, vo.DataBaixa);
            WpfModule.ControlFill(MaskReferencia, vo.MesAnoReferencia);

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Persist();
        }

        private void Persist()
        {
            //
            try
            {
                switch (_action)
                {
                    case Constants.Insert:
                        Insert();
                        break;
                    case Constants.Update when CheckRemover.IsChecked == true:
                        Delete();
                        break;
                    case Constants.Update:
                        Update();
                        break;
                }

                //
                ((IGrid)Owner).LoadGrid();
                this.Close();
            }
            catch (ValidationException e)
            {
                MessageBox.Show(e.Message, "Validação", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Integridade", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }




        private void Insert()
        {
            //
            const string sql = $"insert into lancamento" +
                               $"( operacao,  id_contaFinanceira,  id_grupoContabil,  dataVencimento, dataBaixa,  mesAnoReferencia,  valorBruto,  valorLiquido,  descricao) values " +
                               $"(@operacao, @id_contaFinanceira, @id_grupoContabil, @dataVencimento,@dataBaixa, @mesAnoReferencia, @valorBruto, @valorLiquido, @descricao);";

            //
            MySqlCommand command = new(sql, _conn);
            Parameters(command);

            //
            command.ExecuteNonQuery();
        }

        private void Update()
        {
            //
            var sql = MySqlModule.ParMirror(
                $" update lancamento set " +
                $"  operacao = @@ " +
                $", id_contaFinanceira = @@ " +
                $", id_grupoContabil = @@ " +
                $", dataVencimento = @@ " +
                $", dataBaixa = @@ " +
                $", mesAnoReferencia = @@ " +
                $", valorBruto = @@ " +
                $", valorLiquido = @@ " +
                $", descricao = @@ " +
                $" where id_lancamento = {_idLancamento}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            Parameters(command);
            command.ExecuteNonQuery();
        }

        private void Parameters(MySqlCommand command)
        {
            //
            command.Parameters.AddWithValue("@operacao", _operacao);
            MySqlModule.AddParameter(command, "@id_contaFinanceira", ComboConta);
            MySqlModule.AddParameter(command, "@id_grupoContabil", ComboGrupoContabil);
            MySqlModule.AddParameter(command, "@dataVencimento", MaskVencimento);
            MySqlModule.AddParameter(command, "@dataBaixa", MaskBaixa);
            MySqlModule.AddParameter(command, "@mesAnoReferencia", MaskReferencia);
            MySqlModule.AddParameter(command, "@valorBruto", TextValorBruto);
            MySqlModule.AddParameter(command, "@valorLiquido", TextValorLiquido);
            MySqlModule.AddParameter(command, "@descricao", TextDescricao);
        }

        private void Delete()
        {
            //
            var sql = $"delete from lancamento " +
                      $"where id_lancamento = @id_lancamento;";

            //
            MySqlCommand command = new(sql, _conn);
            command.Parameters.AddWithValue("@id_lancamento", MySqlModule.ToInt(_idLancamento));
            command.ExecuteNonQuery();
        }
    }
}