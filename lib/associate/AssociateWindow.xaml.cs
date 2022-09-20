using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using intnet22.lib.member;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

namespace intnet22.lib.associate
{
    public partial class AssociateWindow
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly VoMember _member;

        //
        public AssociateWindow(string action, long? idMembro)
        {
            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            if (_action == Constants.Update && idMembro == 0)
                throw new Exception("id lancamento nao pode ser zero ao alterar");

            //
            InitializeComponent();
            GeneralModule.UrlIcon(ImgPessoal, "https://cdn-icons-png.flaticon.com/128/3917/3917711.png");
            GeneralModule.UrlIcon(ImgVinculo, "https://cdn-icons-png.flaticon.com/128/3914/3914283.png");
            GeneralModule.UrlIcon(ImgFuncional, "https://cdn-icons-png.flaticon.com/128/3916/3916690.png");
            GeneralModule.UrlIcon(ImgEnderecos, "https://cdn-icons-png.flaticon.com/128/5074/5074235.png");
            GeneralModule.UrlIcon(ImgInfo, "https://cdn-icons-png.flaticon.com/128/3916/3916607.png");
            GeneralModule.UrlIcon(ImgInstituidor, "https://cdn-icons-png.flaticon.com/128/3917/3917717.png");

            //
            _conn = MySqlModule.Connectt();
            _member = MemberModel.MemberFromReader(_conn, idMembro);
            TplDependents.IdPessoaOwner = _member.IdPessoa;
            TplJuridical.IdMembroOwner = _member.IdMembro;

            //
            Load();
            Fill();

            //
            TextCpf.Focus();
        }

        private void CheckVinculoAtivo_OnClick(object? sender = null, RoutedEventArgs? e = null)
        {
            LblVinculoAtivo.Foreground = Brushes.White;
            LblVinculoAtivo.Background = CheckVinculoAtivo.IsChecked == true ? Brushes.Green : Brushes.DarkRed;
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
            WpfModule.ControlFill(CheckVinculoAtivo, member.Active);
            CheckVinculoAtivo_OnClick();

            //
            LblVinculoAtivo.FontWeight = FontWeights.Bold;
            WpfModule.ControlFill(ComboComoEnviarJornal, member.ComoEnviarJornal);
            WpfModule.ControlFill(ComboStatusAssociativo, member.IdStatusAssociativo);

            // Dados funcionais
            // Principal
            WpfModule.ControlFill(ComboCarreira, member.IdCarreiraServidor);
            WpfModule.ControlFill(ComboStatusCarreira, member.TagStatusCarreira);
            WpfModule.ControlFill(ComboSituacaoMensalidade, member.SituacaoMensalidade);

            //
            WpfModule.ControlFill(TextMatricula, member.MatriculaOrgao);
            WpfModule.ControlFill(TextMatricula, member.MatriculaOrgao);
            WpfModule.ControlFill(TextNrContrato, member.NrContrato);
            WpfModule.ControlFill(MoneyMensalidade, member.ValorMensalidade);
            WpfModule.ControlFill(MaskDataIngresso, member.DataIngresso);
            WpfModule.ControlFill(MaskDataFiliacao, member.DataFiliacao);
            WpfModule.ControlFill(MaskDataDesfiliacao, member.DataDesfiliacao);

            //
            WpfModule.ControlFill(MaskCodigoOrgao, member.CodigoOrgaoPublico);
            WpfModule.ControlFill(TextNomeOrgao, member.OrgaoPublico);
            WpfModule.ControlFill(TextSiglaOrgao, member.SiglaOrgaoPublico);

            //
            WpfModule.ControlFill(MaskCodigoUpag, member.CodigoUnidadePagadora);
            WpfModule.ControlFill(TextNomeUpag, member.NomeUnidadePagadora);
            WpfModule.ControlFill(TextSiglaUpag, member.SiglaUnidadePagadora);
            WpfModule.ControlFill(TextUfUpag, member.UfUnidadePagadora);

            //
            WpfModule.ControlFill(MaskCodigoUorg, member.CodigoUnidadeOrganizacional);
            WpfModule.ControlFill(TextNomeUorg, member.NomeUnidadeOrganizacional);
            WpfModule.ControlFill(TextUfUorg, member.UfUnidadeOrganizacional);

            //
            WpfModule.ControlFill(MaskDataAposentadoria, member.DataAposentadoria);
            WpfModule.ControlFill(ComboTipoAposentadoria, member.TipoAposentadoria);
            WpfModule.ControlFill(TextFracaoAposentadoria, member.FracaoAposentadoria);

            //
            TplInstituidor.Fill(member);
        }

        private void FillPerson(VoPerson? person)
        {
            //
            if (person is null) return;

            //
            WpfModule.ControlFill(TextCpf, person.Cpf);
            WpfModule.ControlFill(TextNome, person.Nome);
            // WpfModule.ControlFill(TextApelido, person.Fantasia);
            WpfModule.ControlFill(CheckNaoMarketing, person.FlagNaoEnviarEmail);

            //
            WpfModule.ControlFill(MaskDataNascimento, person.DataNascimento);
            WpfModule.ControlFill(MaskDataObito, person.DataObito);

            //
            WpfModule.ControlFill(TextEmailPrincipal, person.EmailPrincipal);
            WpfModule.ControlFill(ComboSexo, person.TagSexo);

            //
            WpfModule.ControlFill(ComboEstadoCivil, person.TagEstadoCivil);
            WpfModule.ControlFill(CheckFalecido, person.EhFalecida);

            //
            WpfModule.ControlFill(MaskCelularPrincipal, person.CelularPrincipal);
            WpfModule.ControlFill(MaskCelularComercial, person.CelularComercial);
            WpfModule.ControlFill(MaskTelefoneResidencial, person.TelefoneResidencial);
            WpfModule.ControlFill(MaskTelefoneComercial, person.TelefoneComercial);
            WpfModule.ControlFill(MaskTelefoneComercial2, person.TelefoneComercial2);
            WpfModule.ControlFill(MaskTelefoneExtra, person.TelefoneExtra);

            //
            WpfModule.ControlFill(TextNaturalidade, person.Naturalidade);
            WpfModule.ControlFill(TextNacionalidade, person.Nacionalidade);
            WpfModule.ControlFill(TextConjuge, person.NomeConjuge);
            // WpfModule.ControlFill(TextPai, person.NomePai);
            // WpfModule.ControlFill(TextMae, person.NomeMae);

            //
            WpfModule.ControlFill(TextRgNr, person.RgNr);
            WpfModule.ControlFill(TextRgEmissor, person.RgOrgao);
            WpfModule.ControlFill(ComboRgUf, person.RgUf);
            WpfModule.ControlFill(MaskRgEmissao, person.RgEmissao);
            WpfModule.ControlFill(TextOab, person.OabNr);

            //
            // WpfModule.ControlFill(TextFacebook, person.FacebookUri);
            // WpfModule.ControlFill(TextInstagram, person.InstagramUri);
            // WpfModule.ControlFill(TextTwitter, person.TwitterUri);
            // WpfModule.ControlFill(TextSite, person.SiteUri);

            //
            WpfModule.ControlFill(TextObs, person.PersonObs);
        }

        private void FillAddressRIf(VoAddress? vo)
        {
            //
            if (vo is null) return;

            //
            MaskCepR.Text = vo.Cep;
            ComboUfR.SelectedValue = vo.TagUf;
            TextCidadeR.Text = vo.Cidade;
            TextBairroR.Text = vo.Bairro;
            TextLogR.Text = vo.Logradouro;
            TextNrR.Text = vo.Numero;
            TextCompR.Text = vo.Complemento;
        }

        private void FillAddressCIf(VoAddress? vo)
        {
            //
            if (vo is null) return;

            //
            MaskCepC.Text = vo.Cep;
            ComboUfC.SelectedValue = vo.TagUf;
            TextCidadeC.Text = vo.Cidade;
            TextBairroC.Text = vo.Bairro;
            TextLogC.Text = vo.Logradouro;
            TextNrC.Text = vo.Numero;
            TextCompC.Text = vo.Complemento;
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
                $", cpf=@@ " +
                $", dataNascimento=@@ " +
                $", tag_sexo=@@ " +
                $", tag_estadoCivil=@@ " +
                $", dataObito=@@ " +
                $", ehFalecida=@@ " +
                $", celularPrincipal=@@ " +
                $", celularComercial=@@ " +
                $", telefoneResidencial=@@ " +
                $", telefoneComercial=@@ " +
                $", telefoneComercial2=@@ " +
                $", telefoneExtra=@@ " +
                $", flagNaoEnviarEmail=@@ " +
                $", naturalidade=@@ " +
                $", nacionalidade=@@ " +
                $", nomeConjugue=@@ " +
                $", rg=@@ " +
                $", rgOrgao=@@ " +
                $", rgUf=@@ " +
                $", rgData=@@ " +
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
            MySqlModule.AddParameter(command, "@cpf", TextCpf);
            MySqlModule.AddParameter(command, "@nome", TextNome);
            MySqlModule.AddParameter(command, "@dataNascimento", MaskDataNascimento);
            MySqlModule.AddParameter(command, "@emailPrincipal", TextEmailPrincipal);
            // MySqlModule.AddParameter(command, "@fantasia", TextApelido);
            MySqlModule.AddParameter(command, "@tag_sexo", ComboSexo);
            MySqlModule.AddParameter(command, "@tag_estadoCivil", ComboEstadoCivil);
            MySqlModule.AddParameter(command, "@dataObito", MaskDataObito);
            MySqlModule.AddParameter(command, "@ehFalecida", CheckFalecido);
            MySqlModule.AddParameter(command, "@celularPrincipal", MaskCelularPrincipal);
            MySqlModule.AddParameter(command, "@celularComercial", MaskCelularComercial);
            MySqlModule.AddParameter(command, "@telefoneResidencial", MaskTelefoneResidencial);
            MySqlModule.AddParameter(command, "@telefoneComercial", MaskTelefoneComercial);
            MySqlModule.AddParameter(command, "@telefoneComercial2", MaskTelefoneComercial2);
            MySqlModule.AddParameter(command, "@telefoneExtra", MaskTelefoneExtra);
            MySqlModule.AddParameter(command, "@flagNaoEnviarEmail", CheckNaoMarketing);

            //
            MySqlModule.AddParameter(command, "@naturalidade", TextNaturalidade);
            MySqlModule.AddParameter(command, "@nacionalidade", TextNacionalidade);
            MySqlModule.AddParameter(command, "@nomeConjugue", TextConjuge);
            // MySqlModule.AddParameter(command, "@nomePai", TextPai);
            // MySqlModule.AddParameter(command, "@nomeMae", TextMae);

            //
            MySqlModule.AddParameter(command, "@rg", TextRgNr);
            MySqlModule.AddParameter(command, "@rgOrgao", TextRgEmissor);
            MySqlModule.AddParameter(command, "@rgUf", ComboRgUf);
            MySqlModule.AddParameter(command, "@rgData", MaskRgEmissao);
            MySqlModule.AddParameter(command, "@documentoOAB", TextOab);

            //
            // MySqlModule.AddParameter(command, "@facebook", TextFacebook);
            // MySqlModule.AddParameter(command, "@instagram", TextInstagram);
            // MySqlModule.AddParameter(command, "@twitter", TextTwitter);
            // MySqlModule.AddParameter(command, "@site", TextSite);

            //
            MySqlModule.AddParameter(command, "@obs", TextObs);
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
                $", matriculaOrgao = @@ " +
                $", nrContrato = @@ " +
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
                $", codigoUnidadeOrganizacional = @@ " +
                $", nomeUnidadeOrganizacional = @@ " +
                $", ufUnidadeOrganizacional = @@ " +
                $", codigoInstituidorPensao = @@ " +
                $", nomeInstituidorPensao = @@ " +
                $", dataAposentadoria = @@ " +
                $", tipoAposentadoria = @@ " +
                $", fracaoAposentadoria = @@ " +
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
            MySqlModule.AddParameter(command, "@active", CheckVinculoAtivo);
            MySqlModule.AddParameter(command, "@id_statusAssociativo", ComboStatusAssociativo);
            MySqlModule.AddParameter(command, "@naoEnviarEmail", CheckNaoMarketing);
            MySqlModule.AddParameter(command, "@flagMalaDireta", ComboComoEnviarJornal);
            MySqlModule.AddParameter(command, "@id_carreiraServidor", ComboCarreira);
            MySqlModule.AddParameter(command, "@tag_statusCarreira", ComboStatusCarreira);
            MySqlModule.AddParameter(command, "@situacaoMensalidade", ComboSituacaoMensalidade);
            MySqlModule.AddParameter(command, "@matriculaOrgao", TextMatricula);
            MySqlModule.AddParameter(command, "@nrContrato", TextNrContrato);
            MySqlModule.AddParameter(command, "@valorMensalidade", MoneyMensalidade);
            MySqlModule.AddParameter(command, "@dataIngresso", MaskDataIngresso);
            MySqlModule.AddParameter(command, "@dataFiliacao", MaskDataFiliacao);
            MySqlModule.AddParameter(command, "@dataDesfiliacao", MaskDataDesfiliacao);
            MySqlModule.AddParameter(command, "@codigoOrgaoPublico", MaskCodigoOrgao);
            MySqlModule.AddParameter(command, "@orgaoPublico", TextNomeOrgao);
            MySqlModule.AddParameter(command, "@siglaOrgaoPublico", TextSiglaUpag);
            MySqlModule.AddParameter(command, "@codigoUnidadePagadora", MaskCodigoUpag);
            MySqlModule.AddParameter(command, "@nomeUnidadePagadora", TextNomeUpag);
            MySqlModule.AddParameter(command, "@siglaUnidadePagadora", TextSiglaUpag);
            MySqlModule.AddParameter(command, "@ufUnidadePagadora", TextUfUpag);
            MySqlModule.AddParameter(command, "@codigoUnidadeOrganizacional", MaskCodigoUorg);
            MySqlModule.AddParameter(command, "@nomeUnidadeOrganizacional", TextNomeUorg);
            MySqlModule.AddParameter(command, "@ufUnidadeOrganizacional", TextUfUorg);
            MySqlModule.AddParameter(command, "@dataAposentadoria", MaskDataAposentadoria);
            MySqlModule.AddParameter(command, "@tipoAposentadoria", ComboTipoAposentadoria);
            MySqlModule.AddParameter(command, "@fracaoAposentadoria", TextFracaoAposentadoria);
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
            MySqlModule.AddParameter(command, "@cep", MaskCepR);
            MySqlModule.AddParameter(command, "@tag_uf", ComboUfR);
            MySqlModule.AddParameter(command, "@cidade", TextCidadeR);
            MySqlModule.AddParameter(command, "@bairro", TextBairroR);
            MySqlModule.AddParameter(command, "@logradouro", TextLogR);
            MySqlModule.AddParameter(command, "@numero", TextNrR);
            MySqlModule.AddParameter(command, "@complemento", TextCompR);
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
            MySqlModule.AddParameter(command, "@cep", MaskCepC);
            MySqlModule.AddParameter(command, "@tag_uf", ComboUfC);
            MySqlModule.AddParameter(command, "@cidade", TextCidadeC);
            MySqlModule.AddParameter(command, "@bairro", TextBairroC);
            MySqlModule.AddParameter(command, "@logradouro", TextLogC);
            MySqlModule.AddParameter(command, "@numero", TextNrC);
            MySqlModule.AddParameter(command, "@complemento", TextCompC);
        }
    }
}