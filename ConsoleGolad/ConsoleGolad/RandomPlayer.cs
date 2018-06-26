using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class RandomPlayer : Player
    {

        public RandomPlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            ArrayList aliveCells = new ArrayList();

            foreach (Cell cell in game.cells)
                if (cell.cellState == Cell.CellState.RED || cell.cellState == Cell.CellState.BLUE)
                    aliveCells.Add(cell);

            theChosenOne = (Cell)aliveCells[game.rnd.Next(aliveCells.Count)];

            base.PlayTurn();
        }
    }
}
