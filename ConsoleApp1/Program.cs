using System;
using System.Collections.Generic;

public class Classroom
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public List<FluorescentTubeUnit> FluorescentTubesUnits { get; set; } = new List<FluorescentTubeUnit>();
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

			var classroom = new Classroom(hoursPerYear: hoursPerYear, numberOfFluorescentTubeUnits: 4);

			var replacements = 0;
			var overallFaultyTubes = 0;
			List<Guid> unitsToReplace = new List<Guid>();

			for (var i = 1; i <= classroom.HoursPerYear; i++)
			{
				foreach (var fluorescentUnit in classroom.FluorescentTubesUnits)
				{
					if (fluorescentUnit.FaultyTubes == FluorescentTubeUnit.NumberOfFaultyTubesForUnitChange)
					{
						continue;
					};

					foreach (var fluorescent in fluorescentUnit.FluorescentTubes)
					{

						if (fluorescent.Lifespan <= 0)
						{
							continue;
						};

						fluorescent.Lifespan--;

						if (fluorescent.Lifespan <= 0)
						{
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
				foreach (var id in unitsToReplace)
				{
					var index = classroom.FluorescentTubesUnits.FindIndex(x => x.Id == id);
					classroom.FluorescentTubesUnits[index] = new FluorescentTubeUnit(
						numberOfFluorescentTubes: FluorescentTubeUnit.DefaultNumberOfTubes,
						costPerUnit: FluorescentTube.DefaultTubeCost);
				}
				unitsToReplace.Clear();
			}
			var initialMoneyExpended = classroom.FluorescentTubesUnits.Count *	
				FluorescentTubeUnit.DefaultNumberOfTubes *
				FluorescentTube.DefaultTubeCost;

			var totalMoneyExpended = initialMoneyExpended +
				(replacements * 
				FluorescentTubeUnit.DefaultNumberOfTubes *
				FluorescentTube.DefaultTubeCost);


			Console.WriteLine($"Number of broken tubes in 1 year: {overallFaultyTubes}");
			Console.WriteLine($"Total money expended per year per classroom: USD {totalMoneyExpended}");
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
	public List<FluorescentTube> FluorescentTubes { get; set; } = new List<FluorescentTube>();
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

	private static Random Random = new Random();

	public static int Rand(int min, int max)
	{
		return Random.Next(min, max + 1);
	}
}

public class Program
{
	public static void Main(string[] args)
	{
		Classroom.Simulate(hoursPerDay: 15, daysPerWeek: 5, monthsPerYear: 9);
	}
}