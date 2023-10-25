using FurtherImprovement.Entities;

namespace FurtherImprovement.Services;

public class ExtraBedService
{
	public static decimal GetExtraBedCost(Reservation reservation)
	{
		if (!reservation.HasExtraBed)
			return 0;

		if (reservation.Room.CanHaveExtraBed)
			return 100 * reservation.NumberOfDays;

		return 200 * reservation.NumberOfDays;
	}
}