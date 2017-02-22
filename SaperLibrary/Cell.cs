﻿using System;

namespace SaperLibrary
{
    [Serializable]
    public class Cell
    {
        private int m_bombsInNeighbourhood;

        public int BombsInNeighbourhood
        {
            get
            {
                if (ContainsBomb)
                    throw new InvalidOperationException();

                return m_bombsInNeighbourhood;
            }
            internal set
            {
                if (ContainsBomb)
                    throw new InvalidOperationException();

                if (value < 0 || value > 8)
                    throw new ArgumentOutOfRangeException();

                m_bombsInNeighbourhood = value;
            }
        }

        public bool ContainsBomb { get; internal set; }

        public bool IsOpen { get; set; }

        public bool IsFlagged { get; set; }

        public override string ToString()
        {
            return ContainsBomb ? "X" : BombsInNeighbourhood.ToString();
        }
    }
}