namespace FurtherImprovement.Entities;

public record Age
{
    public int Value { get; private set; }

    public bool IsAtMost(int age) => Value <= age;
}