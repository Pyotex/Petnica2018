using System;
using System.Collections;
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
            ArrayList myAliveCells = new ArrayList();

            Cell sacrifice1 = null, sacrifice2 = null; // Had to initialize them....

            foreach (Cell cell in game.cells)
                if (Game.TurnMatchesColor(playerColor, cell.cellState))
                    myAliveCells.Add(cell);

            foreach (Cell cell in game.cells)
            {
                if (!Cell.CanClick(cell))
                    continue;

                if (cell.cellState != Cell.CellState.DEAD)
                    PredictFutureState(cell);
                else if (myAliveCells.Count > 1)
                {
                    sacrifice1 = (Cell)myAliveCells[game.rnd.Next(myAliveCells.Count)];
                    myAliveCells.Remove(sacrifice1);

                    sacrifice2 = (Cell)myAliveCells[game.rnd.Next(myAliveCells.Count)];
                    myAliveCells.Remove(sacrifice2);

                    PredictFutureState(cell, sacrifice1, sacrifice2);
                }
                else
                    continue;

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

            if (theChosenOne.cellState != Cell.CellState.DEAD)
                base.PlayTurn();
            else
            {
                var temp = theChosenOne;

                theChosenOne = sacrifice1;
                base.PlayTurn();

                theChosenOne = sacrifice2;
                base.PlayTurn();

                theChosenOne = temp;
                base.PlayTurn();
            }
        }
    }
}
