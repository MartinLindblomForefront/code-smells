using FurtherImprovement.Entities;

namespace FurtherImprovement.Services;

public class BreakfastService
{
	public const decimal BaseRate = 100;

	public static decimal GetAgeBreakfastRateDiscountFactor(Age age)
	{
		if (age.IsAtMost(10))
			return 0.5m;

		return 1m;
	}

	public static int GetNumberOfBreakfastsToPayFor(int numberOfBreakfasts, CouponDiscount couponDiscount, decimal breakFastDiscount)
	{
		if (couponDiscount.NumberOfFreeBreakfasts > 0 && breakFastDiscount > 0)
		{
			var freeBreakfastsToUse = Math.Min(couponDiscount.NumberOfFreeBreakfasts, numberOfBreakfasts);
			couponDiscount.UseFreeBreakfasts(freeBreakfastsToUse);
			return numberOfBreakfasts - freeBreakfastsToUse;
		}

		return numberOfBreakfasts;
	}

	public static decimal GetBreakfastPriceForOccupant(int numberOfBreakfastsToPayFor, decimal breakfastDiscount)
	{
		return BaseRate * numberOfBreakfastsToPayFor * breakfastDiscount;
	}
}