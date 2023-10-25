namespace FurtherImprovement.Guards;

public class PriceGuards
{
	public static decimal ClampToPositivePrice(decimal price)
	{
		return price < 0 ? 0 : price;
	}
}