using PalletHub.Common.Extensions;
using System.Collections;

namespace PalletHub.Core.Models;

public class Pallet : IEnumerable<Box>
{
	private static int _ids = 0;
	private Size3D _size;
	private const double _weight = 30;
	private List<Box> _boxes = [];

	public int Id { get; init; }
	public double Height => _size.Height;
	public double Width => _size.Width;
	public double Depth => _size.Depth;
	public double GeneralVolume => Height * Width * Depth + _boxes.Sum(box => box.Volume);
	public double GeneralWeight => _weight + _boxes.Sum(box => box.Weight);
	public DateOnly BestBeforeDate => _boxes.Min(box => box.BestBeforeDate);

	public static Size3D MinSize => new(0.1, 0.6, 0.6);
	public static Size3D MaxSize => new(0.25, 2, 2);

	public Pallet(Size3D size)
	{
		CheckPalletSize(size);
		Id = _ids++;
		_size = size;
	}

	public void AddBox(Box box)
	{
		if (this >= box)
			_boxes.Add(box);
		else
			throw new Exception();
	}

	public bool RemoveBox(Box box)
	{
		return _boxes.Remove(box);
	}

	private static void CheckPalletSize(Size3D size)
	{
		if (size.Height.IsBetween(0.1, 0.25) &&
			size.Width.IsBetween(0.6, 2) &&
			size.Depth.IsBetween(0.6, 2))
			return;
		throw new ArgumentException(@"Паллет ограничен следующими размерами (м):
			ширина 0.5-2, высота 0.1-0.25, глубина 0.5-2", nameof(size));
	}

	public IEnumerator<Box> GetEnumerator()
	{
		return ((IEnumerable<Box>)_boxes).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_boxes).GetEnumerator();
	}

	public static bool operator <(Pallet pallet, Box box)
	{
		(double, double) boxSize = box.GetTwoSmallestSizes();
		(double, double) palletSize = pallet.Width.OrderWith(pallet.Depth);

		if (palletSize.Item1 < boxSize.Item1 || palletSize.Item2 < boxSize.Item2)
			return true;
		return false;
	}
	public static bool operator >(Pallet pallet, Box box)
	{
		(double, double) boxSize = box.GetTwoSmallestSizes();
		(double, double) palletSize = pallet.Width.OrderWith(pallet.Depth);

		if (palletSize.Item1 > boxSize.Item1 || palletSize.Item2 > boxSize.Item2)
			return true;
		return false;
	}
	public static bool operator >=(Pallet pallet, Box box)
	{
		return !(pallet < box);
	}
	public static bool operator <=(Pallet pallet, Box box)
	{
		return !(pallet > box);
	}
}
