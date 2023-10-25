using FurtherImprovement.Entities;
using FurtherImprovement.Guards;
using FurtherImprovement.Services;

namespace FurtherImprovement;

public class BookingService
{
	public decimal CalculateTotalPrice(Order order)
	{
		var couponDiscount = CouponService.GetCouponDiscount(order.CouponCode);

		var totalPrice = 0m;
		totalPrice += order.Reservations.Sum(r => GetPriceForReservation(r, couponDiscount));
		totalPrice += order.PaymentType.GetServiceFee();
		totalPrice += PaymentService.GetEarlyPaymentDiscount(order.OrderDate, DateTime.UtcNow);

		totalPrice *= GetTotalDiscount(order, couponDiscount);

		return PriceGuards.ClampToPositivePrice(totalPrice);
	}



	private static decimal GetPriceForReservation(Reservation reservation, CouponDiscount couponDiscount)
	{
		var reservationPrice = 0m;

		reservationPrice += reservation.Occupants.Sum(o => GetPriceForOccupant(o, reservation, couponDiscount));
		reservationPrice += ExtraBedService.GetExtraBedCost(reservation);
		
		return reservationPrice;
	}

	private static decimal GetPriceForOccupant(Person occupant, Reservation reservation, CouponDiscount couponDiscount)
	{
		var occupantPrice = 0m;

		occupantPrice += GetOccupantRoomPrice(occupant, reservation);
		occupantPrice += GetOccupantBreakfastPrice(occupant, couponDiscount);

		return occupantPrice;
	}

	private static decimal GetOccupantRoomPrice(Person occupant, Reservation reservation)
	{
		var roomRateDiscount = Math.Min(occupant.Membership.GetRoomDiscount(), RoomService.GetAgeRoomRateDiscountFactor(occupant.Age));
		var occupantRoomRate = reservation.Room.BaseRate * roomRateDiscount;
		occupantRoomRate += OccupantService.GetOccupantSpecificDiscount(occupant);
		
		return PriceGuards.ClampToPositivePrice(occupantRoomRate * reservation.NumberOfDays);
	}

	private static decimal GetOccupantBreakfastPrice(Person occupant, CouponDiscount couponDiscount)
	{
		var breakFastDiscount = Math.Min(occupant.Membership.GetBreakfastDiscount(), BreakfastService.GetAgeBreakfastRateDiscountFactor(occupant.Age));
		var numberOfBreakfastsToPayFor = BreakfastService.GetNumberOfBreakfastsToPayFor(occupant.NumberOfBreakfasts, couponDiscount, breakFastDiscount);
		
		return BreakfastService.GetBreakfastPriceForOccupant(numberOfBreakfastsToPayFor, breakFastDiscount);
	}

	private static decimal GetTotalDiscount(Order order, CouponDiscount couponDiscount)
	{
		var bulkDiscount = ReservationService.GetBulkDiscountFactor(order);
		var orderDiscount = couponDiscount.DiscountFactorOnOrder;
		return Math.Min(bulkDiscount, orderDiscount);
	}
}