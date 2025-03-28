using MathGame.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MathGame.Classes;
public class Game
{
    private Stopwatch _timer;
    private GameHistory _history;
    private int Score { get; set; }
    private const int _problemCount = 5;

    public Game()
    {
        Score = 0;
        _history = new();
        _timer = new();
    }

    public void Start()
    {
        Console.WriteLine("Hello! Welcome to the Math Game!");
        Thread.Sleep(1000);
        PlayGame();
    }

    private void PlayGame()
    {
        string menuSelection;
        bool showMenu = true;

        do
        {
            Console.Clear();
            DisplayMenu();
            menuSelection = GetUserInput();

            switch (menuSelection)
            {
                case "1":
                    RunOperationGame(Operation.Addition);
                    break;
                case "2":
                    RunOperationGame(Operation.Subtraction);
                    break;
                case "3":
                    RunOperationGame(Operation.Multiplication);
                    break;
                case "4":
                    RunOperationGame(Operation.Division);
                    break;
                case "5":
                    RunOperationGame(Operation.Random);
                    break;
                case "6":
                    _history.PrintHistory();
                    break;
                case "7":
                    showMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            
        } while (showMenu);
    }

    private void DisplayMenu()
    {
        string menu = @"Choose an option:
    1. Addition
    2. Subtraction
    3. Multiplication
    4. Division
    5. Random
    6. Previous Game History
    7. Quit

Enter your selection number:";
        Console.WriteLine(menu);
    }

    private void DisplayDifficultyLevelMenu()
    {
        string level = @"Choose the level of difficulty:
    1. Easy
    2. Medium
    3. Hard
    4. Extreme

Enter your selection number:";
        Console.WriteLine(level);
    }

    private void RunOperationGame(Operation operation)
    {
        int numberOne, numberTwo;
        Difficulty level = Difficulty.Easy;
        string levelSelection = "";
        bool isRandom = false, showMenu = true;

        if (operation == Operation.Random)
            isRandom = true;

        while(showMenu)
        {
            Console.Clear();
            DisplayDifficultyLevelMenu();
            levelSelection = GetUserInput();

            switch (levelSelection)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                    level = (Difficulty)Enum.Parse(typeof(Difficulty), levelSelection, true);
                    showMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid input.. Please try again.");
                    break;

            }
        }

        do
        {
            _timer.Start();
            Score = 0;
            Console.Clear();
            Console.WriteLine($"{operation} - {level}\n----------------------------------------------------");

            for (int i = 0; i < _problemCount; i++)
            {
                if (isRandom)
                    operation = GetOperation();
                (numberOne, numberTwo) = GenerateNumbers(operation, level);
                GenerateQuestion(numberOne, numberTwo, operation);
                CheckAnswer(CalculateAnswer(numberOne, numberTwo, operation));
            }

            _timer.Stop();
            DisplayResults();
            if (isRandom)
                operation = Operation.Random;
            
            _history.AddToHistory(Score, operation);
        }
        while (PlayAgain());

        Console.WriteLine("Returning to main menu...");
        Thread.Sleep(500);
    }

    private void GenerateQuestion(int numberOne, int numberTwo, Operation operation)
    {
        string operationSymbol = "";
        switch (operation)
        {
            case Operation.Addition:
                operationSymbol = "+";
                break;
            case Operation.Subtraction:
                operationSymbol = "-";
                break;
            case Operation.Multiplication:
                operationSymbol = "*";
                break;
            case Operation.Division:
                operationSymbol = "/";
                break;
        }

        Console.WriteLine($"{numberOne} {operationSymbol} {numberTwo}:");
    }

    private int CalculateAnswer(int numberOne, int numberTwo, Operation operation)
    {
        int answer = 0;
        switch(operation)
        {
            case Operation.Addition:
                answer = numberOne + numberTwo;
                break;
            case Operation.Subtraction:
                answer = numberOne - numberTwo;
                break;
            case Operation.Multiplication:
                answer = numberOne * numberTwo;
                break;
            case Operation.Division:
                answer = numberOne / numberTwo;
                break;
        }

        return answer;
    }

    private void CheckAnswer(int answer)
    {
        if (int.TryParse(Console.ReadLine(), out var userAnswer) && userAnswer == answer)
        {
            Console.WriteLine("You answered correctly!");
            Score++;
        }
        else
            Console.WriteLine("You answered incorrectly!");
    }

    private void DisplayResults()
    {
        Console.WriteLine($"Your score: {Score} / {_problemCount}");
        Console.WriteLine($"Time elapsed: {_timer.Elapsed.Hours}:{_timer.Elapsed.Minutes}:{_timer.Elapsed.Seconds}.{_timer.Elapsed.Milliseconds / 10}");
    }

    private (int, int) GenerateNumbers(Operation gameType, Difficulty difficultyLevel)
    {
        int numberOne, numberTwo;
        int upperLimit;

        const int lowerLimit = 0, divisorLowerLimit = 1;
        Random rand = new Random();

        switch(difficultyLevel)
        {
            case Difficulty.Easy:
                upperLimit = 100;
                break;
            case Difficulty.Medium:
                upperLimit = 300;
                break;
            case Difficulty.Hard:
                upperLimit = 500;
                break;
            case Difficulty.Extreme:
                upperLimit = 5000;
                break;
            default:
                upperLimit = 100;
                break;
        }

        if (gameType == Operation.Division)
        {
            /* Ensure the divisor is not 0 and result is a whole number */
            do
            {
                numberOne = rand.Next(lowerLimit, upperLimit);
                numberTwo = rand.Next(divisorLowerLimit, upperLimit);
            }
            while (numberOne % numberTwo != 0);
        }
        else
        {
            numberOne = rand.Next(lowerLimit, upperLimit);
            numberTwo = rand.Next(lowerLimit, upperLimit);
        }

        //Avoid negative number answers & higher number on the right side for Easy level
        if(difficultyLevel == Difficulty.Easy && numberTwo > numberOne)
        {
            int temp = numberOne;
            numberOne = numberTwo;
            numberTwo = temp;
        }

        /* Return two values using a Tuple */
        return (numberOne, numberTwo);
    }

    private Operation GetOperation()
    {
        Random rand = new Random();
        int getOperation = rand.Next(0, 4);

        return (Operation)getOperation;
    }

    private string GetUserInput()
    {
        string readInput = "";
        do
        {
            readInput = Console.ReadLine();
        } while (readInput == "" || readInput == null);

        return readInput.ToLower().Trim();
    }

    private bool PlayAgain()
    {
        Console.Write("Would you like to play again (y/n): ");
        var response = Console.ReadLine();

        return response.Trim().ToLower() == "y";
    }
}
