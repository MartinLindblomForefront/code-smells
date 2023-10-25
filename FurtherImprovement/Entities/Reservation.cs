namespace FurtherImprovement.Entities;

public record Reservation
{
    public Room Room;
    public DateTime StartDate;
    public DateTime EndDate;
    public List<Person> Occupants;
    public bool HasExtraBed;

    public int NumberOfDays => (EndDate - StartDate).Days;
}