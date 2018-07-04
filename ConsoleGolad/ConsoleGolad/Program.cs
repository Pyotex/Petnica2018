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

        public static int simNumber = 100;
        public static float minPenalty = 0.5f;
        public static bool penaltyActive = false;
        public static bool randomActive = true;
        public static bool printCellGrid = false;

        static void Main(string[] args)
        {
            rnd = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

            for (int i = 0; i < simNumber; i++)
            {
                Game game = new Game(100, 100, new RandomPlayer(Player.PlayerColor.RED), new RandomPlayer(Player.PlayerColor.BLUE), rnd);
                game.SetupGame();

                Console.WriteLine(game.RunGame());
                Console.WriteLine("Game ran for {0} turns", game.turns);
            }

            Console.ReadKey();
        }
    }
}
