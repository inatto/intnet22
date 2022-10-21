using System;
using intnet22.lib.general;
using MySql.Data.MySqlClient;

// ReSharper disable once UseObjectOrCollectionInitializer
// ReSharper disable ConvertIfStatementToSwitchExpression
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace intnet22.lib.person
{
    public static class PersonModule
    {
        public static VoPerson VoPessoaFromReader(MySqlConnection? conn, long? idPessoa = null)
        {
            //
            if (conn is null) throw new Exception("Null Connection");

            //
            var command = new MySqlCommand($"select * from pessoa where id_pessoa = " + idPessoa, conn);
            var reader = command.ExecuteReader();

            //
            reader.Read();

            //
            var vo = new VoPerson();
            // aba dados pessoais
            vo.IdPessoa = (int)(uint)reader["id_pessoa"];
            vo.IdEnderecoResidencial = MySqlModule.ToInt(reader["id_endereco_residencial"]);
            vo.IdEnderecoComercial = MySqlModule.ToInt(reader["id_endereco_comercial"]);

            //
            vo.TagEstadoCivil = reader["tag_estadoCivil"].ToString();
            vo.TagSexo = reader["tag_sexo"].ToString();
            vo.EhFalecida = MySqlModule.FromStringToBool(reader["ehFalecida"].ToString());
            vo.Cpf = reader["cpf"].ToString();
            vo.Nome = reader["nome"].ToString();
            vo.Fantasia = reader["fantasia"].ToString();
            vo.EmailPrincipal = reader["emailPrincipal"].ToString();
            vo.EmailComercial = reader["emailComercial"].ToString();
            vo.DataNascimento = MySqlModule.MyDateToDateTime(reader["dataNascimento"].ToString());
            vo.DataObito = MySqlModule.MyDateToDateTime(reader["dataObito"].ToString());
            vo.CelularPrincipal = MySqlModule.PhoneStringCheck(reader["celularPrincipal"].ToString());
            vo.CelularPrincipal2 = MySqlModule.PhoneStringCheck(reader["celularPrincipal2"].ToString());
            vo.CelularComercial = MySqlModule.PhoneStringCheck(reader["celularComercial"].ToString());
            vo.TelefoneResidencial = MySqlModule.PhoneStringCheck(reader["telefoneResidencial"].ToString());
            vo.TelefoneComercial = MySqlModule.PhoneStringCheck(reader["telefoneComercial"].ToString());
            vo.TelefoneComercial2 = MySqlModule.PhoneStringCheck(reader["telefoneComercial2"].ToString());
            vo.TelefoneExtra = MySqlModule.PhoneStringCheck(reader["telefoneExtra"].ToString());

            // aba vinculo
            // vo.PodeEnviarEmailSenha = MySqlModule.FromStringToBool(reader["active"].ToString());
            vo.FlagNaoEnviarEmail = MySqlModule.FromStringToBool(reader["flagNaoEnviarEmail"].ToString());

            //
            vo.Naturalidade = reader["naturalidade"].ToString();
            vo.Nacionalidade = reader["nacionalidade"].ToString();
            vo.NomeConjuge = reader["nomeConjugue"].ToString();
            vo.NomePai = reader["nomePai"].ToString();
            vo.NomeMae = reader["nomeMae"].ToString();

            //
            vo.RgNr = reader["rg"].ToString();
            vo.RgOrgao = reader["rgOrgao"].ToString();
            vo.RgUf = reader["rgUf"].ToString();
            vo.RgEmissao = MySqlModule.MyDateToDateTime(reader["rgData"].ToString());
            vo.OabNr = reader["documentoOAB"].ToString();

            //
            vo.FacebookUri = reader["facebook"].ToString();
            vo.InstagramUri = reader["instagram"].ToString();
            vo.TwitterUri = reader["twitter"].ToString();
            vo.SiteUri = reader["site"].ToString();

            //
            vo.PersonObs = reader["obs"].ToString();

            //
            reader.Close();

            if ((vo.IdEnderecoResidencial ?? 0) != 0) vo.VoEnderecoResidencial = VoAddressFromReader(conn, vo.IdEnderecoResidencial);
            if ((vo.IdEnderecoComercial ?? 0) != 0) vo.VoEnderecoComercial = VoAddressFromReader(conn, vo.IdEnderecoComercial);

            //
            return vo;
        }

        private static VoAddress VoAddressFromReader(MySqlConnection? conn, int? id)
        {
            //
            if (conn is null) throw new Exception("Null Connection");

            //
            var command = new MySqlCommand($"select * from endereco where id_endereco = " + id, conn);
            var reader = command.ExecuteReader();
            reader.Read();

            //
            var vo = new VoAddress();
            // aba dados pessoais
            vo.IdEndereco = (int)(uint)reader["id_endereco"];
            vo.TagUf = reader["tag_uf"].ToString();
            vo.Cep = reader["cep"].ToString();
            vo.Cidade = reader["cidade"].ToString();
            vo.Bairro = reader["bairro"].ToString();
            vo.Logradouro = reader["logradouro"].ToString();
            vo.Numero = reader["numero"].ToString();
            vo.Complemento = reader["complemento"].ToString();

            //
            reader.Close();

            //
            return vo;
        }
    }
}