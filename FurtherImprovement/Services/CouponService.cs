namespace FurtherImprovement.Services;

public class CouponService
{
    public static List<CouponDiscount> Discounts = new List<CouponDiscount>();


    public static CouponDiscount GetCouponDiscount(string couponCode) => Discounts.Single(cd => cd.CouponCode == couponCode);
}

public record CouponDiscount
{
	public string CouponCode;
	public int NumberOfFreeBreakfasts;
	public decimal DiscountFactorOnOrder;

	public void UseFreeBreakfasts(int count)
	{
		NumberOfFreeBreakfasts -= count;
	}
}