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

        public Game game;

        private Random rnd = new Random();

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

            Cell theChosenOne = (Cell) aliveCells[rnd.Next(aliveCells.Count)];

            bool finished = theChosenOne.OnCellTap();
            if (!finished)
                PlayTurn();
            else
                game.FinishMove();
        }
    }
}
