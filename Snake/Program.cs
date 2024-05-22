using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Field
            const int fieldLength = 50, fieldWidth = 26;
            const char fieldTile = '#';
            string line = string.Concat(Enumerable.Repeat(fieldTile, fieldLength));

            //Snake
            int snakeX = fieldLength / 2;
            int snakeY = fieldWidth / 2;
            int direction = 1; // 1(->) 2(down) 3(<-) 4(up)
            int newdir = 1;
            int[] HeadPosList = new int[4] { snakeX, snakeY, direction, newdir };
            int[] BodyPosListOne = new int[4] { snakeX - 1, snakeY, direction, newdir };
            int[] BodyPosListTwo = new int[4] { snakeX - 2, snakeY, direction , newdir };
            int[] BodyPosListThree = new int[4] { snakeX - 3, snakeY, direction , newdir };

            const char snakeHead = 'O';
            const char snakeBody = 'o';

            int[][] snake = new int[][] { HeadPosList, BodyPosListOne, BodyPosListTwo, BodyPosListThree };
            Console.WriteLine(snake);

            //Score
            int Score = 0;


            int[] Apple = CreateApple(fieldLength, fieldWidth);
            Console.SetCursorPosition(Apple[0], Apple[1]);
            Console.WriteLine('A');
            while (true)
            {
                //Do until a key is pressed
                while (!Console.KeyAvailable)
                {
                    //Print the borders
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(line);

                    Console.SetCursorPosition(0, fieldWidth);
                    Console.WriteLine(line);

                    for (int i = 0; i < fieldWidth; i++)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(" ");
                        Console.SetCursorPosition(0, fieldWidth);
                        Console.WriteLine(" ");
                        Console.SetCursorPosition(1, i);
                        Console.WriteLine(fieldTile);
                        Console.SetCursorPosition(fieldLength, i);
                        Console.WriteLine(fieldTile);
                    }
                    if (snake[0][0] == 1 || snake[0][0] == fieldLength || snake[0][1] == 0 || snake[0][1] == fieldWidth)
                    {
                        break;
                    }
                    if (Apple[2] != 1)
                    {
                        Apple = CreateApple(fieldLength, fieldWidth);
                        Console.SetCursorPosition(Apple[0], Apple[1]);
                        Console.WriteLine('A');
                    }
                    if (snake[0][0] == Apple[0] && snake[0][1] == Apple[1])
                    {
                        Apple[2] = 0;
                        int newBodDir = snake[snake.Length - 1][2];
                        int newBodNewDir = snake[snake.Length - 1][3];
                        int newBodX = snake[snake.Length - 1][0];
                        int newBodY = snake[snake.Length - 1][1];
                        if (newBodDir == 1)
                        {
                            newBodX -= 1;
                        }
                        else if (newBodDir == 2)
                        {
                            newBodY -= 1;
                            
                        }
                        else if (newBodDir == 3)
                        {
                            newBodX += 1;
                        }
                        else if (newBodDir == 4)
                        {
                            newBodY += 1;
                        }
                        if (newBodDir != newBodNewDir)
                        {
                            //break;
                            //snake breaks when tail moves the same turn as the added one does break fixes by removing possibility
                            newBodNewDir = newBodDir;
                            // might have fixed problem, need to test to know if worked

                        }
                        int[] newBodList = new int[4] { newBodX, newBodY, newBodDir, newBodNewDir };
                        snake = snake.Append(newBodList).ToArray();
                        Score += 1;
                    }

                    Console.SetCursorPosition(fieldLength / 2, fieldWidth + 2);
                    Console.WriteLine(Score);

                    bool done = false;
                    for(int i = 1; i < snake.Length; i ++)
                    {
                        if (snake[0][0] == snake[i][0] && snake[0][1] == snake[i][1])
                        {
                            done = true;
                        }
                    }
                    if (done == true)
                    {
                        break;
                    }


                    // print snake
                    for (int i = 0; i < snake.Length; i++)
                    {
                        if (i == 0)
                        {
                            Console.SetCursorPosition(snake[i][0], snake[i][1]);
                            Console.WriteLine(snakeHead);
                        }
                        else
                        {
                            Console.SetCursorPosition(snake[i][0], snake[i][1]);
                            Console.WriteLine(snakeBody);
                        }
                    }
                    //Adds a timer so that the players have time to react
                    if (snake[0][2] == 2 || snake[0][2] == 4)
                    {
                        Thread.Sleep(200);
                    }
                    else
                    {
                        Thread.Sleep(100); 
                    }
                    //move snake
                    for (int i = 0; i < snake.Length; i++)
                    {
                        Console.SetCursorPosition(snake[i][0], snake[i][1]);
                        Console.WriteLine(" "); //Clears the previous position of the bodypart of the snake
                        if (i == 0)
                        {
                            snake[i] = MoveSnakePart(snake[i][0], snake[i][1], snake[i][2], snake[i][3], snake[i][3]);
                        }                        
                        else
                        {
                            snake[i] = MoveSnakePart(snake[i][0], snake[i][1], snake[i][2], snake[i - 1][2], snake[i][3]);
                        }
                        
                    }
                }

                //Check which key has been pressed
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (snake[0][2] != 2)
                        {
                            snake[0][3] = 4;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (snake[0][2] != 4)
                        {
                            snake[0][3] = 2;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (snake[0][2] != 1)
                        {
                            snake[0][3] = 3;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (snake[0][2] != 3)
                        {
                            snake[0][3] = 1;
                        }
                        break;

                }
            }
        }
        // this function takes the body part and moves it to the new location it should be at
        private static int[] MoveSnakePart(int X, int Y, int dir, int LastDir, int NewDir)
        {
            if (dir == 1)
            {
                X += 1;
            }
            else if (dir == 2)
            {
                Y += 1;
            }
            else if (dir == 3)
            {
                X -= 1;
            }
            else if (dir == 4)
            {
                Y -= 1;
            }
            int[] NewPosition = new int[4] { X, Y, NewDir, LastDir };
            return NewPosition;

        }
        private static int[] CreateApple(int fieldLength, int fieldWidth)
        {
            var random = new Random();
            int X = random.Next(3, fieldLength - 1);
            int Y = random.Next(2, fieldWidth - 1);
            int[] ApplePos = new int[3] {X, Y, 1};
            return ApplePos;
        }
    }
}
