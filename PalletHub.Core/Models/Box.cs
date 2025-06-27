using PalletHub.Common.Extensions;

namespace PalletHub.Core.Models;

public class Box
{
	private Size3D _size;
	private double _weight;

	public double Height => _size.Height;
	public double Width => _size.Width;
	public double Depth => _size.Depth;
	public double Volume => Height * Width * Depth;
	public double Weight
	{
		get => _weight;
		set => _weight = value > 0 ? value
			: throw new ArgumentException("Вес не может быть меньше нуля", nameof(value));
	}
	public DateOnly ProductionDate { get; init; }
	public DateOnly BestBeforeDate { get; init; }

	public static Size3D MinSize => new(0.05, 0.05, 0.05);
	public static Size3D MaxSize => new(5, 5, 5);

	public Box(Size3D size, double weight, DateOnly productionDate) 
		: this(size, weight, productionDate, productionDate.AddDays(100)) { }
	public Box(Size3D size, double weight, DateOnly productionDate, DateOnly bestBeforeDate)
	{
		CheckSize(size);
		_size = size;
		_weight = weight;
		ProductionDate = productionDate;
		BestBeforeDate = bestBeforeDate;
	}

	public (double, double) GetTwoSmallestSizes()
	{
		var sizes = new double[] { Height, Width, Depth }.Order();
		return (sizes.First(), sizes.ElementAt(1));
	}

	private static void CheckSize(Size3D size)
	{
		if (size.Height.IsBetween(0.05, 5) &&
			size.Width.IsBetween(0.05, 5) &&
			size.Depth.IsBetween(0.05, 5))
			return;
		throw new ArgumentException("Сторона коробки должна быть в пределах 0.05-5 м", nameof(size));
	}
}
