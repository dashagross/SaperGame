using SaperLibrary;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Saper
{
    public class ViewModel : INotifyPropertyChanged
    {
        public BombField Field { get; }

        public WriteableBitmap FieldImage { get; }

        public Rules Rules { get; }

        bool m_suppressNotifications;

        public ViewModel(int cols, int rows, int bombs)
        {
            Field = new BombField(cols, rows);
            Field.PlaceBombs(bombs);

            FieldImage = BitmapFactory.New((int)(cols * CellSize.Width), (int)(rows * CellSize.Height));

            Rules = new Rules(Field);
            Rules.CellChanged += Rules_CellChanged;
            
            drawAllCells();
        }

        void blitSprite(int x, int y, CellContentsEnum e)
        {
            FieldImage.Blit(new Point(x * CellSize.Width, y * CellSize.Height),
                              m_skinBitmap,
                              new Rect(m_imagesDictionary[e], CellSize),
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

        Dictionary<CellContentsEnum, Point> m_imagesDictionary = new Dictionary<CellContentsEnum, Point>()
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void Rules_CellChanged(object sender, CellChangedEventArgs arg)
        {
            DrawCell(arg.x, arg.y);
        }
    }
}

