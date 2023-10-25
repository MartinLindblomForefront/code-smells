namespace FurtherImprovement.Entities;

public record Person
{
    public string FirstName;
    public string LastName;
    public Age Age;
    public bool IsStudent;
    public bool IsSenior;
    public Membership Membership;
    public int NumberOfBreakfasts;
}