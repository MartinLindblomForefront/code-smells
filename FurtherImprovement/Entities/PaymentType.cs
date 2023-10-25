namespace FurtherImprovement.Entities;

public abstract class PaymentType
{
	public abstract decimal GetServiceFee();
}

public class CardPayment : PaymentType
{
	public override decimal GetServiceFee() => 15m;
}

public class InvoicePayment : PaymentType
{
	public override decimal GetServiceFee() => 15m;
}

public class CashPayment : PaymentType
{
	public override decimal GetServiceFee() => 15m;
}

public class PointsPayment : PaymentType
{
	public override decimal GetServiceFee() => 15m;
}