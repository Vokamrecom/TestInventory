namespace Test;

public class Item
{
    public string Name { get; }
    public int Weight { get; }

    public Item(string name, int weight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (weight <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weight));
        
        Name = name;
        Weight = weight;
    }
}
