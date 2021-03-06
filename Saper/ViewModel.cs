﻿using SaperLibrary;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using DifficultyTuple = System.Tuple<int, int, int>;

namespace Saper
{
    public class ViewModel : INotifyPropertyChanged
    {  
        public Difficulty Difficulty { get; set; }
   
        public BombField Field { get; protected set; }

        public WriteableBitmap FieldImage { get; protected set; }

        public TimeSpan Elapsed { get => m_stopwatch.IsEnabled ? m_stopwatch.Elapsed : new TimeSpan(); }

        public int FlagsRemaining => m_rules.FlagsRemaining;

        Rules m_rules;
        Stopwatch m_stopwatch;
        bool m_suppressNotifications;

        public ViewModel()
        {
            Difficulty = Difficulty.Professional;

            m_stopwatch = new Stopwatch();
            m_stopwatch.IntervalElapsed += Stopwatch_IntervalElapsed;   
              
            m_rules = new Rules();
            m_rules.CellChanged += rules_CellChanged;
            m_rules.FlagCountChanged += rules_FlagCountChanged;
            m_rules.GameStarted += rules_GameStarted;
            m_rules.GameEnded += rules_GameEnded;

            Start();
        }

        private void rules_FlagCountChanged(object sender, EventArgs e)
        {
            raisePropertyChanged(nameof(FlagsRemaining));
        }

        public void Start()
        {
            int cols = difficultyDictionary[Difficulty].Item1;
            int rows = difficultyDictionary[Difficulty].Item2;
            int bombs = difficultyDictionary[Difficulty].Item3;
            
            Field = new BombField(cols, rows);
            FieldImage = BitmapFactory.New((int)(cols * CellSize.Width), (int)(rows * CellSize.Height));

            m_stopwatch.Stop();
            m_rules.SetField(Field);
            m_rules.Start(bombs);
        }

        #region Wrapped rules

        public void OpenCell(int x, int y)
        {
            m_stopwatch.Start();
            m_rules.OpenCell(x, y);
        }

        public void ToggleFlag(int x, int y)
        {
            m_rules.ToggleFlag(x, y);
        }

        public void OpenCellArea(int x, int y)
        {
            m_rules.OpenCellNeighbours(x, y);
        }
                
        #endregion

        void blitSprite(int x, int y, CellContentsEnum e)
        {
            FieldImage.Blit(new Point(x * CellSize.Width, y * CellSize.Height),
                              m_skinBitmap,
                              new Rect(spriteDictionary[e], CellSize),
                              Colors.White, WriteableBitmapExtensions.BlendMode.None);
        }

        void drawAllCells()
        {
            m_suppressNotifications = true;

            for (int i = 0; i < Field.Width; i++)
                for (int j = 0; j < Field.Height; j++)
                    DrawCell(i, j);

            m_suppressNotifications = false;

            raisePropertyChanged(nameof(FieldImage));
        }

        public void DrawCell(int x, int y)
        {
            var cell = Field[x, y];
            CellContentsEnum e;

            if (!cell.IsOpen)
            {
                if (cell.IncorrectFlag)
                    e = CellContentsEnum.IncorrectFlag;
                else if (cell.IsFlagged)
                    e = CellContentsEnum.Flag;
                else
                    e = CellContentsEnum.Closed;
            }
            else if (cell.ContainsBomb)
                e = CellContentsEnum.Bomb;
            else
                switch (cell.BombsInNeighbourhood)
                {
                    case 0: e = CellContentsEnum.Empty; break;
                    case 1: e = CellContentsEnum.One;   break;
                    case 2: e = CellContentsEnum.Two;   break;
                    case 3: e = CellContentsEnum.Three; break;
                    case 4: e = CellContentsEnum.Four;  break;
                    case 5: e = CellContentsEnum.Five;  break;
                    case 6: e = CellContentsEnum.Six;   break;
                    case 7: e = CellContentsEnum.Seven; break;
                    case 8: e = CellContentsEnum.Eight; break;
                    default:
                        throw new InvalidOperationException("Value of a cell is out of range (from 0 to 8)");
                }

            blitSprite(x, y, e);

            if (!m_suppressNotifications)
                raisePropertyChanged(nameof(FieldImage));
        }

        #region Skin

        public Size CellSize { get; } = new Size(24, 24);
        WriteableBitmap m_skinBitmap = BitmapFactory.New(0, 0).FromResource("Images\\CellSprites.png");

        Dictionary<CellContentsEnum, Point> spriteDictionary = new Dictionary<CellContentsEnum, Point>()
        {
            { CellContentsEnum.Closed, new Point(0, 0)  },
            { CellContentsEnum.Flag,   new Point(24, 0) },
            { CellContentsEnum.IncorrectFlag, new Point(48, 0) },
            { CellContentsEnum.Bomb,  new Point(72, 0) },
            { CellContentsEnum.Empty, new Point(96, 0) },
            { CellContentsEnum.One,   new Point(0, 24) },
            { CellContentsEnum.Two,   new Point(24, 24) },
            { CellContentsEnum.Three, new Point(48, 24) },
            { CellContentsEnum.Four,  new Point(72, 24) },
            { CellContentsEnum.Five,  new Point(96, 24) },
            { CellContentsEnum.Six,   new Point(0, 48)  },
            { CellContentsEnum.Seven, new Point(24, 48) },
            { CellContentsEnum.Eight, new Point(48, 48) },
        };

        #endregion

        Dictionary<Difficulty, DifficultyTuple> difficultyDictionary = new Dictionary<Difficulty, DifficultyTuple>()
        {
            { Difficulty.Beginner,     new DifficultyTuple (9, 9, 10)   },
            { Difficulty.Amateur,      new DifficultyTuple (16, 16, 40) },
            { Difficulty.Professional, new DifficultyTuple (30, 16, 99) }
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void raisePropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void rules_CellChanged(object sender, CellChangedEventArgs arg)
        {
            DrawCell(arg.x, arg.y);
        }

        private void rules_GameStarted(object sender, EventArgs e)
        {
            raisePropertyChanged(nameof(Elapsed));
            drawAllCells();
        }

        private void rules_GameEnded(object sender, GameEndedEventArgs arg)
        {
            m_stopwatch.Stop();
            switch (arg.e)
            {
                case GameEndStates.Win:
                    var scores = ScoreProvider.LoadScores(Difficulty);
                    var elapsed = m_stopwatch.Elapsed;
                    addScore(scores, elapsed);
                    ScoreProvider.SaveScores(scores, Difficulty);
                    var scoresDialog = new Scores(elapsed, scores);
                    bool? scores_result = scoresDialog.ShowDialog();
                    break;

                case GameEndStates.Lose:
                    var gameOver = new GameOver();
                    bool? gameOver_result = gameOver.ShowDialog();
                    break;
            }
        }

        private void addScore(List<ScoreItem> scores, TimeSpan elapsed)
        {
            var bestItem = (scores.Count < 10) ? null : scores.Max();

            if (bestItem == null || elapsed < bestItem.Score)
            {
                scores.Remove(bestItem);
                scores.Add(new ScoreItem { Date = DateTime.Now, Score = elapsed });
            }
        }

        private void Stopwatch_IntervalElapsed(object sender, EventArgs e)
        {
            raisePropertyChanged(nameof(Elapsed));
        }
    }
}

