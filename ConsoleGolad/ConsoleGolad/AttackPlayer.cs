using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class AttackPlayer : Player
    {

        public AttackPlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            if (game.gameOver)
                return;

            ArrayList aliveCells = new ArrayList();

            foreach (Cell cell in game.cells)
            {
                if (playerColor == PlayerColor.BLUE)
                {
                    if (cell.cellState == Cell.CellState.RED)
                        aliveCells.Add(cell);
                }
                else
                {
                    if (cell.cellState == Cell.CellState.BLUE)
                        aliveCells.Add(cell);
                }


            }

            Cell theChosenOne = (Cell)aliveCells[game.rnd.Next(aliveCells.Count)];

            if (Cell.CellAlive(theChosenOne.cellState))
            {
                turns++;
                penalty = Math.Max(0.75f, penalty - turns / 100f);
            }

            bool finished = theChosenOne.OnCellTap();
            if (!finished)
                PlayTurn();
            else
                game.FinishMove();
        }

    }
}
