using SaperLibrary;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Rules Rules { get; }

        bool m_suppressNotifications;

        public ViewModel()
        {
            Difficulty = Difficulty.Amateur;
              
            Rules = new Rules();
            Rules.CellChanged += Rules_CellChanged;
            Rules.GameStarted += Rules_GameStarted;

            Start();

            drawAllCells();
        }

        public void Start()
        {
            int cols = difficultyDictionary[Difficulty].Item1;
            int rows = difficultyDictionary[Difficulty].Item2;
            int bombs = difficultyDictionary[Difficulty].Item3;
            
            Field = new BombField(cols, rows);
            FieldImage = BitmapFactory.New((int)(cols * CellSize.Width), (int)(rows * CellSize.Height));
            Rules.SetField(Field);
            Rules.Start(bombs);
        }

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

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FieldImage"));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FieldImage"));
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

        private void Rules_CellChanged(object sender, CellChangedEventArgs arg)
        {
            DrawCell(arg.x, arg.y);
        }

        private void Rules_GameStarted(object sender, EventArgs e)
        {
            drawAllCells();
        }
    }
}

