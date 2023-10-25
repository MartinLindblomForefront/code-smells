using FurtherImprovement.Entities;

namespace FurtherImprovement.Services;

public class RoomService
{
	public static decimal GetAgeRoomRateDiscountFactor(Age age)
	{
		if (age.IsAtMost(10))
			return 0.5m;

		if (age.IsAtMost(18))
			return 0.9m;

		return 1;
	}
}