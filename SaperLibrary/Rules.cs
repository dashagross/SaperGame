using System;


namespace SaperLibrary
{
    public delegate void CellChangedEventHandler(object sender, CellChangedEventArgs arg);

    public class Rules
    {
        BombField m_field;

        public Rules(BombField field)
        {
            m_field = field;
        }

        public void OpenCell(int x, int y)
        {
            openCellImpl(x, y);
            openSafeZone(x, y);
        }

        public void ToggleFlag(int x, int y)
        {
            m_field[x, y].IsFlagged = !m_field[x, y].IsFlagged;
            raiseCellChanged(x, y);
        }

        public void OpenCellArea(int x, int y)
        {
            if (m_field[x, y].IsOpen)
            {
                count = 0;
                m_field.PerformWithNeighbourhood(countFlags, x, y);
                if (count >= m_field[x, y].BombsInNeighbourhood)
                    m_field.PerformWithNeighbourhood(OpenCell, x, y);
            }
        }

        void openCellImpl(int x, int y)
        {
            if (!m_field[x, y].IsFlagged)
            {
                m_field[x, y].IsOpen = true;
                raiseCellChanged(x, y);
            }
        }

        void openSafeZone(int x, int y)
        {
            if (!m_field[x, y].ContainsBomb && m_field[x, y].BombsInNeighbourhood == 0)
                m_field.PerformWithNeighbourhood(openSafeZoneImpl, x, y);
        }

        void openSafeZoneImpl(int x, int y)
        {
            if (!m_field[x, y].IsOpen && !m_field[x, y].IsFlagged)
            {
                openCellImpl(x, y);
                openSafeZone(x, y);
            }
        }

        int count { get; set; }
        void countFlags(int x, int y)
        {
            if (m_field[x, y].IsFlagged)
                count++;
        }

        public event CellChangedEventHandler CellChanged;

        protected void raiseCellChanged(int x, int y)
        {
            CellChangedEventArgs arg = new CellChangedEventArgs();
            arg.x = x;
            arg.y = y;
            CellChanged?.Invoke(this, arg);
        }
    }
}
