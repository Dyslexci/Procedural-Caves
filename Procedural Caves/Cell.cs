using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedural_Caves
{
    /// <summary>
    /// Data container for each cell object
    /// </summary>
    class Cell
    {
        public bool isWall;
        public readonly List<Cell> neighbours = new List<Cell>();
        bool isWallNext;

        /// <summary>
        /// Calculates the state of the cell in the next iteration based on simple rules
        /// </summary>
        public void NextState()
        {
            int liveNeighbours = neighbours.Where(x => x.isWall).Count();

            if(neighbours.Count <= 7)
            {
                isWallNext = true;
                return;
            }

            if(liveNeighbours == 4)
            {
                isWallNext = isWall;
            } else if(liveNeighbours > 4)
            {
                isWallNext = true;
            } else
            {
                isWallNext = false;
            }
        }

        public void Advance()
        {
            isWall = isWallNext;
        }
    }
}
