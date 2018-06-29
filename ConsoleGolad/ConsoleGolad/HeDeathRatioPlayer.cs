using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class HeDeathRatioPlayer : Player // This player tries to maximize the ratio between his and opponent's alive cells count
    {
        public HeDeathRatioPlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            double ratio = 0;

            foreach (Cell cell in game.cells)
            {
                if (cell.cellState == Cell.CellState.DEAD)
                    continue;

                PredictFutureState(cell);

                if (playerColor == PlayerColor.RED)
                {
                    if ((double) cell.redCells / cell.blueCells > ratio)
                    {
                        ratio = (double) cell.redCells / cell.blueCells;
                        theChosenOne = cell;
                    }
                }
                else
                {
                    if ((double) cell.blueCells / cell.redCells > ratio)
                    {
                        ratio = (double) cell.blueCells / cell.redCells;
                        theChosenOne = cell;
                    }
                }
            }

            base.PlayTurn();
        }
    }
}
