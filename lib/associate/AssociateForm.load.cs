using intnet22.lib.general;

// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate
{
    /// <summary>
    /// </summary>
    public partial class AssociateForm
    {
        //
        private void Load()
        {
            //
            MalaDiretaLoad();
            StatusMensalidadeLoad();
            // TipoAposentadoriaLoad();

            //
            ControlModule.ComboLoad(_conn, TplPrincipal.ComboSexo, "select tag_sexo, nome from sexo where ifnull(active,0) = 1 order by nome;", "tag_sexo", "nome");
            ControlModule.ComboLoad(_conn, TplPrincipal.ComboEstadoCivil, "select id_estadoCivil, tag_estadoCivil, nome from estado_civil;", "tag_estadoCivil", "nome");
            ControlModule.ComboLoad(_conn, TplFuncional.ComboCarreira, "select id_carreiraServidor, nome from carreira_servidor where ifnull(active,0) = 1 order by nome;", "id_carreiraServidor", "nome");
            ControlModule.ComboLoad(_conn, TplPrincipal.ComboStatusAssociativo, "select id_statusAssociativo, nomeStatAssoc from status_associativo  order by nomeStatAssoc;", "id_statusAssociativo", "nomeStatAssoc");
            ControlModule.ComboLoad(_conn, TplPrincipal.ComboStatusCarreira, "select tag_statusCarreira, nome from status_carreira where ifnull(active,0) = 1 order by nome;", "tag_statusCarreira", "nome");

            //
            ControlModule.ComboLoadArray(new[] { TplPrincipal.ComboUfR, TplPrincipal.ComboUfC, TplPrincipal.ComboRgUf, TplPrincipal.ComboOabUf }, _conn, "select tag_uf, nome from uf order by nome;", "tag_uf", "tag_uf");
        }

        private void StatusMensalidadeLoad()
        {
            //
            TplFuncional.ComboSituacaoMensalidade.SelectedValuePath = "valor";
            TplFuncional.ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Boleto", texto = "Boleto" });
            TplFuncional.ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Carne", texto = "Carnê" });
            TplFuncional.ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "Consignação", texto = "Consignação" });
            TplFuncional.ComboSituacaoMensalidade.Items.Add(new ItemCombo() { valor = "N/A", texto = "N/A" });
        }

        private void MalaDiretaLoad()
        {
            //
            TplPrincipal.ComboComoEnviarJornal.SelectedValuePath = "valor";
            TplPrincipal.ComboComoEnviarJornal.Items.Add(new ItemCombo() { valor = "correios", texto = "Impresso, pelos Correios" });
            TplPrincipal.ComboComoEnviarJornal.Items.Add(new ItemCombo() { valor = "digital", texto = "Digital, por email" });
        }

        /*private void TipoAposentadoriaLoad()
        {
            //
            TplFuncional.ComboTipoAposentadoria.SelectedValuePath = "valor";
            TplFuncional.ComboTipoAposentadoria.Items.Add(new ItemCombo() { valor = "Proporcional", texto = "Proporcional" });
            TplFuncional.ComboTipoAposentadoria.Items.Add(new ItemCombo() { valor = "Integral", texto = "Integral" });
        }*/
    }
}