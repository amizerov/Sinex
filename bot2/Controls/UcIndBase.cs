namespace bot2.Controls;

public partial class UcIndBase : UserControl
{
    protected List<string> _indicaList = new();
    public virtual List<string> GetIndicators() { return new(); }

}
