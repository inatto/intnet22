using System;
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
    public partial class GrupoContabilForm
    {
        //
        private readonly MySqlConnection? _conn;
        private readonly string _action;
        private readonly long _id;

        //
        private readonly VoGrupoContabil? _vo;

        public GrupoContabilForm(string action, long id)
        {
            //
            InitializeComponent();

            //
            _id = id;
            _action = action ?? throw new ArgumentNullException(nameof(action));
            Title = "Grupo contábil " + (id > 0 ? id : "");

            //
            if (_action == Constants.Update && _id == 0)
                throw new Exception("id nao pode ser zero ao alterar");

            //
            _conn = MySqlModule.Connectt();

            //
            if (_action == Constants.Update)
                _vo = FinancialModel.GrupoContabilFromReader(_conn, id);

            //
            Fill(_vo);
        }

        private void Fill(VoGrupoContabil? vo)
        {
            //
            if (vo is null) return;
            if (_action != Constants.Update) return;

            //
            WpfModule.ControlFill(TextNome, vo.Nome);
            WpfModule.ControlFill(CheckEhReceita, vo.EhReceita);
            WpfModule.ControlFill(CheckEhDespesa, vo.EhDespesa);
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
            const string sql = $"insert into grupo_contabil" +
                               $"( nome,  ehReceita,  ehDespesa) values " +
                               $"(@nome, @ehReceita, @ehDespesa);";

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
                $" update grupo_contabil set " +
                $"  nome = @@ " +
                $" ,ehReceita = @@ " +
                $" ,ehDespesa = @@ " +
                $" where id_grupoContabil = {_id}"
            );

            //
            MySqlCommand command = new(sql, _conn);
            Parameters(command);
            command.ExecuteNonQuery();
        }

        private void Parameters(MySqlCommand command)
        {
            //
            MySqlModule.AddParameter(command, "@nome", TextNome);
            MySqlModule.AddParameter(command, "@ehReceita", CheckEhReceita);
            MySqlModule.AddParameter(command, "@ehDespesa", CheckEhDespesa);
        }

        private void Delete()
        {
            //
            var sql = $"delete from grupo_contabil " +
                      $"where id_grupoContabil = @id_grupoContabil";

            //
            MySqlCommand command = new(sql, _conn);
            command.Parameters.AddWithValue("@id_grupoContabil", MySqlModule.ToInt(_id));
            command.ExecuteNonQuery();
        }
    }
}