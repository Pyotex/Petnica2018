using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class Player
    {
        public enum PlayerColor { RED, BLUE }

        public PlayerColor playerColor;
        public float penalty = 1f;
        protected float penaltyTurns = 0;

        public Game game;
        protected Cell theChosenOne;

        public Player(PlayerColor color)
        {
            playerColor = color;
        }

        public virtual void PlayTurn()
        {
            if (game.gameOver)
                return;

            if (theChosenOne != null)
            {
                if (Cell.CellAlive(theChosenOne.cellState) && !game.sacrifice)
                {
                    penaltyTurns++;
                    penalty = Math.Max(Program.minPenalty, penalty - penaltyTurns / 10f);
                }
                else
                {
                    penaltyTurns = 0;
                    penalty = 1f;
                }

                bool finished = theChosenOne.OnCellTap(false);
                theChosenOne = null;

                if (finished)
                    game.FinishMove();
            }
            else
            {
                penaltyTurns++;
                penalty = Math.Max(Program.minPenalty, penalty - penaltyTurns / 10f);

                game.FinishMove();
            }
                
        }

        public void PredictFutureState(Cell cell)
        {
            cell.oldCellState = cell.cellState; // Save the old state
            cell.OnCellTap(true); // Press on the cell

            game.CheckFutureGameStatus(cell);

            cell.cellState = cell.oldCellState;
            game.CalculateNextForAllCells(game.cells);
            game.CheckGameStatus();
        }

        public bool ShouldBePunished()
        {
            if (!Program.penaltyActive)
                return false;

            if (game.rnd.NextDouble() < penalty)
                return false;

            return true;
        }
    }
}
