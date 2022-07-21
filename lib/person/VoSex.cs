namespace intnet22.lib.person
{
    //
    public class VoSex
    {
        public string? TagSexo { get; set; }

        public string? NomeSexo { get; set; }

        public override string ToString()
        {
            return NomeSexo ?? "";
        }
    }
}