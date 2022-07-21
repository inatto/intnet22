// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using intnet22.lib.general;
using intnet22.lib.person;

namespace intnet22.lib.member
{
    public class VoMember : Vo
    {
        //
        public override long? GetId()
        {
            return IdMembro;
        }

        //
        public bool Active { get; set; }
        public string? ActiveAlt { get; set; }
        public long IdMembro { get; set; }

        //
        public long? IdMembroPai { get; set; }
        public VoMember? VoMembroPai { get; set; }

        //
        public long IdPessoa { get; set; }
        public VoPerson VoPerson { get; set; } = null!;

        //
        public long IdPessoaOwner { get; set; }

        //
        public string? ComoEnviarJornal { get; set; }

        // Dados Funcionais
        public long? IdCarreiraServidor { get; set; }
        public long? IdStatusAssociativo { get; set; }
        public string? TagStatusCarreira { get; set; }
        public string? SituacaoMensalidade { get; set; }

        //
        public string? MatriculaOrgao { get; set; }
        public string? NrContrato { get; set; }
        public float? ValorMensalidade { get; set; }

        //
        public DateTime? DataIngresso { get; set; }
        public DateTime? DataFiliacao { get; set; }
        public DateTime? DataDesfiliacao { get; set; }

        //
        public string? CodigoOrgaoPublico { get; set; }
        public string? OrgaoPublico { get; set; }
        public string? SiglaOrgaoPublico { get; set; }

        //
        public string? CodigoUnidadePagadora { get; set; }
        public string? NomeUnidadePagadora { get; set; }
        public string? SiglaUnidadePagadora { get; set; }
        public string? UfUnidadePagadora { get; set; }

        //
        public string? CodigoUnidadeOrganizacional { get; set; }
        public string? NomeUnidadeOrganizacional { get; set; }
        public string? UfUnidadeOrganizacional { get; set; }

        //
        public string? CodigoInstituidorPensao { get; set; }
        public string? NomeInstituidorPensao { get; set; }

        //
        public DateTime? DataAposentadoria { get; set; }
        public string? TipoAposentadoria { get; set; }
        public string? FracaoAposentadoria { get; set; }
    }
}