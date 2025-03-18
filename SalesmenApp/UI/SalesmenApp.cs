using ConsolaUI;

namespace SalesmanBrowser;

internal class SalesmenApp : AppBase
{
    internal Salesman Root { get; }
    internal SalesmenListsManager SalesmenListsManager { get; }
    
    internal SalesmenApp(Salesman root)
    {
        Root = root;
        SalesmenListsManager = new SalesmenListsManager(Root);
    }
    
    protected override PageBase GetInitialPage()
    {
        return new SalesmanPage(Root, this);
    }
}