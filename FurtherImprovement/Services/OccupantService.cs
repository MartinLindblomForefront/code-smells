using FurtherImprovement.Entities;

namespace FurtherImprovement.Services;

public class OccupantService
{
	public static decimal GetOccupantSpecificDiscount(Person occupant)
	{
		if (occupant.IsStudent)
			return -100;

		if (occupant.IsSenior)
			return -150;

		return 0;
	}
}