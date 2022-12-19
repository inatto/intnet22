using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using intnet22.lib.member;
using MySql.Data.MySqlClient;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable UseNullPropagation
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable once UseObjectOrCollectionInitializer

namespace intnet22.lib.associate
{
    /// <summary>
    /// </summary>
    public partial class DependentForm
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly long _idPessoaOwner;

        //
        private VoMember _member = null!;

        //
        public DependentForm(string action, long idPessoaOwner, long? idMembro = null)
        {
            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            if (_action == Constants.Update && idMembro == 0)
                throw new Exception("id lancamento nao pode ser zero ao alterar");

            //
            _idPessoaOwner = idPessoaOwner;
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();
            //
            _action = action ?? throw new ArgumentNullException(nameof(action));
            if (_action == Constants.Update && idMembro == 0)
                throw new Exception("id lancamento nao pode ser zero ao alterar");

            //
            Load();
            Fill(idMembro);

            //
            TextCpf.Focus();
        }

        private void Load()
        {
            //
            ControlModule.ComboLoad(_conn, ComboSexo, "select tag_sexo, nome from sexo where ifnull(active,0) = 1 order by nome;", "tag_sexo", "nome");
            ControlModule.ComboLoad(_conn, ComboEstadoCivil, "select id_estadoCivil, tag_estadoCivil, nome from estado_civil;", "tag_estadoCivil", "nome");

            //
            ParentescoLoad();
        }

        private void ParentescoLoad()
        {
            //
            ComboParentesco.SelectedValuePath = "valor";
            ComboParentesco.Items.Add(new ItemCombo() { valor = "conjuge", texto = "Cônjuge" });
            ComboParentesco.Items.Add(new ItemCombo() { valor = "filho", texto = "Filho(a)" });
            ComboParentesco.Items.Add(new ItemCombo() { valor = "neto", texto = "Neto(a)" });
        }


        private void Fill(long? idMembro)
        {
            //
            if (idMembro is null) return;
            if (_action != Constants.Update) return;

            //
            _member = MemberModel.MemberFromReader(_conn, idMembro);

            //
            var person = _member.VoPerson;
            if (person is null) return;

            //
            TextCpf.Text = person.Cpf;
            TextNome.Text = person.Nome;
            MaskDataNascimento.Text = ControlModule.ToMaskDate(person.DataNascimento);
            MaskFone1.Text = person.CelularPrincipal;
            MaskFone2.Text = person.CelularPrincipal2;

            //
            TextEmailPrincipal.Text = person.EmailPrincipal;
            ComboSexo.SelectedValue = person.TagSexo;
            ComboEstadoCivil.SelectedValue = person.TagEstadoCivil;

            //
            WpfModule.ControlFill(ComboParentesco, _member.Parentesco);
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
                        var idPessoa = PersonInsert();
                        MemberInsert(idPessoa);
                        break;
                    case Constants.Update:
                        if (CheckRemover.IsChecked == true)
                            Delete();
                        else
                        {
                            PersonUpdate();
                            MemberUpdate();
                        }

                        break;
                }


                //
                Close();
            }
            catch (ValidationException e)
            {
                MessageBox.Show(e.Message, "Validação", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message + e.InnerException, "Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Integridade", MessageBoxButton.OK, MessageBoxImage.Error);
                //Environment.Exit(1);
            }

            //
            if (OwnerControl is not null)
                ((IGrid)OwnerControl).LoadGrid();


        }


        private long PersonInsert()
        {
            //
            const string sql = $"insert into pessoa " +
                               $"( nome,  cpf,  dataNascimento,  emailPrincipal,  tag_sexo,  tag_estadoCivil,  tag_tipoPessoa, celularPrincipal, celularPrincipal2) values" +
                               $"(@nome, @cpf, @dataNascimento, @emailPrincipal, @tag_sexo, @tag_estadoCivil, @tag_tipoPessoa, @celularPrincipal, @celularPrincipal2) ";

            //
            MySqlCommand command = new(sql, _conn);

            //
            PersonParameters(command);

            //
            command.Parameters.AddWithValue("@tag_tipopessoa", "dependente");
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }

        private void MemberInsert(long idPessoa)
        {
            //
            const string sql = $"insert into membro " +
                               $"( id_pessoa,  id_pessoa_owner, parentesco) values" +
                               $"(@id_pessoa, @id_pessoa_owner, @parentesco) ";

            //
            MySqlCommand command = new(sql, _conn);


            command.Parameters.AddWithValue("@id_pessoa", idPessoa); //
            command.Parameters.AddWithValue("@id_pessoa_owner", _idPessoaOwner);
            MySqlModule.AddParameter(command, "@parentesco", ComboParentesco);

            //
            command.ExecuteNonQuery();
        }

        private void PersonUpdate()
        {
            // person
            var sql = MySqlModule.ParMirror(
                $" update pessoa set " +
                $"  cpf=@@ " +
                $", nome=@@ " +
                $", dataNascimento=@@ " +
                $", emailPrincipal=@@ " +
                $", tag_sexo=@@ " +
                $", tag_estadoCivil=@@ " +
                $", celularPrincipal=@@ " +
                $", celularPrincipal2=@@ " +
                $" where id_pessoa = {_member.VoPerson.IdPessoa}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            PersonParameters(command);

            //
            command.ExecuteNonQuery();
        }

        private void MemberUpdate()
        {
            // person
            var sql = MySqlModule.ParMirror(
                $" update membro set " +
                $"  parentesco=@@ " +
                $" where id_membro = {_member.IdMembro}"
            );

            MySqlCommand command = new(sql, _conn);
            MySqlModule.AddParameter(command, "@parentesco", ComboParentesco);

            //
            command.ExecuteNonQuery();
        }


        private void PersonParameters(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@cpf", TextCpf);
            MySqlModule.AddParameter(command, "@nome", TextNome);
            MySqlModule.AddParameter(command, "@dataNascimento", MaskDataNascimento);
            MySqlModule.AddParameter(command, "@emailPrincipal", TextEmailPrincipal);
            MySqlModule.AddParameter(command, "@tag_sexo", ComboSexo);
            MySqlModule.AddParameter(command, "@tag_estadoCivil", ComboEstadoCivil);
            MySqlModule.AddParameter(command, "@celularPrincipal", MaskFone1);
            MySqlModule.AddParameter(command, "@celularPrincipal2", MaskFone2);
        }

        private void Delete()
        {
            //
            const string sql = $" delete from membro " +
                               $" where id_membro = @id_membro;";

            //
            MySqlCommand command = new(sql, _conn);
            command.Parameters.AddWithValue("@id_membro", MySqlModule.ToInt(_member.IdMembro));
            command.ExecuteNonQuery();
        }

        private void DataNascimentoPicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            MaskDataNascimento.Text = DataNascimentoPicker.Text;
        }

        private void VerCadastroButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_member == null)
                return;

            // TabControl tabControl = (TabControl)((TabItem)Parent).Parent;
            ControlModule.OpenWindow(Window.GetWindow(this), new AssociateForm(Constants.Update, MySqlModule.ToInt(_member.IdMembro)));
        }
    }
}