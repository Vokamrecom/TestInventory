using System.Collections.Concurrent;
using Test;

namespace Test.Tests;

public class InventoryThreadSafetyTests
{
    [Fact]
    public async Task AddItem_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var inventory = new Inventory();
        var tasks = new List<Task>();
        var exceptions = new ConcurrentBag<Exception>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            int index = i;
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var item = new Item($"Item{index}", 1);
                    inventory.AddItem(item);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.True(inventory.Items.Count <= 100);
        Assert.True(inventory.CurrentWeight <= 100);
    }

    [Fact]
    public async Task RemoveItem_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var inventory = new Inventory();
        var items = new List<Item>();
        
        // Добавляем предметы
        for (int i = 0; i < 50; i++)
        {
            var item = new Item($"Item{i}", 1);
            items.Add(item);
            inventory.AddItem(item);
        }

        var tasks = new List<Task>();
        var exceptions = new ConcurrentBag<Exception>();

        // Act
        foreach (var item in items)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    inventory.RemoveItem(item);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.Empty(inventory.Items);
    }

    [Fact]
    public async Task SearchItems_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var inventory = new Inventory();
        
        // Добавляем предметы
        for (int i = 0; i < 50; i++)
        {
            var item = new Item($"Sword{i}", 1);
            inventory.AddItem(item);
        }

        var tasks = new List<Task>();
        var exceptions = new ConcurrentBag<Exception>();
        var results = new ConcurrentBag<IEnumerable<Item>>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var searchResults = inventory.SearchItems("Sword");
                    results.Add(searchResults);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(100, results.Count);
        Assert.All(results, r => Assert.True(r.Count() == 50));
    }

    [Fact]
    public async Task CurrentWeight_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var inventory = new Inventory();
        var tasks = new List<Task>();
        var exceptions = new ConcurrentBag<Exception>();
        var weights = new ConcurrentBag<int>();

        // Добавляем предметы параллельно
        for (int i = 0; i < 50; i++)
        {
            int index = i;
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var item = new Item($"Item{index}", 1);
                    inventory.AddItem(item);
                    weights.Add(inventory.CurrentWeight);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.True(weights.All(w => w >= 0 && w <= 100));
        Assert.Equal(50, inventory.CurrentWeight);
    }

    [Fact]
    public async Task Items_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var inventory = new Inventory();
        var tasks = new List<Task>();
        var exceptions = new ConcurrentBag<Exception>();
        var itemLists = new ConcurrentBag<IReadOnlyList<Item>>();

        // Добавляем предметы
        for (int i = 0; i < 50; i++)
        {
            var item = new Item($"Item{i}", 1);
            inventory.AddItem(item);
        }

        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    itemLists.Add(inventory.Items);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(100, itemLists.Count);
        Assert.All(itemLists, list => Assert.Equal(50, list.Count));
    }
}

