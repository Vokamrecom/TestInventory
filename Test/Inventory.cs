namespace Test;

public class Inventory
{
    private const int MaxWeight = 100;
    private readonly List<Item> _items = [];
    private readonly object _lock = new object();

    public IReadOnlyList<Item> Items
    {
        get
        {
            lock (_lock)
            {
                return _items.ToList().AsReadOnly();
            }
        }
    }

    public int CurrentWeight
    {
        get
        {
            lock (_lock)
            {
                return _items.Sum(item => item.Weight);
            }
        }
    }

    public int MaxWeightLimit => MaxWeight;

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock)
        {
            var existingItem = _items.FirstOrDefault(i => 
                string.Equals(i.Name, item.Name, StringComparison.OrdinalIgnoreCase));

            if (existingItem != null)
            {
                var newWeight = existingItem.Weight + item.Weight;
                if (CurrentWeight - existingItem.Weight + newWeight > MaxWeight)
                {
                    throw new InvalidOperationException(
                        $"Cannot add item: total weight would exceed maximum weight of {MaxWeight}");
                }

                _items.Remove(existingItem);
                _items.Add(new Item(existingItem.Name, newWeight));
            }
            else
            {
                if (CurrentWeight + item.Weight > MaxWeight)
                {
                    throw new InvalidOperationException(
                        $"Cannot add item: total weight would exceed maximum weight of {MaxWeight}");
                }

                _items.Add(item);
            }
        }
    }

    public bool RemoveItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock)
        {
            return _items.Remove(item);
        }
    }

    public bool RemoveItemByName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        lock (_lock)
        {
            var item = _items.FirstOrDefault(i => 
                string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
            
            if (item != null)
            {
                return _items.Remove(item);
            }

            return false;
        }
    }

    public IEnumerable<Item> SearchItems(string searchTerm)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchTerm);

        lock (_lock)
        {
            return _items
                .Where(item => item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
