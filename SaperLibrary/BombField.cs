using System;
using System.Linq;
using System.Text;

using cellAreaTuple = System.Tuple<int, int, int, int>;

namespace SaperLibrary
{
    public delegate void NeighbourhoodAction(int x, int y);

    public class BombField
    {
        #region Cell access

        public int Width { get { return m_cells.GetLength(0); } }
        public int Height { get { return m_cells.GetLength(1); } }

        public Cell this[int col, int row] { get { return m_cells[col, row]; } }

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
            var bombArray = new int[count];
            var r = new Random();

            for (int k = 0; k < count; k++)
            {
                bombArray[k] = r.Next(m_cells.Length);

                if (bombArray.Take(k).Contains(bombArray[k]))
                    --k;
                else
                    m_cells[bombArray[k] % m_cells.GetLength(0), bombArray[k] / m_cells.GetLength(0)].ContainsBomb = true;
            }

            Fill();

        }

        void countCell(int x, int y)
        {
            if (!m_cells[x, y].ContainsBomb) ++m_cells[x, y].BombsInNeighbourhood;
        }

        void FillCell(int i, int j)
        {
            if (m_cells[i, j].ContainsBomb)
                PerformWithNeighbourhood(countCell, i, j);
        }

        void Fill()
        {
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++)
                {
                    FillCell(i, j);
                }

            }
        }

        public void PerformWithNeighbourhood(NeighbourhoodAction callback, int i, int j)
        {
            bool isLastLine = (j == m_cells.GetLength(1) - 1);

            cellAreaTuple cellArea;
            
            if (i == 0)
            {
                if (j == 0)
                    cellArea = new cellAreaTuple(0, 1, 0, 1);         // left top corner
                else if (isLastLine)
                    cellArea = new cellAreaTuple(0, 1, j - 1, j);     // left down corner
                else
                    cellArea = new cellAreaTuple(0, 1, j - 1, j + 1); // left wall
            }
            else if (i == m_cells.GetLength(0) - 1)
            {
                if (j == 0)
                    cellArea = new cellAreaTuple(i - 1, i, 0, 1);         // right top corner
                else if (isLastLine)
                    cellArea = new cellAreaTuple(i - 1, i, j - 1, j);     // right down corner
                else
                    cellArea = new cellAreaTuple(i - 1, i, j - 1, j + 1); // right wall
            }
            else
            {
                if (j == 0)
                    cellArea = new cellAreaTuple(i - 1, i + 1, 0, 1);         // top wall
                else if (isLastLine)
                    cellArea = new cellAreaTuple(i - 1, i + 1, j - 1, j);     // down wall
                else
                    cellArea = new cellAreaTuple(i - 1, i + 1, j - 1, j + 1); // center
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
