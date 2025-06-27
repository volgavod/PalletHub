using PalletHub.Core.Models;

namespace PalletHub.Tests.Core;

[TestClass]
public class Size3DTests
{
	[TestMethod]
	public void Constructor_ValidValues_SetsPropertiesCorrectly()
	{
		var size = new Size3D(10.0, 20.0, 30.0);

		Assert.AreEqual(10.0, size.Height);
		Assert.AreEqual(20.0, size.Width);
		Assert.AreEqual(30.0, size.Depth);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentOutOfRangeException))]
	public void Height_SetNegativeValue_ThrowsArgumentOutOfRangeException()
	{
		var size = new Size3D { Height = -1.0 };
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentOutOfRangeException))]
	public void Width_SetNegativeValue_ThrowsArgumentOutOfRangeException()
	{
		var size = new Size3D { Width = -1.0 };
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentOutOfRangeException))]
	public void Depth_SetNegativeValue_ThrowsArgumentOutOfRangeException()
	{
		var size = new Size3D { Depth = -1.0 };
	}

	[TestMethod]
	public void IsGreaterOrEqualInAllAxes_AllAxesGreater_ReturnsTrue()
	{
		var size1 = new Size3D(10.0, 20.0, 30.0);
		var size2 = new Size3D(5.0, 10.0, 15.0);

		var result = size1.IsGreaterOrEqualInAllAxes(size2);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsGreaterOrEqualInAllAxes_OneAxisNotGreater_ReturnsFalse()
	{
		var size1 = new Size3D(10.0, 20.0, 30.0);
		var size2 = new Size3D(15.0, 10.0, 15.0);

		var result = size1.IsGreaterOrEqualInAllAxes(size2);

		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsGreaterOrEqualInAllAxes_EqualValues_ReturnsFalse()
	{
		var size1 = new Size3D(10.0, 20.0, 30.0);
		var size2 = new Size3D(10.0, 20.0, 30.0);

		var result = size1.IsGreaterOrEqualInAllAxes(size2);

		Assert.IsFalse(result);
	}
}