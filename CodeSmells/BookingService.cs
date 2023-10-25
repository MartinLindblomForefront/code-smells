using Problem.Entities;

namespace Problem;

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

                var roomRateDiscount = 1m;
                var breakFastDiscount = 1m;

                switch (occupant.Membership.Level)
                {
	                // Membership level discounts
	                case MembershipLevel.Platinum:
		                roomRateDiscount = 0.9m;
		                breakFastDiscount = 0m; // Platinum level members get free breakfast
		                break;
	                case MembershipLevel.Gold:
		                roomRateDiscount = 0.9m;
		                breakFastDiscount = 1m;
		                break;
	                case MembershipLevel.Silver:
		                roomRateDiscount = 0.95m;
		                breakFastDiscount = 1m;
		                break;
                }

                // Age discounts
                if (occupant.Age <= 10)
                {
                    roomRateDiscount = 0.5m;
                    breakFastDiscount = 0.5m;
                }
                else if (occupant.Age <= 18)
                {
                    roomRateDiscount = 0.9m;
                }

                var occupantRoomRate = reservation.Room.BaseRate * roomRateDiscount;

                // Special discounts
                if (occupant.IsStudent)
                {
                    occupantRoomRate -= 100;
                }
                else if (occupant.IsSenior)
                {
                    occupantRoomRate -= 150;
                }

                // Clamp price to prevent negative price (McDonald's incident)
                if (occupantRoomRate < 0)
                {
                    occupantRoomRate = 0;
                }

                // Tally up the occupants price
                occupantPrice += occupantRoomRate * reservation.NumberOfDays;

                // Calculate free breakfasts and discounts
                var numberOfBreakfastsToPayFor = occupant.NumberOfBreakfasts;
                if (numberOfFreeBreakfasts > 0 && breakFastDiscount > 0)
                {
                    var freeBreakfastsToUse = Math.Min(numberOfFreeBreakfasts, numberOfBreakfastsToPayFor);
                    numberOfBreakfastsToPayFor -= freeBreakfastsToUse;
                    numberOfFreeBreakfasts -= freeBreakfastsToUse;
                }

                occupantPrice += Breakfast.BaseRate * numberOfBreakfastsToPayFor * breakFastDiscount;

                reservationPrice += occupantPrice;
            }

            // Calculate cost for extra bed
            if (reservation.HasExtraBed)
            {
                if (reservation.Room.CanHaveExtraBed)
                {
                    reservationPrice += 100 * reservation.NumberOfDays;
                }
                else
                {
                    reservationPrice += 200 * reservation.NumberOfDays;
                }
            }

            totalPrice += reservationPrice;
        }

        // Additional charges based on chosen payment method
		switch (order.PaymentType)
        {
	        case PaymentType.Invoice:
		        totalPrice += 25;
		        break;
	        case PaymentType.Card:
		        totalPrice += 15;
		        break;
	        case PaymentType.Cash:
		        totalPrice += 5;
		        break;
        }

        // Apply discount for early payers
        if ((DateTime.UtcNow - order.OrderDate).Days <= 10)
        {
            totalPrice -= 200;
        }

        // Clamp price
        if (totalPrice < 0)
        {
            totalPrice = 0;
        }

        // Apply bulk discounts
        if (totalDiscount < 1)
        {
            totalPrice *= totalDiscount;
        }
        else if (order.Reservations.Sum(r => r.Occupants.Count()) >= 10)
        {
            totalPrice *= 0.9m;
        }
        else if (order.Reservations.Count() >= 3)
        {
            totalPrice *= 0.95m;
        }

        return totalPrice;
    }
}