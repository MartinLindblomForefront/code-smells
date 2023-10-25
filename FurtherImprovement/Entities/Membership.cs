namespace FurtherImprovement.Entities;

public abstract record Membership
{
    public DateTime StartDate;
    public int Points;

    public abstract decimal GetRoomDiscount();
    public abstract decimal GetBreakfastDiscount();
}

public record PlatinumMemberShip : Membership
{
    public override decimal GetRoomDiscount() => 0.9m;
    public override decimal GetBreakfastDiscount() => 0;
}

public record GoldMemberShip : Membership
{
    public override decimal GetRoomDiscount() => 0.9m;
    public override decimal GetBreakfastDiscount() => 1m;
}

public record SilverMemberShip : Membership
{
    public override decimal GetRoomDiscount() => 0.95m;
    public override decimal GetBreakfastDiscount() => 1m;
}

public record BronzeMemberShip : Membership
{
    public override decimal GetRoomDiscount() => 0.97m;
    public override decimal GetBreakfastDiscount() => 1m;
}