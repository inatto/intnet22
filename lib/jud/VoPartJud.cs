// ReSharper disable UnusedAutoPropertyAccessor.Global

using intnet22.lib.general;
using intnet22.lib.member;

namespace intnet22.lib.jud
{
    public class VoPartJud : Vo
    {
        //
        public override long? GetId()
        {
            return IdPartJud;
        }

        //
        public long IdPartJud { get; set; }
        public long IdMembro { get; set; }
        public VoMember? VoMember { get; set; }

        //
        public string? Ref1993 { get; set; }
        public string? CorrespondenciaEnviada { get; set; }
        public string? TermoAcordoRecebido { get; set; }
        public string? CalculoHexagonRecebido { get; set; }
        public string? AvisoRecebimento { get; set; }
        public string? TermoAssinadoRecebido { get; set; }
        public string? EmailEnviado { get; set; }
        public string? PercentualAplicado { get; set; }
        public string? EnviadoAoMotaParaContato { get; set; }
        public string? AlegacaoLitispendencia { get; set; }
        public string? PediramDesistencia { get; set; }

        //
        public string? NumeroExecucao { get; set; }
        public string? NumeroEmbargos { get; set; }


    }
}