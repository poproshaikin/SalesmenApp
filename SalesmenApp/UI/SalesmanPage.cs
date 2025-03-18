using ConsolaUI;

namespace SalesmanBrowser;

public class SalesmanPage : PageBase
{
    private Salesman _salesman;

    private SalesmenListsManager _salesmenListsManager => GetApp<SalesmenApp>().SalesmenListsManager;
    
    public SalesmanPage(Salesman salesman, AppBase app) : base(app)
    {
        _salesman = salesman;
    }

    protected override void Markup()
    {
        if (_salesman.ParentId != 0)
        {
            Button("Prejit nahoru", onClick: GoUp);
            Margin(1);
            VerticalLine();
            Margin(1);
        }
        Button("Prejit na seznam", GoToSalesmanList);
        NewLine(); HorizontalLine(length: 32); NewLine(); 
        NewLine();
        Text($"Obchodnik: {_salesman.Name} {_salesman.Surname}");

        if (_salesmenListsManager.CurrentList is not null)
        {
            if (_salesmenListsManager.CurrentList.Contains(_salesman))
            {
                Margin(9);
                Button("Odebrat", onClick: RemoteSalesmanFromList);
            }
            else
            {
                Margin(9);
                Button("Pridat", onClick: AddSalesmanToList);
            }
        }
        
        NewLine(); HorizontalLine(length: 32); NewLine();
        Text($"Prime prodeje: {_salesman.Sales}$");
        NewLine(); 
        Text($"Celkove prodeje site: {_salesman.GetTotalSales()}$");
        NewLine(); NewLine();

        if (_salesman.ParentId != 0)
        {
            Text($"Nadrizeny: {_salesman.Name} {_salesman.Surname}");
            NewLine(); NewLine();
        }
        
        if (_salesman.Subordinates.Count > 0)
        {
            Text("Podrizeni: ");
            NewLine();
            foreach (var subordinate in _salesman.Subordinates)
            {
                Margin(4);
                Button($"{subordinate.Name} {subordinate.Surname}", () => ChangeSalesman(subordinate));
                NewLine();
            }
        }
    }

    private void ChangeSalesman(Salesman salesman) 
    {
        ChangePage(new SalesmanPage(salesman, GetApp()));
    }

    private void GoUp()
    {
        Salesman boss = GetApp<SalesmenApp>().Root;
        Salesman parentOfCurrent = boss.FindSubordinateById(_salesman.ParentId)!;
        ChangePage(new SalesmanPage(parentOfCurrent, GetApp()));
    }

    private void GoToSalesmanList()
    {
        ChangePage(new SalesmenListPage(GetApp<SalesmenApp>()));
    }

    private void AddSalesmanToList()
    {
        if (_salesmenListsManager.CurrentList is null)
            return;

        _salesmenListsManager.CurrentList.Add(_salesman);
        
        ChangePage(new SalesmenListPage(GetApp()));
    }

    private void RemoteSalesmanFromList()
    {
        if (_salesmenListsManager.CurrentList is null)
            return;
        
        _salesmenListsManager.CurrentList.Remove(_salesman);
        
        ChangePage(new SalesmenListPage(GetApp()));
    }
}