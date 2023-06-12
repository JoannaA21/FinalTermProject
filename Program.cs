using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FinalTermProject
{
    public class ConnectFour
    {
        private int Row = 6;
        private int Column = 7;
        private char[,] board;
        private string Player1;
        private string Player2;
        private char CurrentPlayer;
        public ConnectFour() 
        {
            board = new char[Row, Column];
            Player1 = "";
            Player2 = "";
            CurrentPlayer = 'X';
        }

        public void PrintBoard()
        {
            for (int i = 0; i < Row; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < Column; j++)
                {
                    board[i, j] = '#';
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }

        private void PlayersIdentity()
        {
            Console.WriteLine("Enter player 1's name: ");
            Player1 = Console.ReadLine();

            Console.WriteLine("Enter player 2's name: ");
            Player2 = Console.ReadLine();
        }

        public void StartGame()
        {
            PlayersIdentity();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectFour game = new ConnectFour();

            Console.WriteLine("Connect 4 Game Development Project: ");
           game.StartGame();
           game.PrintBoard();
        }
    }
}
