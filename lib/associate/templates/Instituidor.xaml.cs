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
    public partial class Instituidor
    {
        //
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MySqlConnection? _conn;
        private readonly AutoComplete _autoComplete;


        public Instituidor()
        {
            //
            InitializeComponent();

            //
            _conn = MySqlModule.Connectt();
            _autoComplete = new AutoComplete(_conn, TextBlockIdMembroPai, TextBlockCaption, TextBoxBusca, ListBoxAuto);

            //
            GeneralModule.UrlIcon(ImgInstituidor, "https://cdn-icons-png.flaticon.com/128/3917/3917717.png");
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
                _autoComplete.SetCaption(member.VoMembroPai.IdMembro.ToString(), $"{member.VoMembroPai.VoPerson.Cpf} - {member.VoMembroPai.VoPerson.Nome} ({member.VoMembroPai.VoPerson.EmailPrincipal})");

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

    }
}