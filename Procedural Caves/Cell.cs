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
        public bool flaggedAsCavern = false;
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

            if(isWall)
            {
                if(liveNeighbours < 4)
                    isWallNext = false;
                else
                    isWallNext = true;
            }
            if(!isWall)
            {
                if(liveNeighbours > 4)
                    isWallNext = true;
                else
                    isWallNext = false;
            }

/*            if(liveNeighbours == 4)
            {
                isWallNext = isWall;
            } else if(liveNeighbours > 4)
            {
                isWallNext = true;
            } else
            {
                isWallNext = false;
            }*/
        }

        public List<Cell> GetFloorNeighbours()
        {
            List<Cell> floorNeighbours = new List<Cell>();
            foreach(var cell in neighbours)
            {
                if (!cell.isWall && !cell.flaggedAsCavern)
                {
                    floorNeighbours.Add(cell);
                    cell.flaggedAsCavern = true;
                }
                    
            }
            return floorNeighbours;
        }

        public void Advance()
        {
            if(flaggedAsCavern)
            {
                isWall = true;
                flaggedAsCavern = false;
            } else
            {
                isWall = isWallNext;
            }
            
        }
    }
}
