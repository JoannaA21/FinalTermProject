using System;
using System.ComponentModel;
using System.Globalization;

namespace FinalTermProject
{
    public abstract class Player
    {
        public char Symbol;
        //public string Name;

        public Player(char symbol)
        {
            Symbol = symbol;
        }
        public virtual void Name()
        {
            //Write something here
        }

        public abstract int NextMove(); //Abstract method to get the next move from the player
    }

    //A class representing a human player
    public class HumanPlayer: Player
    {
        public HumanPlayer (char symbol) : base(symbol)
        { 
        }
        public override int NextMove()
        {
            Console.WriteLine("Enter the column number (1-7): ");
            int column = -1;
            while(!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 7: ");
            }
            return column - 1;
        }
    }

    //A class representing the computer player
    public class Computer: Player
    {
        private Random r;

        public Computer(char symbol) : base(symbol)
        {
            r = new Random();
        }
        public override int NextMove() //this will generate a random move(column number) for the computer player
        {
            return r.Next(0, 7);
        }
    }

    //Interface class -- defining all the methods that will be used in the ConnectFourGame
    public interface IGame
    {
        void StartGame();
        bool IsWinning(int row, int column); //Method for this is good
        bool PlacePiece(int column);
        bool IsBoardFull(); //Method for this is done
        void PrintBoard(); //Method for this is good
        void Restart(); //Method for this is done
    }
    
    //Class implementing the ConnectFourGame
    public class ConnectFour: IGame
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private int[,] board;
        private Player Player1;
        private Player Player2;
        private char CurrentPlayer;
        public ConnectFour() 
        {
            board = new int[Rows, Columns];
            Player1 = null;
            Player2 = null;
            CurrentPlayer = 'X';
        }
        
        public void PrintBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < Columns; j++)
                {
                    board[i, j] = '*';
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }

        public bool PlacePiece(int column)//Need work
        {
            ////////////////////////////////////////////////Need to implement this 
            for(int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, column] == '*')
                {
                    board[row, column] = CurrentPlayer; 
                    break;
                }
            }
            return false;
        }

        public bool IsWinning(int row, int column) //This should be good
        {
            int player = board[row, column];

            //Check horizontal
            int count = 0;
            for (int col = Math.Max(0, column - 3); col <=Math.Min(Columns - 4, column); col++)
            {
                if (board[row, col] == player &&
                    board[row, col + 1] == player &&
                    board[row, col + 2] == player &&
                    board[row, col + 3] == player)
                {
                    return true;
                }
            }

            //Check vertical
            count = 0;
            for (int r = Math.Max(0, row - 3), c = Math.Min(0, column - 4); r <= Math.Min(Rows - 4, row) && c <= Math.Min(Columns - 4, column); r++, c++) 
            {
                if (board[r, c] == player &&
                    board[r + 1, c + 1] == player &&
                    board[r + 2, c + 2] == player &&
                    board[r + 3, c + 3] == player)
                {
                    return true;
                }
            }

            //Check diagonal
            count = 0;
            for (int r = Math.Min(row - 1, row + 3), c = Math.Max(0, column - 3); r >= Math.Max(3, row) && c <= Math.Min(Columns - 4, column); r--, c++)
            {
                if (board[r, c] == player &&
                    board[r - 1, c + 1] == player &&
                    board[r - 2, c + 2] == player &&
                    board[r - 3, c + 3] == player)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsBoardFull() //This should be good
        {
            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Columns; c++)
                {
                    if (board[r, c] == 0)
                    return false; 
                }
            }
            return true;
        }

        public void Restart() //This should be good
        {
            int tryAgain = 0;
            while(tryAgain != 1 && tryAgain != 2)
            {
                Console.WriteLine("Do you want to play again?\n(Select 1 for 'Yes' or 2 for 'No')");
                int.TryParse(Console.ReadLine(), out tryAgain);
            }

            if(tryAgain == 1)
            {
                board = new int[Rows, Columns];
                CurrentPlayer = 'X';
            }
            else
            {
                Console.WriteLine("Game Over. Thank you for playing.");
            }
        }

        public void StartGame() //Need Work
        {
            Console.WriteLine("Welcome to ConnectFour!!!\n");

            Console.WriteLine("Game mode: ");
            Console.WriteLine("Select 1 for two Players game mode");
            Console.WriteLine("Select 2 for Player vs. Computer mode\n");
            
            int mode = 0;
            while (mode != 1 && mode != 2)
            {
                Console.WriteLine("Please choose game mode (1 or 2): ");
                int.TryParse(Console.ReadLine(), out mode);

            }

            if (mode == 1)
            {
                Console.WriteLine("Enter player 1's name: ");
                string player1Name = Console.ReadLine();
                Player1 = new HumanPlayer ('X');

                Console.WriteLine("Enter player 2's name: ");
                string player2Name = Console.ReadLine();
                Player2 = new HumanPlayer('O');
            }
            else
            {
                Console.WriteLine("Enter player's name: ");
                string player1Name = Console.ReadLine();
                Player1 = new HumanPlayer('X');
                string player2Name = "Computer";
                Player2 = new Computer('O');
            }

            bool GameOver = false;

            while (!GameOver)
            {
                Console.Clear();

                Console.WriteLine($"It is {CurrentPlayer}'s turn"); //last thing I worked on (June 18)
                PrintBoard();
                Restart();
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectFour game = new ConnectFour();

           game.StartGame();
        }
    }
}

//Work on:
//PlacePiece() method
//StartGame() method
//SwitchPlayer() method needs to be added 