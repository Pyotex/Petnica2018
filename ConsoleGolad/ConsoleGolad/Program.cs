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

        public static int simNumber = 10000;
        public static float minPenalty = 0.25f;
        public static bool penaltyActive = false;
        public static bool randomActive = true;
        public static bool printCellGrid = false;

        static void Main(string[] args)
        {
            rnd = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

            int blueWins = 0;
            int redWins = 0;
            int draws = 0;

            for (int i = 0; i < simNumber; i++)
            {
                Game game = new Game(10, 10, new HeDeathRatioPlayer(Player.PlayerColor.RED), new HeDeathMaxPlayer(Player.PlayerColor.BLUE), rnd);
                game.StartGame();

                if (game.blueCells == 0 && game.redCells == 0)
                    draws++;
                else if (game.blueCells == 0)
                    redWins++;
                else if (game.redCells == 0)
                    blueWins++;
            }

            Console.WriteLine("Red wins: " + redWins + " , blue wins: " + blueWins + " , draws: " + draws);

            Console.ReadKey();
        }
    }
}
