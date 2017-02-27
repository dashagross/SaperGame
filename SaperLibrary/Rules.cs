﻿using System;


namespace SaperLibrary
{
    //public delegate void CellChangedEventHandler(object sender, CellChangedEventArgs arg);

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
                int count = 0;
                m_field.PerformWithNeighbourhood((u, v) => { if (m_field[u, v].IsFlagged) ++count; }, x, y);
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

        public event EventHandler<CellChangedEventArgs> CellChanged;

        protected void raiseCellChanged(int x, int y)
        {
            CellChanged?.Invoke(this, new CellChangedEventArgs(x, y));
        }
    }
}