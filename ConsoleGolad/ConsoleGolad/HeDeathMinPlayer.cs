using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class HeDeathMinPlayer : Player // This player tries to minimize his own cells' deaths
    {
        public HeDeathMinPlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            int cellDeaths = Int32.MaxValue;

            foreach (Cell cell in game.cells)
            {
                if (cell.cellState == Cell.CellState.DEAD)
                    continue;

                PredictFutureState(cell);
                
                if (playerColor == PlayerColor.BLUE)
                {
                    if (game.blueCells - cell.blueCells < cellDeaths)
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
