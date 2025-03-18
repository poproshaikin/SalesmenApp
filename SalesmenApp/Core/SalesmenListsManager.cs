using System.Numerics;

namespace SalesmanBrowser;

internal class SalesmenListsManager
{
    private const string directory_path = "lists";
    
    private readonly Salesman _root;
    
    private readonly List<SalesmenList> _lists;
    
    internal IReadOnlyCollection<SalesmenList> Lists => _lists;

    internal SalesmenList? CurrentList { get; set; }
    
    internal SalesmenListsManager(Salesman root)
    {
        _root = root;
        _lists = [];
        Load();
        CurrentList = null;
    }

    internal SalesmenList OpenList(string name)
    {
        return new SalesmenList(name, _root);
    }

    public SalesmenList CreateList(string listName)
    {
        var list = new SalesmenList(listName, _root);
        _lists.Add(list);
        return list;
    }

    public void Save()
    {
        foreach (var list in _lists)
            list.Save();
    }

    private void Load()
    {
        foreach (var file in Directory.EnumerateFiles(directory_path))
        {
            var list = new SalesmenList(Path.GetFileName(file), _root);
            _lists.Add(list);
        }
    }
}