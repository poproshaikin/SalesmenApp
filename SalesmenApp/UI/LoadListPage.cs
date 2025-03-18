using ConsolaUI;

namespace SalesmanBrowser;

public class LoadListPage : PageBase
{
    private string _listName = "";
    
    private SalesmenListsManager _salesmenListsManager => GetApp<SalesmenApp>().SalesmenListsManager;
    
    public LoadListPage(AppBase app) : base(app)
    {
    }

    protected override void Markup()
    {
        Header("Vyberte seznam");
        NewLine(); NewLine();
        
        foreach (var list in GetApp<SalesmenApp>().SalesmenListsManager.Lists)
        {
            Button(list.Name, () => LoadList(list));
            NewLine();
        }
        
        NewLine();
        Button("Return", Return);
    }

    private void LoadList(SalesmenList list)
    {
        _salesmenListsManager.CurrentList = list;
        ChangePage(new SalesmenListPage(GetApp()));
    }
    
    private void Return()
    {
        ChangePage(new SalesmenListPage(GetApp()));
    }

    private new SalesmenApp GetApp()
    {
        return base.GetApp<SalesmenApp>();
    }    
}