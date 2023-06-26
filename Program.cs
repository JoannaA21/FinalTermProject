
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace FinalTermProject
{
    // Abstract class representing a player
    public abstract class Player
    {
        public string Symbol;
        public string Name;

        public Player(string name, string symbol)
        {
            Symbol = symbol;
            Name = name;
        }

        //Abstract method to get the next move from the player
        public abstract int MakeMove();
    }

    //A class representing a human player
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, string symbol) : base(name, symbol)
        {
        }

        public override int MakeMove()  // Implements the MakeMove method for a human player
        {
            Console.WriteLine("Enter the column number (1-7): ");
            int column = 0;
            while (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 7: ");
            }
            column = column - 1;
            return column;
        }
    }

    //A class representing AI player
    public class AI_Player : Player
    {
        private Random r;
        public AI_Player(string name, string symbol) : base(name, symbol)
        {
            r = new Random();
        }
        public override int MakeMove()  // Implements the MakeMove method for the AI player
        {
            //return r.Next(0, 7);
            Console.WriteLine("AI Player is making a move...");

            // Add a slight delay to simulate the AI's decision-making process
            System.Threading.Thread.Sleep(1000);

            // Generate a random move (column number)
            int column = r.Next(0, 7);
            return column;
        }
    }

    //Interface class -- defining all the methods that will be used in the ConnectFourGame
    public interface IGame
    {
        void StartGame();

    }

    //Class implementing the ConnectFourGame
    public class ConnectFour : IGame
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private char[,] board;
        private Player Player1;
        private Player Player2;
        private Player CurrentPlayer;
        private bool PlayerIndex;
        public ConnectFour()
        {
            board = new char[Rows, Columns];
            InitializeBoard();
            PlayerIndex = true;
        }


        private void InitializeBoard() // Initializes the game board with empty cells
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    board[r, c] = '*';
                }
            }
        }
        private void PrintBoard() // Prints the current state of the game board
        {
            for (int r = 0; r < Rows; r++)
            {
                Console.Write("| ");
                for (int c = 0; c < Columns; c++)
                {
                    Console.Write(board[r, c] + " ");
                }
                Console.WriteLine("|");
            }
        }


        private bool PlacePiece(int column) // Places a player's piece on the board at the specified column
        {
            if (InvalidMove(column))
            {
                Console.WriteLine("Invalid move! Please choose another move.");
                return false;
            }

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (board[r, column] == '*')
                {
                    board[r, column] = CurrentPlayer.Symbol[0];
                    if (IsWinning(r, column))
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        private bool InvalidMove(int column) // Checks if a move is valid (within the board boundaries and the column is not full)
        {
            return column < 0 || column >= Columns || board[0, column] != '*';
        }

        private bool IsWinning(int row, int column)
        {
            char player = board[row, column];

            // Check horizontal
            for (int c = Math.Max(0, column - 3); c <= Math.Min(Columns - 4, column); c++)
            {
                if (board[row, c] == player &&
                    board[row, c + 1] == player &&
                    board[row, c + 2] == player &&
                    board[row, c + 3] == player)
                {
                    return true;
                }
            }

            // Check vertical
            for (int r = Math.Max(0, row - 3); r <= Math.Min(Rows - 4, row); r++)
            {
                if (board[r, column] == player &&
                    board[r + 1, column] == player &&
                    board[r + 2, column] == player &&
                    board[r + 3, column] == player)
                {
                    return true;
                }
            }

            // Check diagonal (left to bottom)
            int startRow = row;
            int startColumn = column;
            while (startRow > 0 && startColumn > 0)
            {
                startRow--;
                startColumn--;
            }

            for (int r = startRow, c = startColumn; r <= Math.Min(Rows - 4, startRow + 3) && c <= Math.Min(Columns - 4, startColumn + 3); r++, c++)
            {
                if (board[r, c] == player &&
                    board[r + 1, c + 1] == player &&
                    board[r + 2, c + 2] == player &&
                    board[r + 3, c + 3] == player)
                {
                    return true;
                }
            }

            // Check diagonal (right to bottom )
            startRow = row;
            startColumn = column;
            while (startRow < Rows - 1 && startColumn > 0)
            {
                startRow++;
                startColumn--;
            }

            for (int r = startRow, c = startColumn; r >= Math.Max(3, startRow - 3) && c <= Math.Min(Columns - 4, startColumn + 3); r--, c++)
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
        private bool IsBoardFull() //Checks if the game board is full
        {
            for (int c = 0; c < Columns; c++)
            {
                if (board[0, c] == '*')
                {
                    return false;
                }
            }
            return true;
        }

        private void Restart() // Restarts the game based on the user's choice
        {
            int restart = 0;
            while (restart != 1 && restart != 2) //Ensures to get the right input
            {
                Console.WriteLine("Do you want to play again?\n(Select 1 for 'Yes' or 2 for 'No')");
                int.TryParse(Console.ReadLine(), out restart);
            }

            if (restart == 1) //Game restarts
            {
                board = new char[Rows, Columns];
                CurrentPlayer = Player1;
                Console.Clear();
                InitializeBoard();
                StartGame();
            }
            else
            {
                Console.WriteLine("Game Over. Thank you for playing."); //Game ends
            }
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to ConnectFour!!!");

            Console.WriteLine("Game mode: ");
            Console.WriteLine("Select 1 for two Players game mode");
            Console.WriteLine("Select 2 to play with an AI");

            // User gets the opportunity to choose if they want to play with someone or with the computer
            int gameMode = 0;
            while (gameMode != 1 && gameMode != 2)
            {
                Console.WriteLine("Please choose game mode (1 or 2): ");
                int.TryParse(Console.ReadLine(), out gameMode);
            }

            if (gameMode == 1)
            {
                Console.WriteLine("Enter player 1's name: ");
                string player1Name = Console.ReadLine();
                Player1 = new HumanPlayer(player1Name, "X"); // Player1: name and symbol ----player vs. player

                Console.WriteLine("Enter player 2's name: ");
                string player2Name = Console.ReadLine();
                Player2 = new HumanPlayer(player2Name, "O"); // Player2: name and symbol ----player vs. player
            }
            else
            {
                Console.WriteLine("Enter player's name: ");
                string player1Name = Console.ReadLine();
                Player1 = new HumanPlayer(player1Name, "X"); // Player1: name and symbol ----player vs. computer
                string player2Name = "AI Player";
                Player2 = new AI_Player(player2Name, "O"); // Player2: name(computer) and symbol ----player vs. computer
            }
            CurrentPlayer = Player1;

            // Initialize GameOver to false
            bool GameOver = false;

            // While game is not over, players will take turns
            // The game will only end if someone wins or the board becomes full
            // Once someone wins or board's full, player/s will be asked if they want to play again
            while (!GameOver)
            {
                Console.Clear();
                Console.WriteLine("Connect Four Game\n");
                PrintBoard();
                Console.WriteLine($"\nIt is {CurrentPlayerName()}'s turn");

                int move = CurrentPlayer.MakeMove();

                while (InvalidMove(move))
                {
                    Console.WriteLine("Invalid move. Please choose another column (1-7): ");
                    move = CurrentPlayer.MakeMove();
                }


                if (PlacePiece(move))
                {
                    Console.Clear();

                    Console.WriteLine("Connect Four Game\n");
                    PrintBoard();

                    GameOver = true;
                    Console.WriteLine($"\n{CurrentPlayerName()} wins!");
                }
                else if (IsBoardFull())
                {
                    GameOver = true;
                    Console.WriteLine("It's a draw!");
                }
                else
                {
                    if (CurrentPlayer == Player1)
                        CurrentPlayer = Player2;
                    else
                        CurrentPlayer = Player1;
                }
            }

            Restart();
        }

        private string CurrentPlayerName()// Returns the name of the current player

        {
            if (CurrentPlayer == Player1)
            {
                PlayerIndex = true;
                return Player1.Name;
            }
            else
            {
                PlayerIndex = false;
                return Player2.Name;
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

