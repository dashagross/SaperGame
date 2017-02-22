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

        bool m_suppressNotifications;

        public ViewModel(int cols, int rows, int bombs)
        {
            Field = new BombField(cols, rows);
            Field.PlaceBombs(bombs);

            FieldImage = BitmapFactory.New((int)(cols * CellSize.Width), (int)(rows * CellSize.Height));

            drawAllCells();
        }

        public void OpenCell(int x, int y)
        {

            Field[x, y].IsOpen = true;
            DrawCell(x, y);

        }

        public void OpenCellsNearEmpty(int x, int y)
        {
            if (!Field[x, y].ContainsBomb && Field[x, y].BombsInNeighbourhood == 0)
            {
                bool isLastLine = (y == Field.Height - 1);
                if (x == 0)
                {
                    if (y == 0)
                        drawAroundEmtyCell(0, 1, 0, 1); // left top corner
                    else if (isLastLine)
                        drawAroundEmtyCell(0, 1, y - 1, y); // left down corner
                    else
                        drawAroundEmtyCell(0, 1, y - 1, y + 1); // left wall
                }
                else if (x == Field.Width - 1)
                {
                    if (y == 0)
                        drawAroundEmtyCell(x - 1, x, 0, 1); // right top corner
                    else if (isLastLine)
                        drawAroundEmtyCell(x - 1, x, y - 1, y); // right down corner
                    else
                        drawAroundEmtyCell(x - 1, x, y - 1, y + 1); // right wall
                }
                else
                {
                    if (y == 0)
                        drawAroundEmtyCell(x - 1, x + 1, 0, 1); // top wall
                    else if (isLastLine)
                        drawAroundEmtyCell(x - 1, x + 1, y - 1, y); // down wall
                    else
                        drawAroundEmtyCell(x - 1, x + 1, y - 1, y + 1); // center
                }
            }
        }

        void drawAroundEmtyCell(int min_x, int max_x, int min_y, int max_y)
        {
            for (int x = min_x; x <= max_x; ++x)
                for (int y = min_y; y <= max_y; ++y)
                {
                    if (!Field[x, y].IsOpen)
                    {
                        OpenCell(x, y);
                        OpenCellsNearEmpty(x, y);
                    }
                }
        }

        public void FlagCell(int x, int y)
        {
            Field[x, y].IsFlagged = !Field[x, y].IsFlagged;
            DrawCell(x, y);
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
                if (cell.IsFlagged)
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
    }
}

