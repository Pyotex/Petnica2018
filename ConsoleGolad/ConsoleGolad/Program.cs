using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class Program
    {
        public static Random rnd;

        static void Main(string[] args)
        {
            rnd = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

            int blueWins = 0;
            int redWins = 0;
            int draws = 0;

            for (int i = 0; i < 1000; i++)
            {
                Game game = new Game(10, 10, new RndAttackPlayer(Player.PlayerColor.RED), new RandomPlayer(Player.PlayerColor.BLUE), rnd);
                game.StartGame();

                if (game.blueCells == 0 && game.redCells == 0)
                    draws++;
                else if (game.blueCells == 0)
                    redWins++;
                else if (game.redCells == 0)
                    blueWins++;
            }

            Console.WriteLine("Blue wins: " + blueWins + " , red wins: " + redWins + " , draws: " + draws);

            Console.ReadKey();
        }
    }
}
