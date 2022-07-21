//

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace intnet22.lib.general

{
    public class AutoComplete
    {
        private readonly MySqlConnection _conn;
        private readonly TextBlock _idBlock;
        private readonly TextBlock _captionBlock;
        private readonly TextBox _searchBox;
        private readonly ListBox _listBox;

        public AutoComplete(MySqlConnection conn, TextBlock idBlock, TextBlock captionBlock, TextBox searchBox, ListBox listBox)
        {
            //
            _conn = conn;
            _idBlock = idBlock;
            _captionBlock = captionBlock;
            _searchBox = searchBox;
            _listBox = listBox;
            _conn = conn;
        }

        public void SearchBox_KeyUp()
        {
            //
            _listBox.Items.Clear();
            var searchString = (_searchBox).Text;
            if (searchString is { Length: < 3 } or null) return;

            // Clear the list

            var list = Load(searchString);
            if (list == null) return;
            LoopData(list);
        }

        private void LoopData(List<ItemCombo> list)
        {
            //
            foreach (var item in list)
            {
                _listBox.Items.Add(item);
            }

            _listBox.MouseLeftButtonUp += (_, _) => ItemSelected();
        }

        public void ListBox_KeyDown(KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;
            ItemSelected();
        }

        private void ItemSelected()
        {
            //
            var item = (_listBox).SelectedItem;
            var valor = (item as ItemCombo)?.valor;
            var texto = item?.ToString();

            //
            SetCaption(valor, texto);
            // _searchBox.Focus();

            // move cursor para o final se houver texto
            // _searchBox.SelectionStart = _searchBox.Text.Length;
        }

        public void SetCaption(string? id, string? caption)
        {
            //
            _idBlock.Text = id;
            _captionBlock.Text = caption;
        }

        private List<ItemCombo>? Load(string searchString)
        {
            //
            if (searchString.Length < 3) return null;

            //
            // var sql = $"select id_pessoa, nome, emailPrincipal, cpf, matricula from pessoa where " +
                      // $"nome like '%{searchString}%' " +
                      // $"or pessoa.emailPrincipal like '%{searchString}%' " +
                      // $"or pessoa.cpf like '%{searchString}%' " +
                      // $"or pessoa.celularPrincipal like '%{searchString}%' " +
                      // $"or pessoa.matricula like '%{searchString}%' " +
                      // $"order by nome limit 100; ";

            //
            var sql = $"select id_membro, id_pessoa, nome, emailPrincipal, cpf, matriculaOrgao from vw_membro where " +
                      $"nome like '%{searchString}%' " +
                      $"or emailPrincipal like '%{searchString}%' " +
                      $"or cpf like '%{searchString}%' " +
                      $"or celularPrincipal like '%{searchString}%' " +
                      $"or matriculaOrgao like '%{searchString}%' " +
                      $"order by nome limit 100; ";

            //
            MySqlCommand command = new(sql, _conn);
            var reader = command.ExecuteReader();

            //
            var list = new List<ItemCombo>();
            while (reader.Read())
            {
                //
                var caption = $"{reader["cpf"]} - {reader["nome"]} ({reader["emailPrincipal"]})";
                list.Add(new ItemCombo() { valor = reader["id_membro"].ToString(), texto = caption });
            }

            //
            reader.Close();
            return list;
        }
    }
}