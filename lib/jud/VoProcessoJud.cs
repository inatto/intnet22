// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using intnet22.lib.general;

namespace intnet22.lib.jud
{
    public class VoProcessoJud : Vo
    {
        //
        public override long? GetId()
        {
            return IdProcessoJud;
        }

        //
        public long IdProcessoJud { get; set; }
        public string? NrProcessoExecucao { get; set; }
        public string? TempVaraJud { get; set; }
        public DateTime? DataAjuizamento { get; set; }
        public string? MetaTitle { get; set; }
    }
}