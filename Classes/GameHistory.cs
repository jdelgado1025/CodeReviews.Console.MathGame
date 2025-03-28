using MathGame.Models;

namespace MathGame.Classes;
public class GameHistory
{
    private List<Record> History { get; set; }
    private const int _problemCount = 5;

    public GameHistory()
    {
        History = new List<Record>();
    }

    public void AddToHistory(int score, Operation operation)
    {
        History.Add(
            new Record
            {
                Date = DateTime.Now,
                Score = score,
                GameType = operation
            });
    }

    public void PrintHistory()
    {
        if (History.Count == 0)
        {
            Console.WriteLine("There is no history to display. Press any key to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var record in History)
        {
            Console.WriteLine($"{record.Date}: {record.GameType} - {record.Score} / {_problemCount}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
    }
}
