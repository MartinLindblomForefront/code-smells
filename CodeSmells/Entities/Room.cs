﻿namespace Problem.Entities;

public record Room
{
    public int NumberOfBeds;
    public bool CanHaveExtraBed;
    public int Size;
    public decimal BaseRate;
}