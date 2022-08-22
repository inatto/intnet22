﻿// ReSharper disable UnusedAutoPropertyAccessor.Global

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
        public string? NrProcessoOriginario { get; set; }
        public DateTime? DataAjuizamento { get; set; }
        public DateTime? DataTransitoJulgado { get; set; }
        public string? TempVaraJud { get; set; }
        public string? TempAutorJud { get; set; }
        public string? TempEscritorioAdv { get; set; }
        public string? ObjetoJud { get; set; }

        //
        public string? MetaTitle { get; set; }
    }
}