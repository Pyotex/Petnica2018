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
            ArrayList myAliveCells = new ArrayList();
            ArrayList allAliveCells = new ArrayList();

            if (playerColor == PlayerColor.BLUE)
                myAliveCells = game.aliveBlueCells;
            else
                myAliveCells = game.aliveRedCells;

            allAliveCells.AddRange(game.aliveBlueCells);
            allAliveCells.AddRange(game.aliveRedCells);


            if (myAliveCells.Count < 2)
                theChosenOne = (Cell) allAliveCells[game.rnd.Next(allAliveCells.Count)];
            else
                do
                {
                    theChosenOne = game.cells[game.rnd.Next(game.rows - 1), game.rnd.Next(game.columns - 1)];
                } while (!Cell.CanClick(theChosenOne));

            if (Cell.CellAlive(theChosenOne.cellState))
                base.PlayTurn();
            else if (!Cell.CellAlive(theChosenOne.cellState) && theChosenOne.nextCellState == Cell.CellState.DEAD)
            {
                // Press the dead cell to sacrifice
                base.PlayTurn();

                theChosenOne = (Cell) myAliveCells[game.rnd.Next(myAliveCells.Count)];
                myAliveCells.Remove(theChosenOne);

                base.PlayTurn();

                theChosenOne = (Cell)myAliveCells[game.rnd.Next(myAliveCells.Count)];
                myAliveCells.Remove(theChosenOne);

                base.PlayTurn();
            }
            else
                base.PlayTurn();
        }
    }
}
