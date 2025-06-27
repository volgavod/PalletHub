using PalletHub.Core.Models;
using PalletHub.Services.Factories;

namespace PalletHub.Tests.Services;

[TestClass]
public class BoxFactoryTests
{
	private readonly BoxFactory _factory = new();
	private readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Now);
	private readonly DateOnly _lastYear = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));

	[TestMethod]
	public void Generate_ValidAmount_ReturnsCorrectNumberOfBoxes()
	{
		var boxes = _factory.Generate(5);

		Assert.AreEqual(5, boxes.Count());
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
	[ExpectedException(typeof(ArgumentException))]
	public void Generate_MaxSizeLessThanMinSize_ThrowsArgumentException()
	{
		_factory.MinSize = new Size3D(6.0, 6.0, 6.0);
		_factory.MaxSize = Box.MaxSize;

		_factory.Generate(1);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Generate_MaxWeightLessThanMinWeight_ThrowsArgumentException()
	{
		_factory.MinWeight = 51.0;
		_factory.MaxWeight = 50.0;

		_factory.Generate(1);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void Generate_MaxProductionDateLessThanMinProductionDate_ThrowsArgumentException()
	{
		_factory.MinProductionDate = _today;
		_factory.MaxProductionDate = _lastYear;

		_factory.Generate(1);
	}
}