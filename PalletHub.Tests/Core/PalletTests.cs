using PalletHub.Core.Models;

namespace PalletHub.Tests.Core;

[TestClass]
public class PalletTests
{
	private readonly Size3D validPalletSize = new(0.15, 1.0, 1.0);
	private readonly Size3D validBoxSize = new(0.5, 0.5, 0.5);
	private readonly DateOnly validProductionDate = new(2025, 1, 1);

	[TestMethod]
	public void Constructor_ValidSize_SetsPropertiesCorrectly()
	{
		var pallet = new Pallet(validPalletSize);

		Assert.AreEqual(0.15, pallet.Height);
		Assert.AreEqual(1.0, pallet.Width);
		Assert.AreEqual(1.0, pallet.Depth);
		Assert.AreEqual(30.0, pallet.GeneralWeight);
		Assert.AreEqual(0.15, pallet.GeneralVolume);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Constructor_SizeBelowMin_ThrowsArgumentException()
	{
		var invalidSize = new Size3D(0.05, 1.0, 1.0);
		var pallet = new Pallet(invalidSize);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Constructor_SizeAboveMax_ThrowsArgumentException()
	{
		var invalidSize = new Size3D(0.3, 1.0, 1.0);
		var pallet = new Pallet(invalidSize);
	}

	[TestMethod]
	public void AddBox_ValidBox_BoxAddedSuccessfully()
	{
		var pallet = new Pallet(validPalletSize);
		var box = new Box(validBoxSize, 10.0, validProductionDate);

		pallet.AddBox(box);

		Assert.AreEqual(1, pallet.Count());
		Assert.AreEqual(40.0, pallet.GeneralWeight);
		Assert.AreEqual(0.15 + 0.125, pallet.GeneralVolume);
		Assert.AreEqual(validProductionDate.AddDays(100), pallet.BestBeforeDate);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void AddBox_BoxTooLarge_ThrowsException()
	{
		var pallet = new Pallet(validPalletSize);
		var largeBox = new Box(new Size3D(1.0, 2.0, 2.0), 10.0, validProductionDate);

		pallet.AddBox(largeBox);
	}

	[TestMethod]
	public void RemoveBox_ExistingBox_ReturnsTrueAndRemovesBox()
	{
		var pallet = new Pallet(validPalletSize);
		var box = new Box(validBoxSize, 10.0, validProductionDate);

		pallet.AddBox(box);
		var result = pallet.RemoveBox(box);

		Assert.IsTrue(result);
		Assert.AreEqual(0, pallet.Count());
		Assert.AreEqual(30.0, pallet.GeneralWeight);
		Assert.AreEqual(0.15, pallet.GeneralVolume);
	}

	[TestMethod]
	public void RemoveBox_NonExistingBox_ReturnsFalse()
	{
		var pallet = new Pallet(validPalletSize);
		var box = new Box(validBoxSize, 10.0, validProductionDate);

		var result = pallet.RemoveBox(box);

		Assert.IsFalse(result);
	}

	[TestMethod]
	public void BestBeforeDate_MultipleBoxes_ReturnsEarliestDate()
	{
		var pallet = new Pallet(validPalletSize);
		var box1 = new Box(validBoxSize, 10.0, validProductionDate, new DateOnly(2025, 6, 1));
		var box2 = new Box(validBoxSize, 10.0, validProductionDate, new DateOnly(2025, 5, 1));
		pallet.AddBox(box1);
		pallet.AddBox(box2);

		var bestBeforeDate = pallet.BestBeforeDate;

		Assert.AreEqual(new DateOnly(2025, 5, 1), bestBeforeDate);
	}

	[TestMethod]
	public void OperatorGreaterThan_BoxSmallerThanPallet_ReturnsTrue()
	{
		var pallet = new Pallet(validPalletSize);
		var box = new Box(new Size3D(0.5, 0.5, 0.5), 10.0, validProductionDate);

		bool result = pallet > box;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void OperatorLessThan_BoxLargerThanPallet_ReturnsTrue()
	{
		var pallet = new Pallet(validPalletSize);
		var box = new Box(new Size3D(1.0, 2.0, 2.0), 10.0, validProductionDate);

		bool result = pallet < box;

		Assert.IsTrue(result);
	}
}
