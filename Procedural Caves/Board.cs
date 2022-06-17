using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedural_Caves
{
    /// <summary>
    /// Board data structure containing cells to a defined size
    /// </summary>
    class Board
    {
        public readonly Cell[,] cells;
        public readonly int cellSize;

        public int columns { get { return cells.GetLength(0); } }
        public int rows { get { return cells.GetLength(1); } }
        public int width { get { return columns * cellSize; } }
        public int height { get { return rows * cellSize; } }

        public Board(int width, int height, int cellSize, bool wrap = true)
        {
            this.cellSize = cellSize;
            cells = new Cell[width / cellSize, height / cellSize];
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    cells[x, y] = new Cell();

            ConnectNeighbours(false);
        }

        readonly Random rand = new Random();
        /// <summary>
        /// Initialises the board with random states in each cell
        /// </summary>
        /// <param name="liveDensity"></param>
        public void RandomInit(double liveDensity)
        {
            foreach (var cell in cells)
                cell.isWall = rand.NextDouble() < liveDensity;
        }

        /// <summary>
        /// Determines the next state, and advances the state of the board - done separately so that the cells will determine their
        /// state based on the current board states, not on the changing states
        /// </summary>
        public void Advance()
        {
            foreach (var cell in cells)
                cell.NextState();
            foreach (var cell in cells)
                cell.Advance();
        }

        /// <summary>
        /// Connects cells to their 8 neighbours (wrapping around the edge of the screen if enabled)
        /// </summary>
        /// <param name="wrap"></param>
        private void ConnectNeighbours(bool wrap)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    bool isLeftEdge = (x == 0);
                    bool isRightEdge = (x == columns - 1);
                    bool isTopEdge = (y == 0);
                    bool isBottomEdge = (y == rows - 1);
                    bool isEdge = isLeftEdge | isRightEdge | isTopEdge | isBottomEdge;

                    if ((wrap == false) && isEdge)
                        continue;

                    int xL = isLeftEdge ? columns - 1 : x - 1;
                    int xR = isRightEdge ? 0 : x + 1;
                    int yT = isTopEdge ? rows - 1 : y - 1;
                    int yB = isBottomEdge ? 0 : y + 1;

                    cells[x, y].neighbours.Add(cells[xL, yT]);
                    cells[x, y].neighbours.Add(cells[x, yT]);
                    cells[x, y].neighbours.Add(cells[xR, yT]);
                    cells[x, y].neighbours.Add(cells[xL, y]);
                    cells[x, y].neighbours.Add(cells[xR, y]);
                    cells[x, y].neighbours.Add(cells[xL, yB]);
                    cells[x, y].neighbours.Add(cells[x, yB]);
                    cells[x, y].neighbours.Add(cells[xR, yB]);
                }
            }
        }
    }
}
