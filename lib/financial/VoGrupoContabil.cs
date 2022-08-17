
namespace intnet22.lib.financial
{

    //
    public class VoGrupoContabil
    {

        public long Id  { get; set; }

        //
        public string? Nome { get; set; }
        public bool? EhReceita { get; set; }
        public bool? EhDespesa  { get; set; }

        //
        public string? _EhDespesa  { get; set; }
        public string? _EhReceita  { get; set; }

        public override string ToString()
        {
            return Nome ?? "";
        }

    }

}