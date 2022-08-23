using System;
using intnet22.lib.general;
using intnet22.lib.person;
using MySql.Data.MySqlClient;

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.member
{
    public static class MemberModel
    {
        public static VoMember MemberFromReader(MySqlConnection? conn, long? idMembro = null)
        {
            //
            if (conn is null) throw new Exception("Null Connection");

            MySqlCommand command = new($"select * from membro where id_membro = " + idMembro, conn);
            var reader = command.ExecuteReader();

            //
            reader.Read();

            //
            var vo = new VoMember();
            vo.IdMembro = (int)(uint)reader["id_membro"];
            vo.IdMembroPai = MySqlModule.ToInt(reader["id_membro_pai"]);
            vo.IdPessoa = (int)(uint)reader["id_pessoa"];
            vo.IdStatusAssociativo = MySqlModule.ToInt(reader["id_statusAssociativo"]);
            vo.IdCarreiraServidor = MySqlModule.ToInt(reader["id_carreiraServidor"]);

            // Vinculo
            vo.Active = MySqlModule.FromStringToBool(reader["active"].ToString());
            vo.ComoEnviarJornal = reader["flagMalaDireta"].ToString();

            // Dados funcionais
            vo.TagStatusCarreira = reader["tag_StatusCarreira"].ToString();
            vo.SituacaoMensalidade = reader["situacaoMensalidade"].ToString();
            vo.MatriculaOrgao = reader["matriculaOrgao"].ToString();
            vo.NrContrato = reader["nrContrato"].ToString();
            vo.ValorMensalidade = MySqlModule.FromStringToFloat(reader["valorMensalidade"].ToString());
            vo.DataIngresso = MySqlModule.MyDateToDateTime(reader["dataIngresso"].ToString());
            vo.DataFiliacao = MySqlModule.MyDateToDateTime(reader["dataFiliacao"].ToString());
            vo.DataDesfiliacao = MySqlModule.MyDateToDateTime(reader["dataDesfiliacao"].ToString());
            //
            vo.CodigoOrgaoPublico = reader["codigoOrgaoPublico"].ToString();
            vo.OrgaoPublico = reader["orgaoPublico"].ToString();
            vo.SiglaOrgaoPublico = reader["siglaOrgaoPublico"].ToString();
            //
            vo.CodigoUnidadePagadora = reader["codigoUnidadePagadora"].ToString();
            vo.NomeUnidadePagadora = reader["nomeUnidadePagadora"].ToString();
            vo.SiglaUnidadePagadora = reader["siglaUnidadePagadora"].ToString();
            vo.UfUnidadePagadora = reader["ufUnidadePagadora"].ToString();
            //
            vo.CodigoUnidadeOrganizacional = reader["codigoUnidadeOrganizacional"].ToString();
            vo.NomeUnidadeOrganizacional = reader["nomeUnidadeOrganizacional"].ToString();
            vo.UfUnidadeOrganizacional = reader["ufUnidadeOrganizacional"].ToString();

            //
            vo.NomeInstituidorPensao = reader["nomeInstituidorPensao"].ToString();
            vo.CodigoInstituidorPensao = reader["codigoInstituidorPensao"].ToString();

            //
            vo.DataAposentadoria = MySqlModule.MyDateToDateTime(reader["DataAposentadoria"].ToString());
            vo.TipoAposentadoria = reader["tipoAposentadoria"].ToString();
            vo.FracaoAposentadoria = reader["fracaoAposentadoria"].ToString();

            //
            reader.Close();

            //
            vo.VoPerson = PersonModule.VoPessoaFromReader(conn, vo.IdPessoa);
            if ((vo.IdMembroPai ?? 0) != 0) vo.VoMembroPai = MemberFromReader(conn, vo.IdMembroPai);

            //
            return vo;
        }
    }
}