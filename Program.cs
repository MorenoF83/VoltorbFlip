using System;
using System.Diagnostics.Tracing;
using System.Text.Json;

namespace VoltorbGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int flag = 1;
            while (flag == 1){
                Console.Clear();
                string userChoice = "";
                int[,] flipGrid = new int[5, 5];
                int flag2 = 1;
                var grid = placeBombs();
                var values = checkPoints(grid.Item1);
                int numFlipped = 0;
                while (flag2 == 1)
                {
                    Console.Clear();
                    flag2 = printGrid(grid.Item1, values.Item1, values.Item2, values.Item3, values.Item4, flipGrid, grid.Item2, numFlipped, 0);
                    if (flag2 != 0) {
                        Console.WriteLine("Flip what? Type QUIT to quit or RESTART to restart");
                        userChoice = Console.ReadLine();
                        flag2 = checkFlag(userChoice);
                        if (flag2 == 0) { break; }
                        flipGrid = flippedGrid(userChoice, flipGrid);
                        numFlipped += 1;
                    }
                }
            }
        }

        static int[,] flippedGrid(string choice, int[,] grid)
        {
            int flag = 1;
            while (flag == 1)
            {
                int i = -2;
                int j = -2;
                while (choice.Length != 2)
                {
                    Console.WriteLine("Wrong Input. Enter again: ");
                    choice = Console.ReadLine();
                }
                choice = choice.ToUpper();
                switch (choice[0])
                {
                    case 'A': i = 0; break;
                    case 'B': i = 1; break;
                    case 'C': i = 2; break;
                    case 'D': i = 3; break;
                    case 'E': i = 4; break;
                    default: i = -2; break;
                }
                switch (choice[1])
                {
                    case '1': j = 0; break;
                    case '2': j = 1; break;
                    case '3': j = 2; break;
                    case '4': j = 3; break;
                    case '5': j = 4; break;
                    default: j = -2; break;
                }
                if (i == -2 || j == -2){
                    Console.WriteLine("Wrong Input. Enter again: ");
                    choice = Console.ReadLine();
                }
                else
                {
                    grid[i, j] = 1;
                    flag = 0;
                }
            }
            return grid;
        }

        static (int[], int[], int[], int[]) checkPoints(int[,] grid) 
        {
            int[] pointsRow = new int[5];
            int[] pointsColumn = new int[5];
            int[] bombsRow = new int[5];
            int[] bombsColumn = new int[5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (grid[i,j] == -1)
                    {
                        bombsRow[i] += 1;
                        bombsColumn[j] += 1;
                    }
                    else
                    {
                        pointsRow[i] += grid[i,j];
                        pointsColumn[j] += grid[i,j];
                    }
                }
            }
            return (pointsRow, pointsColumn, bombsRow, bombsColumn);
        }

        static int printGrid(int[,] grid, int[] pointsrow, int[] pointsColumn, int[] bombsRow, int[] bombsColumn, int[,] flipped, int numBombs, int numFlipped, int final) {
            int lose = 0;
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0: Console.Write("Row A "); break;
                    case 1: Console.Write("Row B "); break;
                    case 2: Console.Write("Row C "); break;
                    case 3: Console.Write("Row D "); break;
                    case 4: Console.Write("Row E "); break;
                    default: Console.Write(""); break;
                }
                for (int j = 0; j < 5; j++)
                {
                    if ((grid[i, j] == -1 && flipped[i,j] == 1) || (grid[i, j] == -1 && final == 1))
                    {
                        Console.Write(String.Format("{0,10}", "[   X   ]"));
                        if (final == 0) { lose = 1; }
                    }
                    else if (flipped[i,j] == 1 || final == 1)
                    {
                        Console.Write(String.Format("{0,10}", "[   " + grid[i, j] + "   ]"));
                    }
                    else
                    {
                        Console.Write(String.Format("{0,10}", "[   ?   ]"));
                    }
                }
                Console.WriteLine(" Bombs: " + bombsRow[i] + " Points: " + pointsrow[i]);
            }
            Console.Write("      ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(String.Format("{0,10}", "Bombs: " + bombsColumn[i]));
            }
            Console.WriteLine("");
            Console.Write("      ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(String.Format("{0,10}", "Points: " + pointsColumn[i]));
            }
            Console.WriteLine("");
            Console.Write("      ");
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0: Console.Write(String.Format("{0,10}", "Col 1 ")); break;
                    case 1: Console.Write(String.Format("{0,10}", "Col 2 ")); break;
                    case 2: Console.Write(String.Format("{0,10}", "Col 3 ")); break;
                    case 3: Console.Write(String.Format("{0,10}", "Col 4 ")); break;
                    case 4: Console.WriteLine(String.Format("{0,10}", "Col 5 ")); break;
                    default: Console.Write(""); break;
                }
            }
            if (lose == 1) {
                Console.Clear();
                printGrid(grid, pointsrow, pointsColumn, bombsRow, bombsColumn, flipped, numBombs, numFlipped, 1);
                Console.WriteLine("YOU HIT A BOMB");
                Console.WriteLine("Type QUIT to quit or press enter to restart!");
                String userChoice = Console.ReadLine();
                int flag = checkFlag(userChoice);
                return 0;
            }
            else if ((25 - numFlipped) == numBombs)
            {
                Console.Clear();
                Console.WriteLine("YOU WIN");
                Console.WriteLine("Type QUIT to quit or press enter to restart!");
                String userChoice = Console.ReadLine();
                int flag = checkFlag(userChoice);
                return 0;
            }
            else { return 1; }
        }

        static (int[,], int) placeBombs()
        {
            var rand = new Random();
            int counter = 0;
            int[,] grid = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int random = rand.Next(101);
                    if (counter <= 8 && random >= 60)
                    {
                        grid[i, j] = -1;
                        counter++;
                    }
                    else if (random < 60 && random >= 40)
                    {
                        grid[i, j] = 3;
                    }
                    else if (random < 40 && random >= 20)
                    {
                        grid[i, j] = 2;
                    }
                    else
                    {
                        grid[i, j] = 1;
                    }

                }
            }
            return (grid, counter);
        }

        static int checkFlag(string choice)
        {
            if (choice == "QUIT")
            {
                System.Environment.Exit(0);
            }
            else if (choice == "RESTART")
            {
                return 0;
            }
            return 1;
        }
    }
}
