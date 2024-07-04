namespace TenGroundsTest;

public class Classroom
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public List<FluorescentTubeUnit> FluorescentTubesUnits { get; set; } = [];
	public int HoursPerYear { get; set; } = default;

	public Classroom(int hoursPerYear, int numberOfFluorescentTubeUnits = 4)
	{
		HoursPerYear = hoursPerYear;
		for (int i = 1; i <= numberOfFluorescentTubeUnits; i++)
		{
			FluorescentTubesUnits.Add(new FluorescentTubeUnit(
				numberOfFluorescentTubes: FluorescentTubeUnit.DefaultNumberOfTubes, costPerUnit: FluorescentTube.DefaultTubeCost));
		}
	}

	public static void Simulate(int hoursPerDay = 15, int daysPerWeek = 5, int monthsPerYear = 9)
	{
		/*
		 *  15 hours/day
		 *  5 days/week
		 *  9 months/year
		 * 
		 *  15 * 5 = 75 hours/week
		 *	75 * 4 = 300 hours/month
		 *	300 * 9 = 2700 hours/year
		 */
		try
		{
			var hoursPerYear = hoursPerDay * daysPerWeek * 4 * monthsPerYear;

			var classroom = new Classroom(numberOfFluorescentTubeUnits: 4, hoursPerYear: hoursPerYear);

			var replacements = 0;
			var overallFaultyTubes = 0;
			List<Guid> unitsToReplace = [];

			Console.WriteLine($"Hours per year: {hoursPerYear}");

			for (var i = 1; i <= classroom.HoursPerYear; i++)
			{
				foreach (var fluorescentUnit in classroom.FluorescentTubesUnits)
				{
					Console.WriteLine($"Unit: {fluorescentUnit.Id}");

					if (fluorescentUnit.FaultyTubes == FluorescentTubeUnit.NumberOfFaultyTubesForUnitChange)
					{
						Console.WriteLine($"Unit already set for change. Ignoring...");
						continue;
					};

					foreach (var fluorescent in fluorescentUnit.FluorescentTubes)
					{
						Console.WriteLine($"FluorescentId: {fluorescent.Id}, FluorescentLifespan: {fluorescent.Lifespan}");

						if (fluorescent.Lifespan <= 0)
						{
							Console.WriteLine($"Tube reached lifespan. Ignoring...");
							continue;
						};

						fluorescent.Lifespan--;

						if (fluorescent.Lifespan <= 0)
						{
							Console.WriteLine($"Tube: {fluorescent.Id} of unit: {fluorescentUnit.Id} broke");
							fluorescentUnit.FaultyTubes++;
							overallFaultyTubes++;
						}

						if (fluorescentUnit.FaultyTubes == FluorescentTubeUnit.NumberOfFaultyTubesForUnitChange)
						{
							replacements++;
							unitsToReplace.Add(fluorescentUnit.Id);
							break;
						}
					}
				}
				Console.WriteLine($"End of hour {i}. Replacing {unitsToReplace.Count} units");
				foreach (var id in unitsToReplace)
				{
					var index = classroom.FluorescentTubesUnits.FindIndex(x => x.Id == id);
					classroom.FluorescentTubesUnits[index] = new FluorescentTubeUnit(
						numberOfFluorescentTubes: FluorescentTubeUnit.DefaultNumberOfTubes,
						costPerUnit: FluorescentTube.DefaultTubeCost);
				}
				unitsToReplace.Clear();
			}
			Console.WriteLine($"Number of broken tubes in 1 year: {overallFaultyTubes}");
			Console.WriteLine($"Total money expended per year per classroom: USD {replacements * FluorescentTubeUnit.DefaultNumberOfTubes * FluorescentTube.DefaultTubeCost}");
		}
		catch (Exception ex)
		{
			Console.WriteLine("Something went wrong");
			Console.WriteLine(ex.ToString());
		}
	}
}

public class FluorescentTubeUnit
{
	public const int NumberOfFaultyTubesForUnitChange = 2;
	public const int DefaultNumberOfTubes = 4;
	public Guid Id { get; set; } = Guid.NewGuid();
	public List<FluorescentTube> FluorescentTubes { get; set; } = [];
	public int FaultyTubes { get; set; } = default;

	public FluorescentTubeUnit(int numberOfFluorescentTubes = DefaultNumberOfTubes, int costPerUnit = FluorescentTube.DefaultTubeCost)
	{
		for (int i = 1; i <= numberOfFluorescentTubes; i++)
		{
			FluorescentTubes.Add(new FluorescentTube()
			{
				Id = i,
				Lifespan = FluorescentTube.Rand(100, 200),
				Cost = costPerUnit
			});
		}
	}
}

public class FluorescentTube
{
	public const int DefaultTubeCost = 7;
	public int Id { get; set; } = default;
	public int Lifespan { get; set; } = default;
	public int Cost { get; set; } = DefaultTubeCost;

	public static int Rand(int min, int max)
	{
		return new Random().Next(min, max + 1);
	}
}