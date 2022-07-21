// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace intnet22.lib.person
{
    public class VoPerson
    {
        //
        public int IdPessoa { get; set; }

        //
        public int? IdEnderecoResidencial { get; set; }
        public VoAddress? VoEnderecoResidencial { get; set; }

        //
        public int? IdEnderecoComercial { get; set; }
        public VoAddress? VoEnderecoComercial { get; set; }

        //
        public string? TagEstadoCivil { get; set; }
        public string? TagSexo { get; set; }
        public bool? EhFalecida { get; set; }

        //
        public bool TemVinculoAtivo { get; set; }
        public bool PodeEnviarEmailSenha { get; set; }
        public bool FlagNaoEnviarEmail { get; set; }

        //
        public string? Cpf { get; set; }
        public string? Nome { get; set; }
        public string? Fantasia { get; set; }
        public string? EmailPrincipal { get; set; }

        //
        public DateTime? DataNascimento { get; set; }
        public DateTime? DataObito { get; set; }

        //
        public string? CelularPrincipal { get; set; }
        public string? CelularComercial { get; set; }
        public string? TelefoneResidencial { get; set; }
        public string? TelefoneComercial { get; set; }
        public string? TelefoneComercial2 { get; set; }
        public string? TelefoneExtra { get; set; }

        //
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        public string? NomeConjuge { get; set; }
        public string? NomePai { get; set; }
        public string? NomeMae { get; set; }

        //
        public string? RgNr { get; set; }
        public string? RgOrgao { get; set; }
        public string? RgUf { get; set; }
        public DateTime? RgEmissao { get; set; }
        public string? OabNr { get; set; }

        //
        public string? FacebookUri { get; set; }
        public string? InstagramUri { get; set; }
        public string? TwitterUri { get; set; }
        public string? SiteUri { get; set; }

        //
        public string? PersonObs { get; set; }

    }
}