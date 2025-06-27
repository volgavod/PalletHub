using PalletHub.Core.Models;
using PalletHub.Services.Factories;

namespace PalletHub.Tests.Services;

[TestClass]
public class PalletFactoryTests
{
	private readonly PalletFactory _factory = new();
	private readonly Size3D _validPalletSize = new(0.15, 1.0, 1.0);
	private readonly Size3D _validBoxSize = new(0.5, 0.5, 0.5);
	private readonly DateOnly _validProductionDate = new(2025, 1, 1);

	[TestMethod]
	public void Generate_ValidAmount_ReturnsCorrectNumberOfPallets()
	{
		var pallets = _factory.Generate(5);

		Assert.AreEqual(5, pallets.Count());
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentOutOfRangeException))]
	public void Generate_NegativeAmount_ThrowsArgumentOutOfRangeException()
	{
		_factory.Generate(-1);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentOutOfRangeException))]
	public void Generate_ZeroAmount_ThrowsArgumentOutOfRangeException()
	{
		_factory.Generate(0);
	}

	[TestMethod]
	public void Generate_ValidParameters_PalletsWithinConstraints()
	{
		var pallets = _factory.Generate(5);

		foreach (var pallet in pallets)
		{
			Assert.IsTrue(pallet.Height >= 0.1 && pallet.Height <= 0.25);
			Assert.IsTrue(pallet.Width >= 0.6 && pallet.Width <= 2.0);
			Assert.IsTrue(pallet.Depth >= 0.6 && pallet.Depth <= 2.0);
		}
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Generate_MaxSizeLessThanMinSize_ThrowsArgumentException()
	{
		_factory.MinSize = new Size3D(0.3, 2.1, 2.1);
		_factory.MaxSize = new Size3D(0.25, 2.0, 2.0);

		_factory.Generate(1);
	}

	[TestMethod]
	public void Generate_WithBoxes_ValidParameters_DistributesBoxesCorrectly()
	{
		var boxes = Enumerable.Repeat(new Box(_validBoxSize, 10.0, _validProductionDate), 4);
		var pallets = _factory.Generate(2, boxes).ToList();

		Assert.AreEqual(2, pallets.Count);
		Assert.AreEqual(2, pallets[0].Count());
		Assert.AreEqual(2, pallets[1].Count());
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Generate_WithBoxes_LessBoxesThanPallets_ThrowsArgumentException()
	{
		var boxes = new List<Box>
		{
			new (_validBoxSize, 10.0, _validProductionDate)
		};

		_factory.Generate(2, boxes);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void Generate_WithBoxes_NullBoxes_ThrowsArgumentNullException()
	{
		_factory.Generate(1, null);
	}

	[TestMethod]
	public void FillPalletsByBoxes_ValidParameters_DistributesBoxesEvenly()
	{
		var pallets = new List<Pallet> { new(_validPalletSize), new(_validPalletSize) };
		var boxes = new List<Box>
		{
			new(_validBoxSize, 10.0, _validProductionDate),
			new(_validBoxSize, 10.0, _validProductionDate),
			new(_validBoxSize, 10.0, _validProductionDate),
			new(_validBoxSize, 10.0, _validProductionDate)
		};

		var result = PalletFactory.FillPalletsByBoxes(pallets, boxes).ToList();

		Assert.AreEqual(2, result[0].Count());
		Assert.AreEqual(2, result[1].Count());
		Assert.AreEqual(50.0, result[0].GeneralWeight);
		Assert.AreEqual(50.0, result[1].GeneralWeight);
	}

	[TestMethod]
	public void FillPalletsByBoxes_UnevenDistribution_DistributesCorrectly()
	{
		var pallets = new List<Pallet> { new(_validPalletSize), new(_validPalletSize) };
		var boxes = new List<Box>
		{
			new(_validBoxSize, 10.0, _validProductionDate),
			new(_validBoxSize, 10.0, _validProductionDate),
			new(_validBoxSize, 10.0, _validProductionDate)
		};

		var result = PalletFactory.FillPalletsByBoxes(pallets, boxes).ToList();

		Assert.AreEqual(2, result[0].Count());
		Assert.AreEqual(1, result[1].Count());
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void FillPalletsByBoxes_BoxTooLarge_ThrowsException()
	{
		var pallets = new List<Pallet> { new(_validPalletSize) };
		var boxes = new List<Box> { new(new Size3D(1.0, 2.0, 2.0), 10.0, _validProductionDate) };

		PalletFactory.FillPalletsByBoxes(pallets, boxes);
	}
}