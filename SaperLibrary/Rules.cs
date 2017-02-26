using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperLibrary
{
    public class Rules
    {
        BombField m_field;

        public Rules(BombField field)
        {
            m_field = field;
        }

        public void OpenCell(int x, int y)
        {
            if (m_field[x,y].IsFlagged)
            {
                openCellImpl(x, y);
                openCellsNearEmpty(x, y);
            }
        }

        public void ToggleFlag(int x, int y)
        {
            m_field[x, y].IsFlagged = !m_field[x, y].IsFlagged;
        }

        public void OpenCellArea()
        {

        }


        void openCellImpl(int x, int y)
        {
            m_field[x, y].IsOpen = true;
        }

        void openCellsNearEmpty(int x, int y)
        {
            if (!m_field[x, y].ContainsBomb && m_field[x, y].BombsInNeighbourhood == 0)
                m_field.PerformWithNeighbourhood(drawAroundEmptyCell, x, y);
        }

        void drawAroundEmptyCell(int min_x, int max_x, int min_y, int max_y)
        {
            for (int x = min_x; x <= max_x; ++x)
                for (int y = min_y; y <= max_y; ++y)
                {
                    if (!m_field[x, y].IsOpen)
                    {
                        openCellImpl(x, y);
                        openCellsNearEmpty(x, y);
                    }
                }
        }


    }
}
