using ConsolaUI;

namespace SalesmanBrowser;

internal class SalesmenListPage : PageBase
{
    private SalesmenListsManager _salesmenListsManager;

    internal SalesmenListPage(AppBase app) : base(app)
    {
        _salesmenListsManager = ((SalesmenApp)app).SalesmenListsManager;
    }

    protected override void Markup()
    {
        TopBar();
    }

    private void TopBar()
    {
        Margin();
        
        Button("Zalozit", GoToCreateListPage);
        
        Margin();
        VerticalLine();
        Margin();
        
        Button("Nacist", GoToLoadListPage);
        
        Margin();
        VerticalLine();
        Margin();
        
        Button("Ulozit", SaveState);
        
        Margin();
        VerticalLine();
        Margin();

        Button("Prejit na prohlizec", GoToRootSalesman);
        NewLine();
        
        HorizontalLine(48);
        NewLine();

        if (_salesmenListsManager.CurrentList?.Name is null)
            return;

        Text($"Seznam: {_salesmenListsManager.CurrentList.Name}");
        NewLine();
            
        HorizontalLine(20);
        NewLine();
        NewLine();

        foreach (var salesman in _salesmenListsManager.CurrentList.Get())
        {
            Button($"{salesman.Surname}, {salesman.Name}", onClick: () => ChangePage(new SalesmanPage(salesman, GetApp())));
            NewLine();
        }
    }
    
    private void GoToCreateListPage()
    {
        ChangePage(new CreateListPage(GetApp()));
    }

    private void GoToLoadListPage()
    {
        ChangePage(new LoadListPage(GetApp()));
    }

    private void SaveState()
    {
        GetApp<SalesmenApp>().SalesmenListsManager.Save();
    }

    private void GoToRootSalesman()
    {
        ChangePage(new SalesmanPage(GetApp<SalesmenApp>().Root, GetApp()));
    }
}