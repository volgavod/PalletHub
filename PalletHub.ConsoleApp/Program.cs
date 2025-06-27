using PalletHub.Core.Models;
using PalletHub.Services.Factories;

namespace PalletHub.ConsoleApp;

class Program
{
	static void Main()
	{
		IEnumerable<Pallet> pallets = InitializePallets();
		PrintGroupedPallets(pallets);
		PrintTop3Pallets_Max(pallets);
		PrintTop3Pallets_Sum(pallets);
	}

	static IEnumerable<Pallet> InitializePallets()
	{
		BoxFactory boxFactory = new()
		{
			MaxSize = new(1, 1, 5),
			MinProductionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30))
		};
		PalletFactory palletFactory = new()
		{
			MinSize = new(0.1, 1, 1)
		};
		IEnumerable<Box> boxes = boxFactory.Generate(50);
		IEnumerable<Pallet> pallets = palletFactory.Generate(6, boxes);
		return pallets;
	}

	static void PrintGroupedPallets(IEnumerable<Pallet> pallets)
	{
		var groupedPallets = pallets
			.OrderBy(p => p.GeneralWeight)
			.GroupBy(p => p.BestBeforeDate)
			.OrderBy(g => g.Key);

		Console.WriteLine("Все паллеты сгруппированные по сроку годности:");
		foreach (var group in groupedPallets)
		{
			Console.WriteLine($"\n{group.Key}");
			foreach (var pallet in group)
				Console.WriteLine("id[{0}] - {1:F3} кг", pallet.Id, pallet.GeneralWeight);
		}
	}

	static void PrintTop3Pallets_Max(IEnumerable<Pallet> pallets)
	{
		var top3Pallets = pallets
			.Select(p => new 
			{
				Pallet = p, 
				MaxDate = p.Max(b => b.BestBeforeDate) 
			})
			.OrderByDescending(pd => pd.MaxDate)
			.Take(3)
			.OrderBy(p => p.Pallet.GeneralVolume);

		Console.WriteLine("\n\nТоп 3 паллета (по наибольшей дате):");
		foreach (var item in top3Pallets)
			Console.WriteLine("id[{0}] - {1:F3} м^3 - {2}", item.Pallet.Id, item.Pallet.GeneralVolume, item.MaxDate);
	}

	static void PrintTop3Pallets_Sum(IEnumerable<Pallet> pallets)
	{
		DateOnly now = DateOnly.FromDateTime(DateTime.Now);
		var top3Pallets = pallets
			.Select(p => new 
			{
				Pallet = p,
				MaxDays = p.Sum(b => b.BestBeforeDate.DayNumber - now.DayNumber) 
			})
			.OrderByDescending(pd => pd.MaxDays)
			.Take(3)
			.OrderBy(pd => pd.Pallet.GeneralVolume);

		Console.WriteLine("\nТоп 3 паллета (по сумме дней):");
		foreach (var item in top3Pallets)
			Console.WriteLine("id[{0}] - {1:F3} м^3 - {2}", item.Pallet.Id, item.Pallet.GeneralVolume, item.MaxDays);
	}
}
