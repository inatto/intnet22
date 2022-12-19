using System.Windows;
using System.Windows.Input;
using intnet22.lib.general;
using intnet22.lib.member;
using MySql.Data.MySqlClient;

// ReSharper disable LoopCanBeConvertedToQuery

// ReSharper disable once UseObjectOrCollectionInitializer

//
namespace intnet22.lib.associate.templates
{
    /// <summary>
    /// </summary>
    public partial class InstituidorPanel
    {
        //
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MySqlConnection? _conn;
        private readonly AutoComplete _autoComplete;


        public InstituidorPanel()
        {
            //
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();
            _autoComplete = new AutoComplete(_conn, TextBlockIdMembroPai, TextBlockCaption, TextBoxBusca, ListBoxAuto);
        }

        private void TextInstituidor_KeyUp(object sender, KeyEventArgs e)
        {
            //
            _autoComplete.SearchBox_KeyUp();
        }

        private void AutoCompleteList_KeyDown(object sender, KeyEventArgs e)
        {
            _autoComplete.ListBox_KeyDown(e);
        }

        public void Fill(VoMember member)
        {
            // Instituidor
            if (member.VoMembroPai?.VoPerson != null)
            {
                var cpf = member.VoMembroPai.VoPerson.Cpf;
                if (cpf == "") cpf = "[sem cpf]";
                _autoComplete.SetCaption(member.VoMembroPai.IdMembro.ToString(), $"{cpf} - {member.VoMembroPai.VoPerson.Nome} ({member.VoMembroPai.VoPerson.EmailPrincipal})");
            }


            //
            TextCodigoInstituidor.Text = member.CodigoInstituidorPensao;
            TextNomeInstituidor.Text = member.NomeInstituidorPensao;
        }

        public void Parameters(MySqlCommand command)
        {
            //
            command.Parameters.AddWithValue("@id_membro_pai", string.IsNullOrEmpty(TextBlockIdMembroPai.Text) ? null : TextBlockIdMembroPai.Text);
            command.Parameters.AddWithValue("@codigoInstituidorPensao", TextCodigoInstituidor.Text);
            command.Parameters.AddWithValue("@nomeInstituidorPensao", TextNomeInstituidor.Text);
        }

        private void ButtonVerInstituidor_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBlockIdMembroPai.Text == "")
            {
                MessageBox.Show("Instituidor não informado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TabControl tabControl = (TabControl)((TabItem)Parent).Parent;
            ControlModule.OpenWindow(Window.GetWindow(this), new AssociateForm(Constants.Update, MySqlModule.ToInt(TextBlockIdMembroPai.Text)));
        }
    }
}