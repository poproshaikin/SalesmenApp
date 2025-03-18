using SalesmanBrowser;

class Program
{
    static void Main(string[] args)
    {
        string filename = "largetree.json";
        Salesman boss = Salesman.DeserializeTree(File.ReadAllText(filename));

        //DisplaySalesmenTree(boss);
        
        SalesmenApp app = new SalesmenApp(boss);
        app.Run();
    }
    
    static void DisplaySalesmenTree(Salesman node, string indent = "")
    {
        Console.WriteLine($"{indent}{node.Id} {node.Name} {node.Surname} - Sales: {node.Sales}");

        foreach (var subordinate in node.Subordinates)
        {
            DisplaySalesmenTree(subordinate, indent + "    ");
        }
    }
}