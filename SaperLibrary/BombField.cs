using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperLibrary
{
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

        public void OpenCell(int x, int y)
        {
            m_cells[x, y].IsOpen = true;
        }

        public void FlagCell(int x, int y)
        {
            m_cells[x, y].IsFlagged = true;
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

        void CountCell(int min_i, int max_i, int min_j, int max_j)
        {

            for (int a = Math.Min(min_i, max_i); a <= Math.Max(min_i, max_i); a++)
                for (int b = Math.Min(min_j, max_j); b <= Math.Max(min_j, max_j); b++)
                    if (!m_cells[a, b].ContainsBomb) ++m_cells[a, b].BombsInNeighbourhood;
        }

        void FillCell(int i, int j)
        {
            if (m_cells[i, j].ContainsBomb)
            {
                bool isLastLine = (j == m_cells.GetLength(1) - 1);

                if (i == 0)
                {
                    if (j == 0)
                        CountCell(0, 1, 0, 1); // left top corner
                    else if (isLastLine)
                        CountCell(0, 1, j - 1, j); // left down corner
                    else
                        CountCell(0, 1, j - 1, j + 1); // left wall
                }
                else if (i == m_cells.GetLength(0) - 1)
                {
                    if (j == 0)
                        CountCell(i - 1, i, 0, 1); // right top corner
                    else if (isLastLine)
                        CountCell(i - 1, i, j - 1, j); // right down corner
                    else
                        CountCell(i - 1, i, j - 1, j + 1); // right wall
                }
                else
                {
                    if (j == 0)
                        CountCell(i - 1, i + 1, 0, 1); // top wall
                    else if (isLastLine)
                        CountCell(i - 1, i + 1, j - 1, j); // down wall
                    else
                        CountCell(i - 1, i + 1, j - 1, j + 1); // center
                }
            }

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
