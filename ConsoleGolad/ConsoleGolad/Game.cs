using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleGolad
{
    class Game
    {
        public int rows, columns;
        public Cell[,] cells;

        private Player redPlayer;
        private Player bluePlayer;

        public Player currentPlayer;

        public bool sacrifice;
        public bool finishedMove;
        public bool gameOver;
        public Cell newCell;

        public int redCells = 0;
        public int blueCells = 0;

        private Random rnd;

        public Game(int rows, int columns, Player redPlayer, Player bluePlayer)
        {
            rnd = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

            this.rows = rows;
            this.columns = columns;
            this.redPlayer = redPlayer;
            this.bluePlayer = bluePlayer;

            this.redPlayer.game = this;
            this.bluePlayer.game = this;

            currentPlayer = redPlayer;

            cells = new Cell[rows, columns];
        }

        public void StartGame()
        {
            SpawnCells();
            CalculateNextForAllCells();
            CheckGameStatus();

            currentPlayer.PlayTurn();
        }

        void SpawnCells()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns / 2; j++)
                {
                    Cell cell = new Cell(j, i, rows, columns, this);

                    cells[i, j] = cell;

                    cell.cellState = RandomState();
                }
            }

            int newRow = 0;
            int newCol = columns / 2;

            for (int i = rows - 1; i >= 0; i--)
            {
                newCol = columns / 2;

                for (int j = columns / 2 - 1; j >= 0; j--)
                {

                    Cell cell = new Cell(newCol, newRow, rows, columns, this);

                    cells[newRow, newCol] = cell;

                    cell.cellState = ReverseState(cells[i, j].cellState);

                    cell.x = newCol;
                    cell.y = newRow;

                    newCol++;
                }

                newRow++;
            }

            PrintCellGrid();
        }

        public void FinishMove()
        {
            if (gameOver)
                return;

            PrintCellGrid();
            //Thread.Sleep(500);

            currentPlayer = currentPlayer == redPlayer ? bluePlayer : redPlayer;

            finishedMove = false;

            NextStep();
            currentPlayer.PlayTurn();
        }

        void NextStep()
        {
            foreach (Cell cell in cells)
                cell.NextState();

            CalculateNextForAllCells();

            CheckGameStatus();
        }

        void CheckGameStatus()
        {
            redCells = 0;
            blueCells = 0;

            foreach (Cell cell in cells)
            {
                if (cell.cellState == Cell.CellState.BLUE)
                    blueCells++;
                else if (cell.cellState == Cell.CellState.RED)
                    redCells++;
            }

            if (redCells == 0 && blueCells == 0)
                GameOver();
            else if (redCells <= 0)
                GameOver(Player.PlayerColor.BLUE);
            else if (blueCells <= 0)
                GameOver(Player.PlayerColor.RED);
        }

        public void CalculateNextForAllCells()
        {
            foreach (Cell cell in cells)
                cell.CalculateNextState();
        }

        Cell.CellState RandomState()
        {
            if (rnd.Next(0, 100) > 50)
            {
                if (rnd.Next(0, 100) > 75)
                    return Cell.CellState.RED;
                else
                    return Cell.CellState.BLUE;
            }
            return Cell.CellState.DEAD;
        }

        Cell.CellState ReverseState(Cell.CellState color)
        {
            if (color == Cell.CellState.BLUE)
                return Cell.CellState.RED;

            else if (color == Cell.CellState.RED)
                return Cell.CellState.BLUE;

            return Cell.CellState.DEAD;
        }

        public void GameOver()
        {
            PrintCellGrid();

            gameOver = true;
            Console.WriteLine("It's a draw!");
        }

        public void GameOver(Player.PlayerColor color)
        {
            PrintCellGrid();

            gameOver = true;
            if (color == Player.PlayerColor.BLUE)
            {
                Console.WriteLine("Blue player won!");
            }
            else
            {
                Console.WriteLine("Red player won!");
            }
        }

        public void PrintCellGrid()
        {
            Console.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(cells[i, j].cellState + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
