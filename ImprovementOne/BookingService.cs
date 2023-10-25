using Improvement.Entities;

namespace Improvement;

public class BookingService
{
    public decimal CalculateTotalPrice(Order order)
    {
        var totalPrice = 0m;

        var couponDiscount = CouponDiscount.Discounts.Single(cd => cd.CouponCode == order.CouponCode);
        var numberOfFreeBreakfasts = couponDiscount.NumberOfFreeBreakfasts;
        var totalDiscount = couponDiscount.DiscountFactorOnOrder;

        // Calculate price per reservation
        foreach (var reservation in order.Reservations)
        {
            var reservationPrice = 0m;

            // Calculate price per occupant of a reservation
            foreach (var occupant in reservation.Occupants)
            {
                var occupantPrice = 0m;

                var roomRateDiscount = Math.Min(occupant.Membership.GetRoomDiscount(), GetAgeRoomRateDiscountFactor(occupant.Age));

                var occupantRoomRate = reservation.Room.BaseRate * roomRateDiscount;
                occupantRoomRate += GetOccupantSpecificDiscount(occupant);
                occupantPrice = ClampToPositivePrice(occupantRoomRate * reservation.NumberOfDays);

				var breakFastDiscount = Math.Min(occupant.Membership.GetBreakfastDiscount(), GetAgeBreakfastRateDiscountFactor(occupant.Age));
				var numberOfBreakfastsToPayFor = GetNumberOfBreakfastsToPayFor(occupant.NumberOfBreakfasts, numberOfFreeBreakfasts, breakFastDiscount);
				numberOfFreeBreakfasts -= occupant.NumberOfBreakfasts - numberOfBreakfastsToPayFor;
				occupantPrice += GetBreakfastPriceForOccupant(numberOfBreakfastsToPayFor, breakFastDiscount);

				reservationPrice += occupantPrice;
            }
            
            reservationPrice += GetExtraBedCost(reservation);

            totalPrice += reservationPrice;
        }

        totalPrice += order.PaymentType.GetServiceFee();
        totalPrice += GetEarlyPaymentDiscount(order.OrderDate, DateTime.UtcNow);

        totalPrice = ClampToPositivePrice(totalPrice);

        var numberOfOccupants = order.Reservations.Sum(r => r.Occupants.Count);
        var numberOfReservations = order.Reservations.Count;
        var bulkDiscount = GetBulkDiscountFactor(numberOfOccupants, numberOfReservations);
        
        totalPrice *= Math.Min(totalDiscount, bulkDiscount);

        return totalPrice;
    }


    private static decimal GetEarlyPaymentDiscount(DateTime orderTime, DateTime paymentTime)
    {
	    if (IsEarlyPayee(orderTime, paymentTime))
		    return -200;

	    return 0;
    }

	private static bool IsEarlyPayee(DateTime orderTime, DateTime paymentTime)
    {
	    return (paymentTime - orderTime).Days <= 10;
    }


    private static decimal ClampToPositivePrice(decimal price)
    {
	    return price < 0 ? 0 : price;
    }

    private static decimal GetBulkDiscountFactor(int numberOfOccupants, int numberOfReservations)
    {
	    if (numberOfOccupants >= 10)
		    return 0.9m;

	    if (numberOfReservations >= 3)
		    return 0.95m;

	    return 1;
    }

    private static decimal GetOccupantSpecificDiscount(Person occupant)
    {
	    if (occupant.IsStudent)
		    return -100;

	    if (occupant.IsSenior)
		    return -150;

	    return 0;
    }

    private static decimal GetAgeRoomRateDiscountFactor(Age age)
    {
	    if (age.IsAtMost(10))
		    return 0.5m;

	    if (age.IsAtMost(18))
		    return 0.9m;

	    return 1;
    }

    private static decimal GetAgeBreakfastRateDiscountFactor(Age age)
    {
	    if (age.IsAtMost(10))
		    return 0.5m;

	    return 1m;
    }

    private static int GetNumberOfBreakfastsToPayFor(int numberOfBreakfasts, int numberOfFreeBreakfasts, decimal breakFastDiscount)
    {
	    if (numberOfFreeBreakfasts > 0 && breakFastDiscount > 0)
	    {
		    var freeBreakfastsToUse = Math.Min(numberOfFreeBreakfasts, numberOfBreakfasts);
            return numberOfBreakfasts - freeBreakfastsToUse;
	    }

        return numberOfBreakfasts;
	}

    private static decimal GetBreakfastPriceForOccupant(int numberOfBreakfastsToPayFor, decimal breakfastDiscount)
    {
        return Breakfast.BaseRate * numberOfBreakfastsToPayFor * breakfastDiscount;
    }

    private static decimal GetExtraBedCost(Reservation reservation)
    {
	    if (!reservation.HasExtraBed)
		    return 0;

	    if (reservation.Room.CanHaveExtraBed)
		    return 100 * reservation.NumberOfDays;

	    return 200 * reservation.NumberOfDays;
    }
}