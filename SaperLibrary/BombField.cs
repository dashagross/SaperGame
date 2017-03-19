using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CellAreaTuple = System.Tuple<int, int, int, int>;

namespace SaperLibrary
{
    public delegate void NeighbourhoodAction(int x, int y);

    public class BombField
    {
        #region Cell access

        public int Width => m_cells.GetLength(0);
        public int Height => m_cells.GetLength(1);

        int m_bombs;
        public int Bombs
        {
            get => m_bombs;
            set => m_bombs = value;
        }

        public Cell this[int col, int row] => m_cells[col, row];

        #endregion

        Cell[,] m_cells;

        public BombField(int cols, int rows)
        {
            m_cells = new Cell[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    m_cells[i, j] = new Cell();
                }
            }
        }

        public void PlaceBombs(int count)
        {
            var bombSet = new HashSet<int>();
            var r = new Random();
            var w = Width;
            var h = Height;

            foreach (var cell in m_cells)
                cell.Reset();

            do
            {
                int k = r.Next(w * h);
                if (bombSet.Add(k))
                    placeBomb(k % w, k / w);
            }
            while (bombSet.Count < count);
        }

        void placeBomb(int i, int j)
        {
            m_cells[i, j].ContainsBomb = true;
            PerformWithNeighbourhood(countCell, i, j);
            m_bombs++;
        }

        void countCell(int x, int y)
        {
            if (!m_cells[x, y].ContainsBomb)
                ++m_cells[x, y].BombsInNeighbourhood;
        }
        
        public void PerformWithNeighbourhood(NeighbourhoodAction callback, int i, int j)
        {
            bool isLastLine = (j == m_cells.GetLength(1) - 1);

            CellAreaTuple cellArea;
            
            if (i == 0)
            {
                if (j == 0)
                    cellArea = new CellAreaTuple(0, 1, 0, 1);         // left top corner
                else if (isLastLine)
                    cellArea = new CellAreaTuple(0, 1, j - 1, j);     // left down corner
                else
                    cellArea = new CellAreaTuple(0, 1, j - 1, j + 1); // left wall
            }
            else if (i == m_cells.GetLength(0) - 1)
            {
                if (j == 0)
                    cellArea = new CellAreaTuple(i - 1, i, 0, 1);         // right top corner
                else if (isLastLine)
                    cellArea = new CellAreaTuple(i - 1, i, j - 1, j);     // right down corner
                else
                    cellArea = new CellAreaTuple(i - 1, i, j - 1, j + 1); // right wall
            }
            else
            {
                if (j == 0)
                    cellArea = new CellAreaTuple(i - 1, i + 1, 0, 1);         // top wall
                else if (isLastLine)
                    cellArea = new CellAreaTuple(i - 1, i + 1, j - 1, j);     // down wall
                else
                    cellArea = new CellAreaTuple(i - 1, i + 1, j - 1, j + 1); // center
            }

            for (int x = cellArea.Item1; x <= cellArea.Item2; ++x)
                for (int y = cellArea.Item3; y <= cellArea.Item4; ++y)
                    callback(x, y);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++)
                {
                    sb.Append(m_cells[j, i]);
                    sb.Append(' ');
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
