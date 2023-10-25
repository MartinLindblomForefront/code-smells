namespace Improvement.Entities;

public record Order
{
    public List<Reservation> Reservations;
    public PaymentType PaymentType;
    public DateTime OrderDate;
    public string CouponCode;
}