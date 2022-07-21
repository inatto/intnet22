using System.Windows.Controls;

namespace intnet22.lib.general.wpf;

public class GeneralGrid : DataGrid
{
    public long? FirstId()
    {
        // clique no titulo, por exemplo
        if (SelectedCells.Count <= 0) return null;
        var id = ((Vo)SelectedCells[0].Item).GetId();

        //
        return id;
    }
}