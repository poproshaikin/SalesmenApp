using System.Collections;
using System.Collections.Immutable;
using System.Text.Json;

namespace SalesmanBrowser;

public class Salesman
{
    static int NextId = 1;

#region Moje logika 

    public int Id => ID;
    
    public int ParentId { get; private set; }
    
    public Salesman? FindSubordinateById(int id)
    {
        if (Id == id)
        {
            return this;
        }

        foreach (var subordinate in Subordinates)
        {
            Salesman? result = subordinate.FindSubordinateById(id);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
    
    public int GetTotalSales()
    {
        return Sales + Subordinates.Sum(subordinate => subordinate.GetTotalSales());
    }

    public IReadOnlyCollection<Salesman> ToReadOnlyCollection()
    {
        List<Salesman> result = [this];
        
        foreach (var subordinate in Subordinates)
        {
            result.AddRange(subordinate.ToReadOnlyCollection());
        }

        return result.ToImmutableArray();
    }

#endregion 
    
    private int ID { get; set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int Sales { get; private set; }
    public List<Salesman> Subordinates { get; private set; }

    public Salesman(string surname, string name, int sales, int id = 0)
    {
        Name = name;
        Surname = surname;
        Sales = sales;
        Subordinates = new List<Salesman>();

        if (id != 0)
        {
            ID = id;
            if (id > NextId)
                NextId = id + 1;
        }
        else
        {
            ID = NextId++;
        }
    }
    
    public Salesman GetSubordinate(int id)
    {
        if (Id == id)
            return this;
        
        foreach (var subordinate in Subordinates)
        {
            if (subordinate.ID == id)
            {
                return subordinate;
            }
            return subordinate.GetSubordinate(id);
        }
        
        throw new ArgumentException("No such subordinate");
    }

    public void AddSubordinate(Salesman subordinate)
    {
        Subordinates.Add(subordinate);
    }

    //kód níže slouží jen pro naèítání ze souboru

    public static Salesman DeserializeTree(string jsonString)
    {
        List<SalesmanData> deserializedData = JsonSerializer.Deserialize<List<SalesmanData>>(jsonString);

        Dictionary<int, Salesman> treeData = new Dictionary<int, Salesman>();
        Salesman root = null;

        foreach (var item in deserializedData)
        {
            Salesman salesman = item.ToSalesman();
            treeData[salesman.ID] = salesman;

            if (item.ParentId != 0)
                treeData[item.ParentId].AddSubordinate(salesman);
            else
                root = salesman;
        }

        return root;
    }

    private class SalesmanData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Sales { get; set; }
        public int ParentId { get; set; }

        public SalesmanData() { }

        public SalesmanData(Salesman salesman, int parentId = 0)
        {
            ID = salesman.ID;
            Name = salesman.Name;
            Surname = salesman.Surname;
            Sales = salesman.Sales;
            ParentId = parentId;
        }

        public Salesman ToSalesman() => new Salesman(Surname, Name, Sales, ID)
        {
            ParentId = ParentId
        };
    }
}