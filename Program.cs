using System;
using System.IO; //Чтение файла
using System.Threading; //Многопоточность 

namespace ConsoleApp35
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pacman
            Console.WindowHeight = 32;
            Console.WindowWidth = 28;
            Console.CursorVisible = false;

            char[,] map = ReadMap("map.txt");
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();

            var isWin = true;
            Task.Run(() =>
            {
                while (isWin)
                {
                    pressedKey = Console.ReadKey();
                }
            });

            int pacmanX = 14;
            int pacmanY = 23;
            int score = 0;


            while (isWin)
            {
                Console.Clear();

                HandleInput(pressedKey, ref pacmanX, ref pacmanY, map, ref score);

                Console.SetCursorPosition(0, 0);

                int dotsCount = 0;

                DrawMap(map, ref dotsCount);

                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("O");

                Console.SetCursorPosition(0, 31);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Score: {score}");

                if (dotsCount == 0)
                {
                    isWin = false;
                }
                Thread.Sleep(250);
            }

            Console.Clear();

            Console.SetCursorPosition(11, 14);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("You win!");
            Console.ReadLine();
        }

        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines("map.txt");

            char[,] map = new char[GetMaxLengthOfLine(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x];
                }
            }
            return map;
        }

        private static void DrawMap(char[,] map, ref int dotsCount)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (map[x, y] == '·')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                        dotsCount++;
                    }
                    Console.Write(map[x, y]);
                }
                Console.Write("\n");
            }
        }

        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int pacmanX, ref int pacmanY, char[,] map, ref int score)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPacmanPositionX = pacmanX + direction[0];
            int nextPacmanPositionY = pacmanY + direction[1];

            char nextCell = map[nextPacmanPositionX, nextPacmanPositionY];

            if (nextCell == ' ' || nextCell == '·')
            {
                pacmanX = nextPacmanPositionX;
                pacmanY = nextPacmanPositionY;
                if (nextCell == '·')
                {
                    score += 10;
                    map[nextPacmanPositionX, nextPacmanPositionY] = ' ';
                }
            }
        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };

            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    direction[1] = -1;
                    break;
                case ConsoleKey.DownArrow:
                    direction[1] = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    direction[0] = -1;
                    break;
                case ConsoleKey.RightArrow:
                    direction[0] = 1;
                    break;
            }
            return direction;
        }

        private static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }
            return maxLength;
        }
    }
}