namespace FurtherImprovement.Services;

public class PaymentService
{
	public static decimal GetEarlyPaymentDiscount(DateTime orderTime, DateTime paymentTime)
	{
		if (IsEarlyPayee(orderTime, paymentTime))
			return -200;

		return 0;
	}

	private static bool IsEarlyPayee(DateTime orderTime, DateTime paymentTime)
	{
		return (paymentTime - orderTime).Days <= 10;
	}
}