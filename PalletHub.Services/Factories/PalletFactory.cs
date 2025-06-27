using PalletHub.Common.Extensions;
using PalletHub.Core.Models;

namespace PalletHub.Services.Factories;

public class PalletFactory
{
	public Size3D MinSize { get; set; } = Pallet.MinSize;
	public Size3D MaxSize { get; set; } = Pallet.MaxSize;

	public IEnumerable<Pallet> Generate(int amount)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));
		if (!MaxSize.IsGreaterOrEqualInAllAxes(MinSize))
			throw new ArgumentException("Максимальный размер не может быть меньше минимального");

		Random random = new();
		List<Pallet> pallets = new(amount);
		for (int i = 0; i < amount; i++)
			pallets.Add(new Pallet(new Size3D()
				{
					Height = random.NextDouble(MinSize.Height, MaxSize.Height),
					Width = random.NextDouble(MinSize.Width, MaxSize.Width),
					Depth = random.NextDouble(MinSize.Depth, MaxSize.Depth)
				}));

		return pallets;
	}

	public IEnumerable<Pallet> Generate(int amount, IEnumerable<Box> boxes)
	{
		List<Pallet> pallets = (List<Pallet>)Generate(amount);
		return FillPalletsByBoxes(pallets, boxes);
	}

	public static IEnumerable<Pallet> FillPalletsByBoxes(IEnumerable<Pallet> pallets, IEnumerable<Box> boxes)
	{
		ArgumentNullException.ThrowIfNull(pallets);
		ArgumentNullException.ThrowIfNull(boxes);
		List<Box> boxesList = boxes.ToList();
		List<Pallet> palletsList = pallets.ToList();
		if (boxesList.Count < palletsList.Count)
			throw new ArgumentException("Количество коробок должно быть больше количества паллет", nameof(boxes));

		int overfill = boxesList.Count % palletsList.Count;
		for (int p = 0, b = 0; p < palletsList.Count; p++)
		{
			int boxAmount = boxesList.Count / palletsList.Count + (p < overfill ? 1 : 0);
			for (int i = 0; i < boxAmount; i++, b++)
				palletsList[p].AddBox(boxesList[b]);
		}
		return palletsList;
	}
}
