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
        protected float turns = 0;

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

            if (Cell.CellAlive(theChosenOne.cellState))
            {
                turns++;
                penalty = Math.Max(0.75f, penalty - turns / 100f);
            }

            bool finished = theChosenOne.OnCellTap();
            //if (!finished)
            //    PlayTurn();
            //else
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
