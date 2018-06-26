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

            for (int i = 0; i < 150; i++)
            {
                Game game = new Game(10, 10, new Player(Player.PlayerColor.RED), new Player(Player.PlayerColor.BLUE), rnd);
                game.StartGame();
            }

            Console.ReadKey();
        }
    }
}
