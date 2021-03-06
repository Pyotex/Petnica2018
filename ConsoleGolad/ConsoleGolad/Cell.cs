﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGolad
{
    class Cell
    {
        public enum CellState { RED, BLUE, DEAD, BORN_READ, BORN_BLUE }

        public bool halfEmpty; // True if the cell is being revived and only one other cell is sacrificed

        public int x, y, rows, columns;
        public int redCells, blueCells; // Game stats after predicting the future state of the board

        public CellState cellState; //Current cell state
        public CellState oldCellState; //Used when predicting the future state of the board to preserve the old cell state
        public CellState nextCellState; // The state after the next GoL iteration

        private Game game;

        public Cell(int x, int y, int rows, int columns, Game game)
        {
            this.x = x;
            this.y = y;
            this.rows = rows;
            this.columns = columns;
            this.game = game;
        }

        public bool OnCellTap(bool dryRun)
        {
            if (game.finishedMove || game.gameOver)
                return false;

            //Disable clicking on cells that will be born in the next turn
            if (cellState == CellState.DEAD && (nextCellState == CellState.RED || nextCellState == CellState.BLUE))
                return false;

            MakeTurn(game.currentPlayer.playerColor, cellState);

            if (dryRun)
                game.finishedMove = false;

            game.CalculateNextForAllCells(game.cells);

            return game.finishedMove;
        }

        public void MakeTurn(Player.PlayerColor color, CellState state)
        {
            if (CellAlive(state))
            {
                if (!game.sacrifice)
                {
                    cellState = CellState.DEAD;
                    game.finishedMove = true;
                }

                else if (Game.TurnMatchesColor(color, state))
                {
                    cellState = CellState.DEAD;

                    if (!game.newCell.halfEmpty)
                    {
                        game.newCell.halfEmpty = true;
                    }
                    else
                    {
                        game.newCell.cellState = state;
                        game.newCell.halfEmpty = false;

                        game.sacrifice = false;
                        game.finishedMove = true;
                    }
                }
            }
            else
            {
                if (!game.sacrifice)
                {
                    if (game.currentPlayer.playerColor == Player.PlayerColor.BLUE)
                        cellState = CellState.BORN_BLUE;
                    else
                        cellState = CellState.BORN_READ;

                    game.newCell = this;
                    game.sacrifice = true;
                }
                else
                {
                    if ((cellState == CellState.BORN_READ || cellState == CellState.BORN_BLUE) && !halfEmpty)
                    {
                        cellState = CellState.DEAD;
                        game.sacrifice = false;
                    }
                    else if (cellState == CellState.DEAD && !game.newCell.halfEmpty)
                    {
                        game.newCell.cellState = CellState.DEAD;
                        game.newCell = this;

                        if (game.currentPlayer.playerColor == Player.PlayerColor.BLUE)
                            cellState = CellState.BORN_BLUE;
                        else
                            cellState = CellState.BORN_READ;
                    }
                }
            }
        }


        public void NextState()
        {
            cellState = nextCellState;
        }

        public int[] GetNeighbourCount()
        {
            int red = 0;
            int blue = 0;
            int neighbours = 0;

            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (j >= 0 && j < columns && i >= 0 && i < rows)
                    {
                        if (CellAlive(game.cells[i, j].cellState) && (j != x || i != y))
                        {
                            neighbours++;
                            if (game.cells[i, j].cellState == CellState.RED)
                                red++;
                            else if (game.cells[i, j].cellState == CellState.BLUE)
                                blue++;
                        }
                    }
                }
            }

            return new int[] { red, blue, neighbours };
        }

        public static bool CellAlive(CellState state)
        {
            if (state == CellState.BLUE || state == CellState.RED)
                return true;

            return false;
        }

        public static bool CanClick(Cell cell)
        {
            if (cell.cellState == CellState.DEAD && cell.nextCellState != CellState.DEAD)
                return false;

            return true;
        }
    }
}
