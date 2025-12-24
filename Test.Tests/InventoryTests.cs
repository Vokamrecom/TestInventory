using Test;

namespace Test.Tests;

public class InventoryTests
{
    [Fact]
    public void AddItem_ValidItem_ShouldAddItem()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);

        // Act
        inventory.AddItem(item);

        // Assert
        Assert.Single(inventory.Items);
        Assert.Equal(item, inventory.Items[0]);
        Assert.Equal(10, inventory.CurrentWeight);
    }

    [Fact]
    public void AddItem_NullItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var inventory = new Inventory();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => inventory.AddItem(null!));
    }

    [Fact]
    public void AddItem_ExceedsMaxWeight_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("HeavyItem", 60);
        var item2 = new Item("AnotherHeavyItem", 50);

        // Act
        inventory.AddItem(item1);

        // Assert
        Assert.Throws<InvalidOperationException>(() => inventory.AddItem(item2));
    }

    [Fact]
    public void AddItem_DuplicateItem_ShouldSumWeights()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("Sword", 10);
        var item2 = new Item("Sword", 15);

        // Act
        inventory.AddItem(item1);
        inventory.AddItem(item2);

        // Assert
        Assert.Single(inventory.Items);
        Assert.Equal(25, inventory.Items[0].Weight);
        Assert.Equal(25, inventory.CurrentWeight);
    }

    [Fact]
    public void AddItem_DuplicateItemCaseInsensitive_ShouldSumWeights()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("Sword", 10);
        var item2 = new Item("SWORD", 15);

        // Act
        inventory.AddItem(item1);
        inventory.AddItem(item2);

        // Assert
        Assert.Single(inventory.Items);
        Assert.Equal(25, inventory.Items[0].Weight);
    }

    [Fact]
    public void AddItem_DuplicateItemExceedsMaxWeight_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("Sword", 60);
        var item2 = new Item("Sword", 50);

        // Act
        inventory.AddItem(item1);

        // Assert
        Assert.Throws<InvalidOperationException>(() => inventory.AddItem(item2));
    }

    [Fact]
    public void RemoveItem_ExistingItem_ShouldRemoveItem()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);
        inventory.AddItem(item);

        // Act
        var result = inventory.RemoveItem(item);

        // Assert
        Assert.True(result);
        Assert.Empty(inventory.Items);
        Assert.Equal(0, inventory.CurrentWeight);
    }

    [Fact]
    public void RemoveItem_NonExistingItem_ShouldReturnFalse()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);

        // Act
        var result = inventory.RemoveItem(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveItem_NullItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var inventory = new Inventory();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => inventory.RemoveItem(null!));
    }

    [Fact]
    public void RemoveItemByName_ExistingItem_ShouldRemoveItem()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);
        inventory.AddItem(item);

        // Act
        var result = inventory.RemoveItemByName("Sword");

        // Assert
        Assert.True(result);
        Assert.Empty(inventory.Items);
    }

    [Fact]
    public void SearchItems_Substring_ShouldReturnMatchingItems()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("Iron Sword", 10);
        var item2 = new Item("Steel Sword", 15);
        var item3 = new Item("Shield", 20);
        inventory.AddItem(item1);
        inventory.AddItem(item2);
        inventory.AddItem(item3);

        // Act
        var results = inventory.SearchItems("Sword").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, item => Assert.Contains("Sword", item.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchItems_CaseInsensitive_ShouldReturnMatchingItems()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);
        inventory.AddItem(item);

        // Act
        var results = inventory.SearchItems("sword").ToList();

        // Assert
        Assert.Single(results);
        Assert.Equal("Sword", results[0].Name);
    }

    [Fact]
    public void SearchItems_NoMatches_ShouldReturnEmpty()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);
        inventory.AddItem(item);

        // Act
        var results = inventory.SearchItems("Shield").ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void SearchItems_EmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var inventory = new Inventory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => inventory.SearchItems(""));
    }

    [Fact]
    public void CurrentWeight_MultipleItems_ShouldReturnSum()
    {
        // Arrange
        var inventory = new Inventory();
        var item1 = new Item("Sword", 10);
        var item2 = new Item("Shield", 20);
        var item3 = new Item("Potion", 5);

        // Act
        inventory.AddItem(item1);
        inventory.AddItem(item2);
        inventory.AddItem(item3);

        // Assert
        Assert.Equal(35, inventory.CurrentWeight);
    }

    [Fact]
    public void MaxWeightLimit_ShouldReturn100()
    {
        // Arrange
        var inventory = new Inventory();

        // Assert
        Assert.Equal(100, inventory.MaxWeightLimit);
    }

    [Fact]
    public void Items_ShouldReturnReadOnlyCopy()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("Sword", 10);
        inventory.AddItem(item);

        // Act
        var items = inventory.Items;
        var items2 = inventory.Items;

        // Assert
        Assert.NotSame(items, items2); // Должны быть разные экземпляры
        Assert.Equal(item, items[0]);
    }

    [Fact]
    public void AddItem_AtMaxWeight_ShouldSucceed()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new Item("HeavyItem", 100);

        // Act
        inventory.AddItem(item);

        // Assert
        Assert.Single(inventory.Items);
        Assert.Equal(100, inventory.CurrentWeight);
    }
}

