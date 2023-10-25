namespace Problem.Entities;

public record Membership
{
    public DateTime StartDate;
    public int Points;
    public MembershipLevel Level;
}