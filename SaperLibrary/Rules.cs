using System;

namespace SaperLibrary
{
    public class Rules
    {
        BombField m_field;

        int m_openCellCount;
        
        public void SetField(BombField field)
        {
            m_field = field;
        }

        public void Start(int bombs)
        {
            m_field.PlaceBombs(bombs);
            m_openCellCount = 0;
            raiseGameStarted();
        }

        public void OpenCell(int x, int y)
        {
            if (!m_field[x, y].IsFlagged && !m_field[x, y].IsOpen)
            {
                m_field[x, y].IsOpen = true;
                m_openCellCount++;

                raiseCellChanged(x, y);

                if (!isGameOver(x, y))
                    openSafeZone(x, y);
            }
        }

        public void ToggleFlag(int x, int y)
        {
            m_field[x, y].IsFlagged = !m_field[x, y].IsFlagged;
            raiseCellChanged(x, y);
        }

        public void OpenCellNeighbours(int x, int y)
        {
            if (m_field[x, y].IsOpen)
            {
                int count = 0;
                m_field.PerformWithNeighbours((u, v) => { if (m_field[u, v].IsFlagged) ++count; }, x, y);
                if (count >= m_field[x, y].BombsInNeighbourhood)
                    m_field.PerformWithNeighbours(OpenCell, x, y);
            }
        }

        void openSafeZone(int x, int y)
        {
            if (!m_field[x, y].ContainsBomb && m_field[x, y].BombsInNeighbourhood == 0)
                m_field.PerformWithNeighbours(OpenCell, x, y);
        }

        bool isGameOver(int x, int y)
        {
            if (!m_field[x, y].IsFlagged && m_field[x, y].ContainsBomb)
            {
                loseGame();
                return true;
            }

            if (m_openCellCount == (m_field.Height * m_field.Width - m_field.Bombs))
            {
                winGame();
                return true;
            }

            return false;
        }

        void loseGame()
        {
            for (int i = 0; i < m_field.Width; ++i)
                for (int j = 0; j < m_field.Height; ++j)
                {
                    if (m_field[i, j].IsFlagged && !m_field[i, j].ContainsBomb)                    
                        m_field[i, j].IncorrectFlag = true;
                    
                    m_field[i, j].IsOpen = true;
                    raiseCellChanged(i, j);
                }
            raiseGameEnded(GameEndStates.Lose);
        }

        void winGame()
        {
            raiseGameEnded(GameEndStates.Win);
        }

        public event EventHandler<CellChangedEventArgs> CellChanged;
        protected void raiseCellChanged(int x, int y)
        {
            CellChanged?.Invoke(this, new CellChangedEventArgs(x, y));
        }

        public event EventHandler GameStarted;
        protected void raiseGameStarted()
        {
            GameStarted?.Invoke(this, null);
        }

        public event EventHandler<GameEndedEventArgs> GameEnded;
        protected void raiseGameEnded(GameEndStates e)
        {
            GameEnded?.Invoke(this, new GameEndedEventArgs(e));
        }        
    }
}
