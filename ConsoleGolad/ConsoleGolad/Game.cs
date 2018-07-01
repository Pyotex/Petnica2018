using System;
using System.Collections;
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

        public ArrayList aliveRedCells = new ArrayList();
        public ArrayList aliveBlueCells = new ArrayList();

        public bool running = false;
        public int winner = 0;

        public Random rnd;

        public Game(int rows, int columns, Player redPlayer, Player bluePlayer, Random rnd)
        {
            this.rows = rows;
            this.columns = columns;
            this.redPlayer = redPlayer;
            this.bluePlayer = bluePlayer;
            this.rnd = rnd;

            this.redPlayer.game = this;
            this.bluePlayer.game = this;

            currentPlayer = redPlayer;

            cells = new Cell[rows, columns];
        }

        public void SetupGame()
        {
            SpawnCells();

            running = true;
        }

        public int RunGame()
        {
            while (running)
            {
                //PrintCellGrid();
                //Thread.Sleep(500);

                Calculations();

                CheckGameStatus();

                if (gameOver)
                    return winner;

                currentPlayer.PlayTurn();
                currentPlayer = currentPlayer == redPlayer ? bluePlayer : redPlayer;

                // GoL iteration
                NextStep();
            }

            // Should never be called, hopefully
            return 0;
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

                    if (Cell.CellAlive(cell.cellState) && cell.cellState == Cell.CellState.RED)
                    {
                        redCells++;
                        aliveRedCells.Add(cell);
                    } else if (Cell.CellAlive(cell.cellState) && cell.cellState == Cell.CellState.BLUE)
                    {
                        blueCells++;
                        aliveBlueCells.Add(cell);
                    }
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

                    newCol++;

                    if (Cell.CellAlive(cell.cellState) && cell.cellState == Cell.CellState.RED)
                    {
                        redCells++;
                        aliveRedCells.Add(cell);
                    }
                    else if (Cell.CellAlive(cell.cellState) && cell.cellState == Cell.CellState.BLUE)
                    {
                        blueCells++;
                        aliveBlueCells.Add(cell);
                    }
                }

                newRow++;
            }
        }

        void Calculations()
        {
            aliveBlueCells.Clear();
            aliveRedCells.Clear();

            redCells = 0;
            blueCells = 0;

            foreach (Cell cell in cells)
            {
                // Calculate temp next state based on current known neighbour count
                CalculateNextState(cell);

                // if the cell is alive, increase the counter for neighbour cells and calculate their temp next state
                if (Cell.CellAlive(cell.cellState))
                {
                    if (cell.cellState == Cell.CellState.RED)
                    {
                        redCells++;
                        aliveRedCells.Add(cell);
                    }
                    else if (cell.cellState == Cell.CellState.BLUE)
                    {
                        blueCells++;
                        aliveBlueCells.Add(cell);
                    }
                }

                // Update neighbour cells
                for (int i = cell.y - 1; i <= cell.y + 1; i++)
                {
                    for (int j = cell.x - 1; j <= cell.x + 1; j++)
                    {
                        if (j >= 0 && j < columns && i >= 0 && i < rows && Cell.CellAlive(cell.cellState))
                        {
                            if (cell.cellState == Cell.CellState.RED)
                                cells[i, j].redNeighbours++;
                            else if (cell.cellState == Cell.CellState.BLUE)
                                cells[i, j].blueNeighbours++;

                            // calculateNextState
                            CalculateNextState(cells[i, j]);
                        }
                    }
                }
            }
        }

        void NextStep()
        {
            foreach (Cell cell in cells)
            {
                cell.NextState();
                cell.blueNeighbours = 0;
                cell.redNeighbours = 0;
            }
        }

        public void CheckGameStatus()
        {
            if (redCells == 0 && blueCells == 0)
                GameOver();
            else if (redCells <= 0)
                GameOver(Player.PlayerColor.BLUE);
            else if (blueCells <= 0)
                GameOver(Player.PlayerColor.RED);
        }

        public void CheckFutureGameStatus(Cell pressedCell)
        {
            foreach (Cell cell in cells)
            {
                if (cell.nextCellState == Cell.CellState.BLUE)
                    pressedCell.blueCells++;
                else if (cell.nextCellState == Cell.CellState.RED)
                    pressedCell.redCells++;
            }
        }

        public void CalculateNextForAllCells(Cell[,] cells)
        {
            foreach (Cell cell in cells)
                CalculateNextState(cell);
        }

        public void CalculateNextState(Cell cell)
        {
            int neighbours = cell.redNeighbours + cell.blueNeighbours;

            if (Cell.CellAlive(cell.cellState))
            {
                if (neighbours > 3 || neighbours < 2)
                    cell.nextCellState = Cell.CellState.DEAD;
                else
                {
                    if (RandomFactor() || (currentPlayer.ShouldBePunished() && TurnMatchesColor(currentPlayer.playerColor, cell.cellState)))
                        cell.nextCellState = Cell.CellState.DEAD;
                    else
                        cell.nextCellState = cell.cellState;
                }
            }

            else
            {
                if (neighbours == 3)
                {
                    if (cell.redNeighbours > cell.blueNeighbours)
                        cell.nextCellState = Cell.CellState.RED;
                    else
                        cell.nextCellState = Cell.CellState.BLUE;
                }
                else
                    cell.nextCellState = Cell.CellState.DEAD;
            }
        }

        bool RandomFactor()
        {
            if (!Program.randomActive)
                return false;

            return rnd.NextDouble() > 0.9f;
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

        public static bool TurnMatchesColor(Player.PlayerColor color, Cell.CellState state)
        {
            if (color == Player.PlayerColor.BLUE && state == Cell.CellState.BLUE)
                return true;

            if (color == Player.PlayerColor.RED && state == Cell.CellState.RED)
                return true;

            return false;
        }

        public void GameOver()
        {
            PrintCellGrid();

            gameOver = true;
            winner = 0;
        }

        public void GameOver(Player.PlayerColor color)
        {
            PrintCellGrid();

            gameOver = true;
            if (color == Player.PlayerColor.BLUE)
                winner = 2;
            else
                winner = 1;
        }

        public void PrintCellGrid()
        {
            if (!Program.printCellGrid)
                return;

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
