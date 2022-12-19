using System;
using intnet22.lib.general;
using intnet22.lib.jud;
using intnet22.lib.member;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable All

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.financial
{
    public static class JudModel
    {
        public static MySqlDataReader ProcessReader(MySqlConnection conn, long? id, string? search)
        {
            //
            var sql = "select * from processo_jud where 1 ";

            //
            if (!String.IsNullOrEmpty(search))
                sql += $" and nrProcessoExecucao like '%{search}%' or nrProcessoOriginario like '%{search}%' or tempVaraJud like '%{search}%' or tempAutorJud like '%{search}%' ";
            //
            var command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            //
            return reader;
        }


        public static VoProcessoJud? VoProcesso(MySqlConnection conn, long? id)
        {
            //
            // if (id is null or 0) return null;
            if (conn is null) throw new Exception("Null Connection");
            if (id is null) throw new Exception("Null Id");

            //
            string sql = @"select 
                            id_processoJud, tituloProcesso, nrProcessoExecucao, dataAjuizamento, dataTransitoJulgado, nrProcessoNovo,
                            nrProcessoOriginario, tempVaraJud, tempEscritorioAdv, ObjetoJud, nomeJuiz, ultimaMovimentacao, metaTitle, metaObs, insertIp, insertDate 
                            from processo_jud where id_processoJud = " + id;

            //
            MySqlCommand command = new(sql, conn);
            var reader = command.ExecuteReader();
            reader.Read();
            var vo = VoProcessoFromReader(reader);
            reader.Close();

            //
            return vo;
        }

        public static VoProcessoJud VoProcessoFromReader(MySqlDataReader reader)
        {
            //
            var vo = new VoProcessoJud();
            vo.IdProcessoJud = (int)(uint)reader["id_processoJud"];
            vo.TituloProcesso = reader["tituloProcesso"].ToString();
            vo.NrProcessoExecucao = reader["nrProcessoExecucao"].ToString();
            vo.NrProcessoOriginario = reader["nrProcessoOriginario"].ToString();
            vo.NrProcessoNovo = reader["nrProcessoNovo"].ToString();
            vo.TempVaraJud = reader["tempVaraJud"].ToString();
            vo.TempEscritorioAdv = reader["tempEscritorioAdv"].ToString();
            vo.ObjetoJud = reader["ObjetoJud"].ToString();
            vo.MetaTitle = reader["metaTitle"].ToString();
            vo.NomeJuiz = reader["nomeJuiz"].ToString();
            vo.UltimaMovimentacao = reader["ultimaMovimentacao"].ToString();
            vo.MetaObs = reader["metaObs"].ToString(); // TODO requerido
            vo.MetaTitle = reader["metaTitle"].ToString(); // TODO parte autora
            vo.InsertIp = reader["insertIp"].ToString();

            //
            vo.InsertDate = MySqlModule.MyDateToDateTime(reader["insertDate"].ToString());
            vo.DataAjuizamento = MySqlModule.MyDateToDateTime(reader["dataAjuizamento"].ToString());
            vo.DataTransitoJulgado = MySqlModule.MyDateToDateTime(reader["dataTransitoJulgado"].ToString());

            //
            return vo;
        }

        public static VoPartJud VoParteFromReader(MySqlDataReader reader)
        {
            //
            var vo = new VoPartJud();

            //
            vo.VoMember = new VoMember();
            vo.VoMember.IdMembro =(int)(uint)reader["id_membro"];
            vo.VoMember.VoPerson = new VoPerson();
            vo.VoMember.VoPerson.Nome = reader["nome"].ToString();

            //
            vo.IdMembro =(int)(uint)reader["id_membro"];
            vo.NumeroExecucao = reader["numeroExecucao"].ToString();
            vo.NumeroEmbargos = reader["numeroEmbargos"].ToString();
            vo.NrPrecatorioJud = reader["nrPrecatorioJud"].ToString();
            vo.SituacaoSireaExequente = reader["situacaoSireaExequente"].ToString();
            vo.UltimoTramite = "";

            //
            return vo;
        }

    }
}