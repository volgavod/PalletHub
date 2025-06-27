namespace PalletHub.Common.Extensions;

public static class RandomExtensions
{
	public static double NextDouble(this Random random, double min, double max)
	{
		return min <= max
			? min + (max - min) * random.NextDouble()
			: throw new ArgumentException("Максимальное значение не может быть меньше минимального");
	}
	public static DateOnly NextDateOnly(this Random random, DateOnly start, DateOnly end)
	{
		return start <= end
			? start.AddDays(random.Next(end.DayNumber - start.DayNumber + 1))
			: throw new ArgumentException("Конечная дата не может быть раньше начальной");
	}
}
