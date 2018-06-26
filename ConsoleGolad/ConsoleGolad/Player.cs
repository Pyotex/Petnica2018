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
        public bool isAI;
        public float penalty = 1f;
        private float turns = 0;

        public Game game;

        public Player(PlayerColor color)
        {
            playerColor = color;
        }

        public virtual void PlayTurn()
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

            Cell theChosenOne = (Cell) aliveCells[game.rnd.Next(aliveCells.Count)];

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

        public bool shouldBePunished()
        {
            if (game.rnd.NextDouble() < penalty)
                return false;

            return true;
        }
    }
}
