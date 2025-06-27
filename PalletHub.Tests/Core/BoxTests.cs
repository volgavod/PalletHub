using PalletHub.Core.Models;

namespace PalletHub.Tests.Core;

[TestClass]
public class BoxTests 
{
	private readonly Size3D validSize = new(1.0, 1.0, 1.0);
	private readonly DateOnly validProductionDate = new(2025, 1, 1);

	[TestMethod]
	public void Constructor_ValidParameters_SetsPropertiesCorrectly()
	{
		var box = new Box(validSize, 10.0, validProductionDate);

		Assert.AreEqual(1.0, box.Height);
		Assert.AreEqual(1.0, box.Width);
		Assert.AreEqual(1.0, box.Depth);
		Assert.AreEqual(1.0, box.Volume);
		Assert.AreEqual(10.0, box.Weight);
		Assert.AreEqual(validProductionDate, box.ProductionDate);
		Assert.AreEqual(validProductionDate.AddDays(100), box.BestBeforeDate);
	}

	[TestMethod]
	public void Constructor_BestBeforeDate_SetsPropertiesCorrectly()
	{
		var bestBeforeDate = new DateOnly(2025, 6, 1);
		var box = new Box(validSize, 10.0, validProductionDate, bestBeforeDate);

		Assert.AreEqual(bestBeforeDate, box.BestBeforeDate);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Constructor_NegativeWeight_ThrowsArgumentException()
	{
		var box = new Box(validSize, -1.0, validProductionDate);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Constructor_SizeBelowMin_ThrowsArgumentException()
	{
		var invalidSize = new Size3D(0.01, 1.0, 1.0);
		var box = new Box(invalidSize, 10.0, validProductionDate);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Constructor_SizeAboveMax_ThrowsArgumentException()
	{
		var invalidSize = new Size3D(6.0, 1.0, 1.0);
		var box = new Box(invalidSize, 10.0, validProductionDate);
	}

	[TestMethod]
	public void Weight_SetValidValue_SetsCorrectly()
	{
		var box = new Box(validSize, 10.0, validProductionDate);
		box.Weight = 20.0;

		Assert.AreEqual(20.0, box.Weight);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Weight_SetNegativeValue_ThrowsArgumentException()
	{
		var box = new Box(validSize, 10.0, validProductionDate);
		box.Weight = -1.0;
	}

	[TestMethod]
	public void GetTwoSmallestSizes_DifferentSizes_ReturnsCorrectSizes()
	{
		var size = new Size3D(3.0, 1.0, 2.0);
		var box = new Box(size, 10.0, validProductionDate);

		var (small, medium) = box.GetTwoSmallestSizes();

		Assert.AreEqual(1.0, small);
		Assert.AreEqual(2.0, medium);
	}

	[TestMethod]
	public void GetTwoSmallestSizes_EqualSizes_ReturnsCorrectSizes()
	{
		var box = new Box(validSize, 10.0, validProductionDate);

		var (small, medium) = box.GetTwoSmallestSizes();

		Assert.AreEqual(1.0, small);
		Assert.AreEqual(1.0, medium);
	}

	[TestMethod]
	public void MinSize_ReturnsCorrectValues()
	{
		var minSize = Box.MinSize;

		Assert.AreEqual(0.05, minSize.Height);
		Assert.AreEqual(0.05, minSize.Width);
		Assert.AreEqual(0.05, minSize.Depth);
	}

	[TestMethod]
	public void MaxSize_ReturnsCorrectValues()
	{
		var maxSize = Box.MaxSize;

		Assert.AreEqual(5.0, maxSize.Height);
		Assert.AreEqual(5.0, maxSize.Width);
		Assert.AreEqual(5.0, maxSize.Depth);
	}
}
