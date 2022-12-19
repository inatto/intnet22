using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable All

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.associate.model
{
    public class AssociateBiz
    {
        public bool? temInstituidor(long? id, MySqlConnection? conn = null)
        {
            //
            if (conn == null) conn = MySqlModule.Connectt();

            //
            var sql = "select id_membro from membro where id_membro_pai = " + id;
            MySqlCommand command = new(sql, conn);
            var reader = command.ExecuteReader();

            //
            var read = reader.Read();
            if (!read) return false;
            //
            reader.Close();
            conn.Close();

            //
            return true;
        }
    }
}