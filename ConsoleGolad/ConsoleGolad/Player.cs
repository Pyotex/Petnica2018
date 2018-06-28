﻿using System;
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

            if (Cell.CellAlive(theChosenOne.cellState) && !game.sacrifice)
            {
                penaltyTurns++;
                penalty = Math.Max(0.75f, penalty - penaltyTurns / 100f);
            }
            else
            {
                penaltyTurns = 0;
                penalty = 1f;
            }

            bool finished = theChosenOne.OnCellTap();
            theChosenOne = null;

            if (finished)
                game.FinishMove();
        }

        public bool shouldBePunished()
        {
            if (!Program.penaltyActive)
                return false;

            if (game.rnd.NextDouble() < penalty)
                return false;

            return true;
        }
    }
}
