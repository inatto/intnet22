using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Xceed.Wpf.Toolkit;

// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable LoopCanBeConvertedToQuery

namespace intnet22.lib.general
{
    public static class Constants
    {
        public const string Insert = "I_N_S_E_R_T";
        public const string Update = "U_P_D_A_T_E";
        public const string Delete = "D_E_L_E_T_E";
    }

    public static class MySqlModule
    {
        public static MySqlConnection Connectt()
        {
            //
            MySqlConnection conn = new();
            conn.ConnectionString = "server=anpprev.org.br;user id=anp_homologation;database=anp_homologation;password=^CE__gBCvU}a&+!;persistsecurityinfo=True";
            // conn.ConnectionString = "server=localhost;user id=localroot;database=anp_homologation;password=Camry?!;persistsecurityinfo=True";
            // conn.ConnectionString = "server=anpprev.org.br;user id=anp_production;database=anp_production;password=^CEgBCvU}a&+!;persistsecurityinfo=True";
            // conn.ConnectionString = "server=localhost;user id=root;database=anp_20220426;password=Debora?!;persistsecurityinfo=True";
            conn.Open();

            //
            if (conn is null) throw new ValidationException("Connection to DB failed.");

            //
            return conn;
        }

        /**
         *
         */
        public static string ToHumanDate(string? param, bool direct = false)
        {
            // "10/10/2020 00:00:00"
            if (direct)
            {
                if (!string.IsNullOrEmpty(param) && param.Length >= 10) return param[..10];
            }

            //2021-10-10
            if (!string.IsNullOrEmpty(param) && param.Length >= 10) return $"{param.Substring(8, 2)}/{param.Substring(5, 2)}/{param[..4]}";

            //
            return "";
        }

        public static string ToHumanRefDate(string? param)
        {
            // 01/10/2020
            if (!string.IsNullOrEmpty(param) && param.Length >= 10) return $"{param.Substring(5, 2)}/{param[..4]}";
            return "";
        }

        /**
         * Converte texto para data mysql
         */
        public static string? ToMySqlDate(string? param)
        {
            //
            if (!DateTime.TryParse(param, out _)) return null; // throw new ValidationException($"Data inválida: {param}", null, param);

            //
            if (!string.IsNullOrEmpty(param) && param.Length >= 10) // 10/10/2020
                return $"{param.Substring(6, 4)}-{param.Substring(3, 2)}-{param[..2]}";
            else if (!string.IsNullOrEmpty(param) && param.Length == 7) // 10/2020
                return $"{param.Substring(3, 4)}-{param[..2]}-01";
            else
                return "";
        }

        public static string ToMySqlMoney(string param)
        {
            //
            param = param == "" ? "0" : param;
            param = param.Replace(".", "");
            param = param.Replace(',', '.');

            //
            return param;
        }

        /**
         * Converte string para Boolean
         */
        public static bool FromStringToBool(string? param)
        {
            //
            if (param != null && param.Equals("1", StringComparison.CurrentCultureIgnoreCase)) return true;
            if (param != null && param.Equals("true", StringComparison.CurrentCultureIgnoreCase)) return true;

            //
            if (param != null && param.Equals("false", StringComparison.CurrentCultureIgnoreCase)) return false;
            if (param != null && param.Equals("", StringComparison.CurrentCultureIgnoreCase)) return false;
            if (param != null && param.Equals("0", StringComparison.CurrentCultureIgnoreCase)) return false;

            //
            return false;
        }

        public static string FromBoolToHuman(bool? param)
        {
            //
            if (param is true) return "Sim";
            if (param is true) return "Não";

            //
            return "";
        }


        public static float? FromStringToFloat(string? param)
        {
            //
            if (param is not null && param != "")
                return float.Parse(param);

            //
            return null;
        }

        public static int? ToInt(object? param)
        {
            //
            if (int.TryParse(param?.ToString(), out var i)) return i;

            //
            return null;
        }

        public static long? ToLong(object? param)
        {
            //
            if (long.TryParse(param?.ToString(), out var i)) return i;

            //
            return null;
        }


        /**
         * Converte formato data do mysql para formato datetime c#
         * MySql Date >> C# DateTime
         */
        public static DateTime? MyDateToDateTime(string? param)
        {
            //
            if (string.IsNullOrEmpty(param)) return null;
            if (!DateTime.TryParse(param, out var parsedDate)) return null;

            //
            return parsedDate;
        }

        public static string PhoneStringCheck(string? str)
        {
            //
            str = GeneralModule.RemoveMasks(str);

            //
            if (str is { Length: 10 }) str = $"{str[0..2]} {str.Substring(2, 4)}{str.Substring(6, 4)}";

            //
            return str;
        }

        /**
         * Troca nome do parametro@@ pelo mesmo nome do campo
         */
        public static string ParMirror(string sql)
        {
            //
            var lines = "";
            var subs = sql.Split("set")[1].Split("where")[0].Split(","); // pega parte entre o set e o where
            foreach (var sub in subs)
            {
                var parts = sub.Split('=');
                lines += $"{parts[0].Trim()}=@{parts[0].Trim()}, ";
            }

            // remove ultima virgula
            lines = lines.Remove(lines.Length - 2);

            // adicina o where
            lines = $"{sql.Split("set")[0].Trim()} set {lines} where {sql.Split("where")[1].Trim()}";

            //
            return lines;
        }

        public static void AddParameter(MySqlCommand command, string param, Control control)
        {
            switch (control)
            {
                //
                case MaskedTextBox { Mask: "9999999999" } c:
                    command.Parameters.AddWithValue(param, ToInt(c.Text));
                    break;
                case MaskedTextBox { Mask: "00/00/0000" } c:
                    command.Parameters.AddWithValue(param, ToMySqlDate(c.Text));
                    break;
                // case MaskedTextBox { Mask: "00/0000" } c:
                    // command.Parameters.AddWithValue(param, ToMySqlDate(c.Text));
                    // break;
                case MaskedTextBox c:
                    command.Parameters.AddWithValue(param, GeneralModule.RemoveMasks(c.Text));
                    break;
                case CheckBox c:
                    command.Parameters.AddWithValue(param, c.IsChecked);
                    break;
                case TextBox c:

                    //
                    // command.Parameters.AddWithValue(param, ToMySqlMoney(moneyBox.Text));
                    if (control is MoneyBox moneyBox)
                        command.Parameters.AddWithValue(param, ToMySqlMoney(moneyBox.Text));
                    else
                        command.Parameters.AddWithValue(param, c.Text);

                    //
                    break;
                case ComboBox c:
                    object? value = null;
                    if (c.SelectedValue != null) value = string.IsNullOrEmpty(c.SelectedValue.ToString()) ? null : c.SelectedValue;
                    command.Parameters.AddWithValue(param, value);
                    break;
            }
        }
    }
}