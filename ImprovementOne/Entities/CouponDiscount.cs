namespace Improvement.Entities;

public class CouponDiscount
{
    public static List<CouponDiscount> Discounts = new List<CouponDiscount>();

    public string CouponCode;
    public int NumberOfFreeBreakfasts;
    public decimal DiscountFactorOnOrder;
}