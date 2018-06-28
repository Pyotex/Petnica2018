using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class RndCreatePlayer : Player
    {

        public RndCreatePlayer(PlayerColor color) : base(color)
        {
            playerColor = color;
        }

        public override void PlayTurn()
        {
            ArrayList deadCells = new ArrayList();
            ArrayList myAliveCells = new ArrayList();

            foreach (Cell cell in game.cells)
            {
                if (cell.cellState == Cell.CellState.DEAD && cell.nextCellState == Cell.CellState.DEAD)
                    deadCells.Add(cell);

                if (Game.TurnMatchesColor(playerColor, cell.cellState))
                    myAliveCells.Add(cell);
            }


            if (myAliveCells.Count >= 2)
            {
                theChosenOne = (Cell)deadCells[game.rnd.Next(deadCells.Count)];
                base.PlayTurn();

                theChosenOne = (Cell)myAliveCells[game.rnd.Next(myAliveCells.Count)];
                myAliveCells.Remove(theChosenOne);

                base.PlayTurn();

                theChosenOne = (Cell)myAliveCells[game.rnd.Next(myAliveCells.Count)];
                myAliveCells.Remove(theChosenOne);

                base.PlayTurn();
            }
            else
            {
                theChosenOne = (Cell)myAliveCells[0];
                base.PlayTurn();
            }
                
        }
    }
}
