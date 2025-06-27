namespace PalletHub.Common.Extensions;

public static class DoubleExtensions
{
	public static bool IsBetween(this double value, double min, double max)
	{
		return value >= min && value <= max;
	}
	public static (double, double) OrderWith(this double first, double second)
	{
		return first < second ? (first, second) : (second, first);
	}
}
