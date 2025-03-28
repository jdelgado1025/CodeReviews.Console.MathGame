namespace MathGame.Models;
public class Record
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public Operation GameType { get; set; }
    public Difficulty Level { get; set; }
}

public enum Operation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Random
}

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3,
    Extreme = 4
}