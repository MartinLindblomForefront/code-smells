using FurtherImprovement.Entities;

namespace FurtherImprovement.Services;

public class ReservationService
{
	public static decimal GetBulkDiscountFactor(Order order)
	{
		var numberOfOccupants = order.Reservations.Sum(r => r.Occupants.Count);
		var numberOfReservations = order.Reservations.Count;

		if (numberOfOccupants >= 10)
			return 0.9m;

		if (numberOfReservations >= 3)
			return 0.95m;

		return 1;
	}
}