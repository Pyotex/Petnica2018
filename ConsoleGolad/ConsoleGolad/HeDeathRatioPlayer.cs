using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            double ratio = double.MinValue;
            ArrayList myAliveCells = new ArrayList();

            Cell sacrifice1 = null, sacrifice2 = null; // Had to initialize them....

            foreach (Cell cell in game.cells)
                if (Game.TurnMatchesColor(playerColor, cell.cellState))
                    myAliveCells.Add(cell);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

                if (playerColor == PlayerColor.RED)
                {
                    if ((double)cell.redCells / (cell.blueCells == 0 ? 1 : cell.blueCells) > ratio)
                    {
                        ratio = (double)cell.redCells / cell.blueCells;
                        theChosenOne = cell;
                    }
                }
                else
                {
                    if ((double)cell.blueCells / (cell.redCells == 0 ? 1 : cell.redCells) > ratio)
                    {
                        ratio = (double)cell.blueCells / cell.redCells;
                        theChosenOne = cell;
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine("Ran for {0} milliseconds", stopwatch.ElapsedMilliseconds);

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
