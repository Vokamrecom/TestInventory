using Test;

namespace Test.Tests;

public class ItemTests
{
    [Fact]
    public void Constructor_ValidParameters_ShouldCreateItem()
    {
        // Arrange & Act
        var item = new Item("Sword", 10);

        // Assert
        Assert.Equal("Sword", item.Name);
        Assert.Equal(10, item.Weight);
    }

    [Fact]
    public void Constructor_NullName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Item(null!, 10));
    }

    [Fact]
    public void Constructor_EmptyName_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Item("", 10));
    }

    [Fact]
    public void Constructor_WhitespaceName_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Item("   ", 10));
    }

    [Fact]
    public void Constructor_ZeroWeight_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Item("Sword", 0));
    }

    [Fact]
    public void Constructor_NegativeWeight_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Item("Sword", -5));
    }

}

