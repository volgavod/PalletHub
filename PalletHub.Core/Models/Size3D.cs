namespace PalletHub.Core.Models;

public struct Size3D
{
	private double _height;
	private double _width;
	private double _depth;

	public double Height 
	{ 
		get => _height;
		set => _height =
			value > 0
			? value
			: throw new ArgumentOutOfRangeException(nameof(value), "Высота не может быть отрицательной");
	}
	public double Width
	{
		get => _width;
		set => _width =
			value > 0
			? value
			: throw new ArgumentOutOfRangeException(nameof(value), "Ширина не может быть отрицательной");
	}
	public double Depth
	{
		get => _depth;
		set => _depth =
			value > 0
			? value
			: throw new ArgumentOutOfRangeException(nameof(value), "Глубина не может быть отрицательной");
	}

	public Size3D() { }
	public Size3D(double height, double width, double depth)
	{
		Height = height;
		Width = width;
		Depth = depth;
	}

	public bool IsGreaterOrEqualInAllAxes(Size3D other)
	{
		return _height > other.Height && _width > other.Width && _depth > other.Depth;
	}
}
