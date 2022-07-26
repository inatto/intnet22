﻿using System.Windows;
using System.Windows.Controls;
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
        public ProcesseForm(string action, long id) : base(action, id)
        {
            //
            InitializeComponent();
            Title = "Processo " + (Id > 0 ? Id : "");

            //
            Load();
            Fill();
        }

        //
        private void Load()
        {
            //
            if (Action == Constants.Update)
            {
                Vo = JudModel.VoProcesso(Conn, Id);
                TplPartsGrid.IdProcesso = Id;
            }
        }

        private void Fill()
        {
            //
            if (Action != Constants.Update) return;
            if (Vo is null) return;

            //
            VoProcessoJud vo = (VoProcessoJud)Vo;
            WpfModule.ControlFill(TextNome, vo.TituloProcesso);
            WpfModule.ControlFill(TextOriginario, vo.NrProcessoOriginario);
            WpfModule.ControlFill(TextNovaNr, vo.NrProcessoNovo);
            WpfModule.ControlFill(TextObjeto, vo.ObjetoJud);
            WpfModule.ControlFill(TextEscritorio, vo.TempEscritorioAdv);
            WpfModule.ControlFill(TextVara, vo.TempVaraJud);
            WpfModule.ControlFill(TextJuiz, vo.NomeJuiz);
            WpfModule.ControlFill(DataAutuacaoMask, vo.DataAjuizamento);
            WpfModule.ControlFill(UltimaMovimentacaoText, vo.UltimaMovimentacao);
            WpfModule.ControlFill(ParteAutoraText, vo.MetaTitle);
            WpfModule.ControlFill(RequeridoText, vo.MetaObs);
            //
            WpfModule.ControlFill(TextoAvisoText, vo.InsertIp);
            WpfModule.ControlFill(DataPrazoMask, vo.InsertDate);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            //
            Persist();
        }

        protected override void Update()
        {
            //
            var sql = MySqlModule.ParMirror(
                $" update processo_jud set " +
                $"  tituloProcesso = @@ " +
                $", nrProcessoOriginario = @@ " +
                $", nrProcessoNovo = @@ " +
                $", objetoJud = @@ " +
                $", tempEscritorioAdv = @@ " +
                $", tempVaraJud = @@ " +
                $", nomeJuiz = @@ " +
                $", dataAjuizamento = @@ " +
                $", ultimaMovimentacao = @@ " +
                $", metaObs = @@ " +
                $", metaTitle = @@ " +
                $", insertIp = @@ " +
                $", insertDate = @@ " +
                $" where id_processoJud = {Id}"
            );

            //
            MySqlCommand command = new(sql, Conn);
            Parameters(command);
            command.ExecuteNonQuery();
        }

        private void Parameters(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@tituloProcesso", TextNome);
            MySqlModule.AddParameter(command, "@nrProcessoOriginario", TextOriginario);
            MySqlModule.AddParameter(command, "@nrProcessoNovo", TextNovaNr);
            MySqlModule.AddParameter(command, "@objetoJud", TextObjeto);
            MySqlModule.AddParameter(command, "@tempEscritorioAdv", TextEscritorio);
            MySqlModule.AddParameter(command, "@tempVaraJud", TextVara);
            MySqlModule.AddParameter(command, "@nomeJuiz", TextJuiz);
            MySqlModule.AddParameter(command, "@dataAjuizamento", DataAutuacaoMask);
            MySqlModule.AddParameter(command, "@ultimaMovimentacao", UltimaMovimentacaoText);
            MySqlModule.AddParameter(command, "@metaObs", RequeridoText);
            MySqlModule.AddParameter(command, "@metaTitle", ParteAutoraText);
            MySqlModule.AddParameter(command, "@insertIp", TextoAvisoText);
            MySqlModule.AddParameter(command, "@insertDate", DataPrazoMask);
        }

        private void DataAutuacaoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            DataAutuacaoMask.Text = DataAutuacaoPicker.Text;
        }

        private void DataPrazoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            DataPrazoMask.Text = DataPrazoPicker.Text;
        }
    }
}