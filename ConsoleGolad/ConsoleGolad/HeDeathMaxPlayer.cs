using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class HeDeathMaxPlayer : Player // This player tries to maximize the death of opponent cells
    {
        public HeDeathMaxPlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            int cellDeaths = Int32.MinValue;

            foreach (Cell cell in game.cells)
            {
                if (cell.cellState == Cell.CellState.DEAD)
                    continue;

                PredictFutureState(cell);

                if (playerColor == PlayerColor.RED)
                {
                    if (game.blueCells - cell.blueCells > cellDeaths)
                    {
                        cellDeaths = game.blueCells - cell.blueCells;
                        theChosenOne = cell;
                    }
                }
                else
                {
                    if (game.redCells - cell.redCells < cellDeaths)
                    {
                        cellDeaths = game.redCells - cell.redCells;
                        theChosenOne = cell;
                    }
                }
            }

            base.PlayTurn();
        }
    }
}
