using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(10, 10, new Player(Player.PlayerColor.RED), new Player(Player.PlayerColor.BLUE));

            game.StartGame();

            Console.ReadKey();
        }
    }
}
