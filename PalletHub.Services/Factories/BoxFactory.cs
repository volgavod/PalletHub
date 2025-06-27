using PalletHub.Common.Extensions;
using PalletHub.Core.Models;

namespace PalletHub.Services.Factories;

public class BoxFactory
{
	public Size3D MinSize { get; set; } = Box.MinSize;
	public Size3D MaxSize { get; set; } = Box.MaxSize;
	public double MinWeight { get; set; } = 0.01;
	public double MaxWeight { get; set; } = 50;
	public DateOnly MinProductionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));
	public DateOnly MaxProductionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	
	public IEnumerable<Box> Generate(int amount)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount,  nameof(amount));
		CheckGenerationRules();
		Random random = new();
		List<Box> boxes = new(amount);
		for (var i = 0; i < amount; i++)
		{
			double weight = random.NextDouble(MinWeight, MaxWeight);
			DateOnly productionDate = random.NextDateOnly(MinProductionDate, MaxProductionDate);
			Size3D size = new()
			{ 
				Height = random.NextDouble(MinSize.Height, MaxSize.Height),
				Width = random.NextDouble(MinSize.Width, MaxSize.Width),
				Depth = random.NextDouble(MinSize.Depth, MaxSize.Depth)
			};

			boxes.Add(new Box(size, weight, productionDate));
		}
		return boxes;
	}

	private void CheckGenerationRules()
	{
		if (!MaxSize.IsGreaterOrEqualInAllAxes(MinSize))
			throw new ArgumentException("Максимальный размер не может быть меньше минимального");
		if (MinWeight > MaxWeight)
			throw new ArgumentException("Максимальный вес не может быть меньше минимального");
		if (MinProductionDate > MaxProductionDate)
			throw new ArgumentException("Конечная дата производства не может быть раньше начальной");
	}
}
