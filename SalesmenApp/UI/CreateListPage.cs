using ConsolaUI;

namespace SalesmanBrowser;

public class CreateListPage : PageBase
{
    private string? _listName = "";

    private SalesmenListsManager _salesmenListsManager => GetApp<SalesmenApp>().SalesmenListsManager;
    
    public CreateListPage(AppBase app) : base(app)
    {
    }
    
    protected override void Markup()
    {
        Header("Zalozit seznam");
        NewLine();
        
        if (string.IsNullOrEmpty(_listName))
        {
            InputField("Jmeno seznamu: ", input =>
            {
                _listName = input;
                Rerender();
            });
            NewLine();
        }
        else
        {
            Text($"Jmeno seznamu: {_listName}");
            NewLine();
            Button("Vytvorit", CreateList, id: "create-list");
        }
    }

    private void CreateList()
    {
        _salesmenListsManager.CreateList(_listName!);
        _salesmenListsManager.Save();

        NewLine();
        RemoveElement("create-list");
        Text("Seznam byl vytvoren uspesne.");
        NewLine();
        Rerender();
        
        ChangePage(new SalesmenListPage(GetApp()));
    }
}