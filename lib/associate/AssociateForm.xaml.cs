using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using intnet22.lib.associate.model;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using intnet22.lib.member;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

namespace intnet22.lib.associate
{
    public partial class AssociateForm
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly VoMember _member;

        //
        public AssociateForm(string action, long? idMembro)
        {
            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            if (_action == Constants.Update && idMembro == 0)
                throw new Exception("id lancamento nao pode ser zero ao alterar");

            //
            InitializeComponent();
            GeneralModule.UrlIcon(ImgPessoal, "https://cdn-icons-png.flaticon.com/128/3917/3917711.png");
            GeneralModule.UrlIcon(ImgFuncional, "https://cdn-icons-png.flaticon.com/128/3916/3916690.png");
            GeneralModule.UrlIcon(ImgInstituidor, "https://cdn-icons-png.flaticon.com/128/3917/3917717.png");
            GeneralModule.UrlIcon(ImgJuridical, "https://cdn-icons-png.flaticon.com/128/3916/3916607.png");
            GeneralModule.UrlIcon(ImgInfo, "https://cdn-icons-png.flaticon.com/128/3916/3916607.png");
            GeneralModule.UrlIcon(ImgFiles, "https://cdn-icons-png.flaticon.com/128/3916/3916607.png");
            GeneralModule.UrlIcon(ImgDependentes, "https://cdn-icons-png.flaticon.com/128/5069/5069169.png");

            //
            _conn = MySqlModule.Connectt();
            _member = MemberModel.MemberFromReader(_conn, idMembro);
            TplDependents.IdPessoaOwner = _member.IdPessoa;
            TplDependents.IdMembroPai = _member.IdMembro;
            TplFiles.IdMember = _member.IdMembro;
            TplJuridical.IdMembroOwner = _member.IdMembro;

            //
            Load();
            Fill();

            //
            TplFiles.FilesLoad();

            //
            TplPrincipal.TextCpf.Focus();

            // muda aba de instituidor/dependentes
            var biz = new AssociateBiz();
            var tem = biz.temInstituidor(_member.IdMembro);
            if (tem == true)
            {
                InstituidorTabItem.Visibility = Visibility.Collapsed;
                TextBlockDependentes.Text = "Pensionista/Dependentes";
            }
            else
            {
                InstituidorTabItem.Visibility = Visibility.Visible;
                TextBlockDependentes.Text = "Dependentes";
            }
        }


        private void Fill()
        {
            //
            if (_action != Constants.Update) return;

            //
            FillMember(_member);
            FillPerson(_member.VoPerson);
            FillAddressRIf(_member.VoPerson.VoEnderecoResidencial);
            FillAddressCIf(_member.VoPerson.VoEnderecoComercial);
        }

        private void FillMember(VoMember member)
        {
            // Vinculo
            WpfModule.ControlFill(TplPrincipal.CheckVinculoAtivo, member.Active);
            TplPrincipal.CheckVinculoAtivo_OnClick();

            //
            TplPrincipal.LblVinculoAtivo.FontWeight = FontWeights.Bold;
            WpfModule.ControlFill(TplPrincipal.ComboComoEnviarJornal, member.ComoEnviarJornal);
            WpfModule.ControlFill(TplPrincipal.ComboStatusAssociativo, member.IdStatusAssociativo);

            // Dados funcionais
            // Principal
            WpfModule.ControlFill(TplFuncional.ComboCarreira, member.IdCarreiraServidor);
            WpfModule.ControlFill(TplPrincipal.ComboStatusCarreira, member.TagStatusCarreira);
            WpfModule.ControlFill(TplFuncional.ComboSituacaoMensalidade, member.SituacaoMensalidade);

            //
            // WpfModule.ControlFill(TextMatricula, member.MatriculaOrgao);
            // WpfModule.ControlFill(TextMatricula, member.MatriculaOrgao);
            // WpfModule.ControlFill(TextNrContrato, member.NrContrato);
            WpfModule.ControlFill(TplFuncional.MoneyMensalidade, member.ValorMensalidade);
            WpfModule.ControlFill(TplFuncional.MaskDataIngresso, member.DataIngresso);
            WpfModule.ControlFill(TplPrincipal.MaskDataFiliacao, member.DataFiliacao);
            WpfModule.ControlFill(TplPrincipal.MaskDataDesfiliacao, member.DataDesfiliacao);

            //
            WpfModule.ControlFill(TplFuncional.MaskCodigoOrgao, member.CodigoOrgaoPublico);
            WpfModule.ControlFill(TplFuncional.TextNomeOrgao, member.OrgaoPublico);
            WpfModule.ControlFill(TplFuncional.TextSiglaOrgao, member.SiglaOrgaoPublico);

            //
            WpfModule.ControlFill(TplFuncional.MaskCodigoUpag, member.CodigoUnidadePagadora);
            WpfModule.ControlFill(TplFuncional.TextNomeUpag, member.NomeUnidadePagadora);
            WpfModule.ControlFill(TplFuncional.TextSiglaUpag, member.SiglaUnidadePagadora);
            WpfModule.ControlFill(TplFuncional.TextUfUpag, member.UfUnidadePagadora);

            //
            // WpfModule.ControlFill(MaskCodigoUorg, member.CodigoUnidadeOrganizacional);
            // WpfModule.ControlFill(TextNomeUorg, member.NomeUnidadeOrganizacional);
            // WpfModule.ControlFill(TextUfUorg, member.UfUnidadeOrganizacional);

            //
            WpfModule.ControlFill(TplFuncional.MaskDataAposentadoria, member.DataAposentadoria);
            // WpfModule.ControlFill(TplFuncional.ComboTipoAposentadoria, member.TipoAposentadoria);
            // WpfModule.ControlFill(TplFuncional.TextFracaoAposentadoria, member.FracaoAposentadoria);

            //
            TplInstituidor.Fill(member);
        }

        private void FillPerson(VoPerson? person)
        {
            //
            if (person is null) return;

            //
            WpfModule.ControlFill(TplPrincipal.TextCpf, person.Cpf);
            WpfModule.ControlFill(TplPrincipal.TextNome, person.Nome);
            // WpfModule.ControlFill(TextApelido, person.Fantasia);
            WpfModule.ControlFill(TplPrincipal.CheckNaoMarketing, person.FlagNaoEnviarEmail);

            //
            WpfModule.ControlFill(TplPrincipal.MaskDataNascimento, person.DataNascimento);
            WpfModule.ControlFill(TplPrincipal.MaskDataObito, person.DataObito);

            //
            WpfModule.ControlFill(TplPrincipal.TextEmailPrincipal, person.EmailPrincipal);
            WpfModule.ControlFill(TplPrincipal.TextEmailSecundario, person.EmailComercial);
            WpfModule.ControlFill(TplPrincipal.ComboSexo, person.TagSexo);

            //
            WpfModule.ControlFill(TplPrincipal.ComboEstadoCivil, person.TagEstadoCivil);
            WpfModule.ControlFill(TplPrincipal.CheckFalecido, person.EhFalecida);

            //
            WpfModule.ControlFill(TplPrincipal.MaskCelularPrincipal, person.CelularPrincipal);
            WpfModule.ControlFill(TplPrincipal.MaskCelularPrincipal2, person.CelularPrincipal2);
            WpfModule.ControlFill(TplPrincipal.MaskTelefoneResidencial, person.TelefoneResidencial);
            WpfModule.ControlFill(TplPrincipal.MaskTelefoneComercial, person.TelefoneComercial);
            // WpfModule.ControlFill(MaskTelefoneComercial2, person.TelefoneComercial2);
            WpfModule.ControlFill(TplPrincipal.MaskTelefoneExtra, person.TelefoneExtra);

            //
            WpfModule.ControlFill(TplPrincipal.TextNaturalidade, person.Naturalidade);
            WpfModule.ControlFill(TplPrincipal.TextNacionalidade, person.Nacionalidade);
            // WpfModule.ControlFill(TplPrincipal.TextConjuge, person.NomeConjuge);
            // WpfModule.ControlFill(TextPai, person.NomePai);
            // WpfModule.ControlFill(TextMae, person.NomeMae);

            //
            WpfModule.ControlFill(TplPrincipal.TextRgNr, person.RgNr);
            WpfModule.ControlFill(TplPrincipal.TextRgEmissor, person.RgOrgao);
            WpfModule.ControlFill(TplPrincipal.ComboRgUf, person.RgUf);
            WpfModule.ControlFill(TplPrincipal.MaskRgEmissao, person.RgEmissao);
            WpfModule.ControlFill(TplPrincipal.TextOab, person.OabNr);
            WpfModule.ControlFill(TplPrincipal.ComboOabUf, person.OabUf);

            //
            // WpfModule.ControlFill(TextFacebook, person.FacebookUri);
            // WpfModule.ControlFill(TextInstagram, person.InstagramUri);
            // WpfModule.ControlFill(TextTwitter, person.TwitterUri);
            // WpfModule.ControlFill(TextSite, person.SiteUri);

            //
            WpfModule.ControlFill(TplMaisInfo.TextObs, person.PersonObs);
        }

        private void FillAddressRIf(VoAddress? vo)
        {
            //
            if (vo is null) return;

            //
            TplPrincipal.MaskCepR.Text = vo.Cep;
            TplPrincipal.ComboUfR.SelectedValue = vo.TagUf;
            TplPrincipal.TextCidadeR.Text = vo.Cidade;
            TplPrincipal.TextBairroR.Text = vo.Bairro;
            TplPrincipal.TextLogR.Text = vo.Logradouro;
            TplPrincipal.TextNrR.Text = vo.Numero;
            TplPrincipal.TextCompR.Text = vo.Complemento;
        }

        private void FillAddressCIf(VoAddress? vo)
        {
            //
            if (vo is null) return;

            //
            TplPrincipal.MaskCepC.Text = vo.Cep;
            TplPrincipal.ComboUfC.SelectedValue = vo.TagUf;
            TplPrincipal.TextCidadeC.Text = vo.Cidade;
            TplPrincipal.TextBairroC.Text = vo.Bairro;
            TplPrincipal.TextLogC.Text = vo.Logradouro;
            TplPrincipal.TextNrC.Text = vo.Numero;
            TplPrincipal.TextCompC.Text = vo.Complemento;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Persist();
        }

        private void Persist()
        {
            //
            try
            {
                //
                switch (_action)
                {
                    case Constants.Insert:
                        Insert();
                        break;
                    //else if (myAction == "UPDATE" && checkBoxRemover.IsChecked == true)
                    //                delete();
                    case Constants.Update:

                        //
                        if (TplPrincipal.MaskDataObito.IsManipulationEnabled && TplPrincipal.MaskDataObito.Text == "__/__/____")
                            throw new ValidationException("Campo data de óbito precisa ser preenchida.");

                        //
                        if (TplPrincipal.MaskDataDesfiliacao.IsManipulationEnabled && TplPrincipal.MaskDataDesfiliacao.Text == "__/__/____")
                            throw new ValidationException("Campo data de desfiliação precisa ser preenchida.");


                        MemberUpdate();
                        PersonUpdate();
                        AddressUpdateR(_member.VoPerson.IdEnderecoResidencial);
                        AddressUpdateC(_member.VoPerson.IdEnderecoComercial);
                        break;
                }

                //
                ((IGrid)Owner).LoadGrid();
                Close();
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
                //Environment.Exit(1);
            }
        }


        private void Insert()
        {
            //
            // var nome = TxtNome.Text;

            //
            // var sql = $"insert into pessoa(nome) values " +
            // $"('{nome}');";

            //
            // MySqlCommand command = new(sql, _conn);
            // command.ExecuteNonQuery();
            // var idPessoa = command.LastInsertedId;
        }

        private void PersonUpdate()
        {
            // person
            var sql = MySqlModule.ParMirror(
                $" update pessoa set " +
                $"  nome=@@ " +
                $", emailPrincipal=@@ " +
                $", emailComercial=@@ " +
                $", cpf=@@ " +
                $", dataNascimento=@@ " +
                $", tag_sexo=@@ " +
                $", tag_estadoCivil=@@ " +
                $", dataObito=@@ " +
                $", ehFalecida=@@ " +
                $", celularPrincipal=@@ " +
                $", telefoneResidencial=@@ " +
                $", telefoneExtra=@@ " +
                $", flagNaoEnviarEmail=@@ " +
                $", naturalidade=@@ " +
                $", nacionalidade=@@ " +
                $", rg=@@ " +
                $", rgOrgao=@@ " +
                $", rgUf=@@ " +
                $", rgData=@@ " +
                $", oabUf=@@ " +
                $", documentoOAB=@@ " +
                $", obs=@@ " +
                $" where id_pessoa = {_member.VoPerson.IdPessoa}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            PersonParameters(command);
            command.ExecuteNonQuery();
        }

        private void PersonParameters(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@cpf", TplPrincipal.TextCpf);
            MySqlModule.AddParameter(command, "@nome", TplPrincipal.TextNome);
            MySqlModule.AddParameter(command, "@dataNascimento", TplPrincipal.MaskDataNascimento);
            MySqlModule.AddParameter(command, "@emailPrincipal", TplPrincipal.TextEmailPrincipal);
            MySqlModule.AddParameter(command, "@emailComercial", TplPrincipal.TextEmailSecundario);
            // MySqlModule.AddParameter(command, "@fantasia", TextApelido);
            MySqlModule.AddParameter(command, "@tag_sexo", TplPrincipal.ComboSexo);
            MySqlModule.AddParameter(command, "@tag_estadoCivil", TplPrincipal.ComboEstadoCivil);
            MySqlModule.AddParameter(command, "@dataObito", TplPrincipal.MaskDataObito);
            MySqlModule.AddParameter(command, "@ehFalecida", TplPrincipal.CheckFalecido);
            MySqlModule.AddParameter(command, "@celularPrincipal", TplPrincipal.MaskCelularPrincipal);
            // MySqlModule.AddParameter(command, "@celularComercial", MaskCelularComercial);
            MySqlModule.AddParameter(command, "@telefoneResidencial", TplPrincipal.MaskTelefoneResidencial);
            // MySqlModule.AddParameter(command, "@telefoneComercial", MaskTelefoneComercial);
            // MySqlModule.AddParameter(command, "@telefoneComercial2", MaskTelefoneComercial2);
            MySqlModule.AddParameter(command, "@telefoneExtra", TplPrincipal.MaskTelefoneExtra);
            MySqlModule.AddParameter(command, "@flagNaoEnviarEmail", TplPrincipal.CheckNaoMarketing);

            //
            MySqlModule.AddParameter(command, "@naturalidade", TplPrincipal.TextNaturalidade);
            MySqlModule.AddParameter(command, "@nacionalidade", TplPrincipal.TextNacionalidade);
            // MySqlModule.AddParameter(command, "@nomeConjugue", TplPrincipal.TextConjuge);
            // MySqlModule.AddParameter(command, "@nomePai", TextPai);
            // MySqlModule.AddParameter(command, "@nomeMae", TextMae);

            //
            MySqlModule.AddParameter(command, "@rg", TplPrincipal.TextRgNr);
            MySqlModule.AddParameter(command, "@rgOrgao", TplPrincipal.TextRgEmissor);
            MySqlModule.AddParameter(command, "@rgUf", TplPrincipal.ComboRgUf);
            MySqlModule.AddParameter(command, "@rgData", TplPrincipal.MaskRgEmissao);
            MySqlModule.AddParameter(command, "@documentoOAB", TplPrincipal.TextOab);
            MySqlModule.AddParameter(command, "@oabUf", TplPrincipal.ComboOabUf);

            //
            // MySqlModule.AddParameter(command, "@facebook", TextFacebook);
            // MySqlModule.AddParameter(command, "@instagram", TextInstagram);
            // MySqlModule.AddParameter(command, "@twitter", TextTwitter);
            // MySqlModule.AddParameter(command, "@site", TextSite);

            //
            MySqlModule.AddParameter(command, "@obs", TplMaisInfo.TextObs);
        }


        private void MemberUpdate()
        {
            //
            var sql = MySqlModule.ParMirror(
                $" update membro set " +
                $"  active = @@ " +
                $", id_membro_pai = @@ " +
                $", id_statusAssociativo = @@ " +
                $", naoEnviarEmail = @@ " +
                $", flagMalaDireta = @@ " +
                $", id_carreiraServidor = @@ " +
                $", tag_statusCarreira = @@ " +
                $", situacaoMensalidade = @@ " +
                // $", matriculaOrgao = @@ " +
                // $", nrContrato = @@ " +
                $", valorMensalidade = @@ " +
                $", dataIngresso = @@ " +
                $", dataFiliacao = @@ " +
                $", dataDesfiliacao = @@ " +
                $", codigoOrgaoPublico = @@ " +
                $", orgaoPublico = @@ " +
                $", siglaOrgaoPublico = @@ " +
                $", codigoUnidadePagadora = @@ " +
                $", nomeUnidadePagadora = @@ " +
                $", siglaUnidadePagadora = @@ " +
                $", ufUnidadePagadora = @@ " +
                // $", codigoUnidadeOrganizacional = @@ " +
                // $", nomeUnidadeOrganizacional = @@ " +
                // $", ufUnidadeOrganizacional = @@ " +
                $", codigoInstituidorPensao = @@ " +
                $", nomeInstituidorPensao = @@ " +
                $", dataAposentadoria = @@ " +
                // $", tipoAposentadoria = @@ " +
                // $", fracaoAposentadoria = @@ " +
                $" where id_membro = {_member.IdMembro}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            MemberParameters(command);
            command.ExecuteNonQuery();
        }

        private void MemberParameters(MySqlCommand command)
        {
            //
            TplInstituidor.Parameters(command);

            //
            if (TplPrincipal.CheckFalecido.IsChecked == true) TplPrincipal.CheckVinculoAtivo.IsChecked = false;
            if (TplPrincipal.MaskDataDesfiliacao.Text != "__/__/____") TplPrincipal.CheckVinculoAtivo.IsChecked = false;

            //
            MySqlModule.AddParameter(command, "@active", TplPrincipal.CheckVinculoAtivo);
            MySqlModule.AddParameter(command, "@id_statusAssociativo", TplPrincipal.ComboStatusAssociativo);
            MySqlModule.AddParameter(command, "@naoEnviarEmail", TplPrincipal.CheckNaoMarketing);
            MySqlModule.AddParameter(command, "@flagMalaDireta", TplPrincipal.ComboComoEnviarJornal);
            MySqlModule.AddParameter(command, "@id_carreiraServidor", TplFuncional.ComboCarreira);
            MySqlModule.AddParameter(command, "@tag_statusCarreira", TplPrincipal.ComboStatusCarreira);
            MySqlModule.AddParameter(command, "@situacaoMensalidade", TplFuncional.ComboSituacaoMensalidade);
            // MySqlModule.AddParameter(command, "@matriculaOrgao", TextMatricula);
            // MySqlModule.AddParameter(command, "@nrContrato", TextNrContrato);
            MySqlModule.AddParameter(command, "@valorMensalidade", TplFuncional.MoneyMensalidade);
            MySqlModule.AddParameter(command, "@dataIngresso", TplFuncional.MaskDataIngresso);
            MySqlModule.AddParameter(command, "@dataFiliacao", TplPrincipal.MaskDataFiliacao);
            MySqlModule.AddParameter(command, "@dataDesfiliacao", TplPrincipal.MaskDataDesfiliacao);
            MySqlModule.AddParameter(command, "@codigoOrgaoPublico", TplFuncional.MaskCodigoOrgao);
            MySqlModule.AddParameter(command, "@orgaoPublico", TplFuncional.TextNomeOrgao);
            MySqlModule.AddParameter(command, "@siglaOrgaoPublico", TplFuncional.TextSiglaUpag);
            MySqlModule.AddParameter(command, "@codigoUnidadePagadora", TplFuncional.MaskCodigoUpag);
            MySqlModule.AddParameter(command, "@nomeUnidadePagadora", TplFuncional.TextNomeUpag);
            MySqlModule.AddParameter(command, "@siglaUnidadePagadora", TplFuncional.TextSiglaUpag);
            MySqlModule.AddParameter(command, "@ufUnidadePagadora", TplFuncional.TextUfUpag);
            // MySqlModule.AddParameter(command, "@codigoUnidadeOrganizacional", MaskCodigoUorg);
            // MySqlModule.AddParameter(command, "@nomeUnidadeOrganizacional", TextNomeUorg);
            // MySqlModule.AddParameter(command, "@ufUnidadeOrganizacional", TextUfUorg);
            MySqlModule.AddParameter(command, "@dataAposentadoria", TplFuncional.MaskDataAposentadoria);
            // MySqlModule.AddParameter(command, "@tipoAposentadoria", TplFuncional.ComboTipoAposentadoria);
            // MySqlModule.AddParameter(command, "@fracaoAposentadoria", TplFuncional.TextFracaoAposentadoria);
        }

        private void AddressUpdateR(int? id)
        {
            //
            if (id is null) return;

            // person
            var sql = MySqlModule.ParMirror(
                $" update endereco set " +
                $"  cep=@@ " +
                $", tag_uf=@@ " +
                $", cidade=@@ " +
                $", bairro=@@ " +
                $", logradouro=@@ " +
                $", numero=@@ " +
                $", complemento=@@ " +
                $" where id_endereco = {id}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            AddressParametersR(command);
            command.ExecuteNonQuery();
        }

        private void AddressParametersR(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@cep", TplPrincipal.MaskCepR);
            MySqlModule.AddParameter(command, "@tag_uf", TplPrincipal.ComboUfR);
            MySqlModule.AddParameter(command, "@cidade", TplPrincipal.TextCidadeR);
            MySqlModule.AddParameter(command, "@bairro", TplPrincipal.TextBairroR);
            MySqlModule.AddParameter(command, "@logradouro", TplPrincipal.TextLogR);
            MySqlModule.AddParameter(command, "@numero", TplPrincipal.TextNrR);
            MySqlModule.AddParameter(command, "@complemento", TplPrincipal.TextCompR);
        }


        private void AddressUpdateC(int? id)
        {
            //
            if (id is null) return;

            // person
            var sql = MySqlModule.ParMirror(
                $" update endereco set " +
                $"  cep=@@ " +
                $", tag_uf=@@ " +
                $", cidade=@@ " +
                $", bairro=@@ " +
                $", logradouro=@@ " +
                $", numero=@@ " +
                $", complemento=@@ " +
                $" where id_endereco = {id}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            AddressParametersC(command);
            command.ExecuteNonQuery();
        }


        private void AddressParametersC(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@cep", TplPrincipal.MaskCepC);
            MySqlModule.AddParameter(command, "@tag_uf", TplPrincipal.ComboUfC);
            MySqlModule.AddParameter(command, "@cidade", TplPrincipal.TextCidadeC);
            MySqlModule.AddParameter(command, "@bairro", TplPrincipal.TextBairroC);
            MySqlModule.AddParameter(command, "@logradouro", TplPrincipal.TextLogC);
            MySqlModule.AddParameter(command, "@numero", TplPrincipal.TextNrC);
            MySqlModule.AddParameter(command, "@complemento", TplPrincipal.TextCompC);
        }
    }

    internal class MessageBoxButtons
    {
    }

    internal class DialogResult
    {
    }
}