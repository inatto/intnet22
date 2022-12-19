
using System;

namespace intnet22.lib.financial
{

    //
    public class VoLancamento
    {

        public long IdLancamento { get; set; }

        //
        public long? IdContaFinanceira { get; set; }
        public long? IdGrupoContabil { get; set; }
        public long? IdTipoLancamento { get; set; }

        //
        public long? Conciliado { get; set; }

        //
        public string? Descricao { get; set; }
        public string? MetaTitle { get; set; }
        public DateTime? DataBaixa{ get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? MesAnoReferencia { get; set; }

        public float? ValorBruto { get; set; }
        public float? ValorLiquido { get; set; }

        //
        public string? OutNomeGrupo{ get; set; }
        public string? OutNomeConta{ get; set; }
        public string? OutDataBaixa{ get; set; }
        public string? OutDataVencimento { get; set; }

        public override string ToString()
        {
            return Descricao ?? "";
        }

    }

}