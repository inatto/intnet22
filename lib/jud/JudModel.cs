using System;
using intnet22.lib.jud;
using MySql.Data.MySqlClient;

// ReSharper disable All

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.financial
{
    public static class JudModel
    {
        public static VoProcessoJud? ProcessoFromReader(MySqlConnection? conn, long? id = null)
        {
            //
            if (id is null or 0) return null;
            if (conn is null) throw new Exception("Null Connection");

            MySqlCommand command = new($"select * from processo_jud where id_processoJud = " + id, conn);
            var reader = command.ExecuteReader();

            //
            reader.Read();

            //
            var vo = new VoProcessoJud();
            vo.IdProcessoJud = (int)(uint)reader["id_processoJud"];
            vo.NrProcessoExecucao = reader["nrProcessoExecucao"].ToString();
            // vo.IdGrupoContabil = MySqlModule.ToLong(reader["id_grupoContabil"]);
            // vo.IdContaFinanceira = MySqlModule.ToLong(reader["id_contaFinanceira"]);
            // vo.DataBaixa = MySqlModule.ToDateTime(reader["dataBaixa"].ToString());
            // vo.ValorLiquido = MySqlModule.FromStringToFloat(reader["valorLiquido"].ToString());

            //
            reader.Close();

            //
            return vo;
        }
    }
}