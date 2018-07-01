using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class Program
    {
        public static Random rnd;

        public static int simNumber = 30000;
        public static float minPenalty = 0.5f;
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

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Game game = new Game(100, 100, new RandomPlayer(Player.PlayerColor.RED), new RandomPlayer(Player.PlayerColor.BLUE), rnd);
                game.StartGame();

                stopwatch.Stop();

                Console.WriteLine(stopwatch.ElapsedMilliseconds);

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
