//

namespace intnet22.lib.general

{
    public class ItemCombo
    {
        public string? valor { get; set; }

        public string? texto { get; set; }

        public override string ToString()
        {
            return texto ?? "";
        }

    }
}