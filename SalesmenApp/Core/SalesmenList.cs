using System.Security;

namespace SalesmanBrowser;

internal class SalesmenList
{
    private const string directory_path = "lists";

    private readonly string _filePath;

    private List<int> _list;

    private bool _needsSaving;

    private readonly Salesman _root;
    private bool _directoryExists => Directory.Exists(directory_path);

    private bool _fileExists => File.Exists(_filePath);
    
    
    internal bool NeedsSaving => _needsSaving;
    
    internal string Name { get; set; }

    internal SalesmenList(string name, Salesman root)
    {
        Name = name;
        _list = [];
        _needsSaving = true;
        _root = root;
        _filePath = $"{directory_path}/{Name}";
        
        if (!_directoryExists)
            InitDirectory();

        if (_fileExists)
        {
            Load();
        }
    }

    internal bool Add(Salesman salesman)
    {
        int id = salesman.Id;
        if (_list.Contains(id))
        {
            _needsSaving = false;
            return false;
        }
        else
        {
            _list.Add(id);
            _needsSaving = true;
            return true;
        }
    }

    internal bool Remove(Salesman salesman)
    {
        int id = salesman.Id;

        if (!_list.Contains(id))
        {
            _needsSaving = false;
            return false;
        }
        else
        {
            _list.Remove(id);
            _needsSaving = true;
            return true;
        }
    }

    internal IReadOnlyCollection<Salesman> Get()
    {
        Salesman[] salesmen = new Salesman[_list.Count];
        
        for (int i = 0; i < _list.Count; i++)
        {
            salesmen[i] = _root.GetSubordinate(_list[i]);
        }

        return salesmen;
    }

    private void Load()
    {
        string[] content = File.ReadAllLines(_filePath);
        _list = Deserialize(content);
    }
    
    internal void Save()
    {
        File.WriteAllText(_filePath, Serialize());
        _needsSaving = false;
    }

    internal bool Contains(Salesman salesman)
    {
        return _list.Contains(salesman.Id);
    }

    private string Serialize() => string.Join('\n', _list.Select(id => id.ToString()));

    private List<int> Deserialize(string[] content) => content.Select(int.Parse).ToList();

    private void InitDirectory()
    {
        Directory.CreateDirectory($"{directory_path}");
    }
}   