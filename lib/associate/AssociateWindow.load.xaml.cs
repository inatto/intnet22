using intnet22.lib.general;

// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate
{
    /// <summary>
    /// </summary>
    public partial class AssociateWindow
    {
        //
        private void Load()
        {
            //
            MalaDiretaLoad();
            StatusMensalidadeLoad();
            TipoAposentadoriaLoad();

            //
            ControlModule.ComboLoad(_conn, ComboSexo, "select tag_sexo, nome from sexo where ifnull(active,0) = 1 order by nome;", "tag_sexo", "nome");
            ControlModule.ComboLoad(_conn, ComboEstadoCivil, "select id_estadoCivil, tag_estadoCivil, nome from estado_civil;", "tag_estadoCivil", "nome");
            ControlModule.ComboLoad(_conn, ComboCarreira, "select id_carreiraServidor, nome from carreira_servidor where ifnull(active,0) = 1 order by nome;", "id_carreiraServidor", "nome");
            ControlModule.ComboLoad(_conn, ComboStatusAssociativo, "select id_statusAssociativo, nomeStatAssoc from status_associativo  order by nomeStatAssoc;", "id_statusAssociativo", "nomeStatAssoc");
            ControlModule.ComboLoad(_conn, ComboStatusCarreira, "select tag_statusCarreira, nome from status_carreira where active = 1 order by nome;", "tag_statusCarreira", "nome");

            //
            ControlModule.ComboLoadArray(new[] { ComboUfR, ComboUfC, ComboRgUf }, _conn, "select tag_uf, nome from uf order by nome;", "tag_uf", "tag_uf");
        }

        private void StatusMensalidadeLoad()
        {
            //
            ComboSituacaoMensalidade.SelectedValuePath = "valor";
            ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Boleto", texto = "Boleto" });
            ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Carne", texto = "Carne" });
            ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Consignação", texto = "Consignação" });
            ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "N/A", texto = "N/A" });
        }

        private void MalaDiretaLoad()
        {
            //
            ComboComoEnviarJornal.SelectedValuePath = "valor";
            ComboComoEnviarJornal.Items.Add(new ItemCombo() { valor = "correios", texto = "Impresso, pelos Correios" });
            ComboComoEnviarJornal.Items.Add(new ItemCombo() { valor = "digital", texto = "Digital, por email" });
        }

        private void TipoAposentadoriaLoad()
        {
            //
            ComboTipoAposentadoria.SelectedValuePath = "valor";
            ComboTipoAposentadoria.Items.Add(new ItemCombo() { valor = "Proporcional", texto = "Proporcional" });
            ComboTipoAposentadoria.Items.Add(new ItemCombo() { valor = "Integral", texto = "Integral" });
        }
    }
}