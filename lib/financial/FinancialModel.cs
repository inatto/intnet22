using System;
using intnet22.lib.general;
using MySql.Data.MySqlClient;
// ReSharper disable All

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.financial
{
    public static class FinancialModel
    {
        public static VoLancamento? LancamentoFromReader(MySqlConnection? conn, long? id = null)
        {
            //
            if (id is null or 0) return null;
            if (conn is null) throw new Exception("Null Connection");

            MySqlCommand command = new($"select * from lancamento where id_lancamento = " + id, conn);
            var reader = command.ExecuteReader();

            //
            reader.Read();

            //
            var vo = new VoLancamento();
            vo.IdLancamento = (int)(uint)reader["id_lancamento"];
            vo.IdTipoLancamento = MySqlModule.ToLong(reader["id_tipoLancamento"]);
            vo.IdGrupoContabil = MySqlModule.ToLong(reader["id_grupoContabil"]);
            vo.IdContaFinanceira = MySqlModule.ToLong(reader["id_contaFinanceira"]);
            vo.Descricao = reader["descricao"].ToString();
            vo.MetaTitle = reader["metaTitle"].ToString();
            vo.DataBaixa = MySqlModule.ToDateTime(reader["dataBaixa"].ToString());
            vo.DataVencimento = MySqlModule.ToDateTime(reader["dataVencimento"].ToString());
            vo.MesAnoReferencia = MySqlModule.ToDateTime(reader["mesAnoReferencia"].ToString());
            vo.ValorBruto = MySqlModule.FromStringToFloat(reader["valorBruto"].ToString());
            vo.ValorLiquido = MySqlModule.FromStringToFloat(reader["valorLiquido"].ToString());

            //
            reader.Close();

            //
            return vo;
        }

        public static VoGrupoContabil? GrupoContabilFromReader(MySqlConnection? conn, long? id = null)
        {
            //
            if (id is null or 0) return null;
            if (conn is null) throw new Exception("Null Connection");

            MySqlCommand command = new($"select * from grupo_contabil where id_grupoContabil = " + id, conn);
            var reader = command.ExecuteReader();

            //
            reader.Read();

            //
            var vo = new VoGrupoContabil();

            vo.Id = (int)(uint)reader["id_grupoContabil"];
            vo.Nome = reader["nome"].ToString();
            vo.EhDespesa = MySqlModule.FromStringToBool(reader["ehDespesa"].ToString());
            vo.EhReceita = MySqlModule.FromStringToBool(reader["ehReceita"].ToString());

            //
            reader.Close();

            //
            return vo;
        }


    }
}