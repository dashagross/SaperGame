using SaperLibrary;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Saper
{
    public class ViewModel
    {
        public BombField Field { get; }
        public WriteableBitmap writeableBmp { get; }
        static readonly Size c_CellSize = new Size(24, 24);

        public ViewModel(int cols, int rows, int bombs)
        {
            Field = new BombField(cols, rows);
            Field.PlaceBombs(bombs);

            writeableBmp = BitmapFactory.New((int)(cols * c_CellSize.Width), (int)(rows * c_CellSize.Height));
            using (var ctx = writeableBmp.GetBitmapContext())
            {
                for (int i = 0; i < cols; i++)
                    for (int j = 0; j < rows; j++)
                    {
                        if (Field.Cells[i, j].ContainsBomb)
                            blitSprite(i, j, CellContentsEnum.Bomb);
                        else
                            switch (Field.Cells[i, j].BombsInNeighbourhood)
                            {
                                case 0:
                                    blitSprite(i, j, CellContentsEnum.Empty);
                                    break;
                                case 1:
                                    blitSprite(i, j, CellContentsEnum.One);
                                    break;
                                case 2:
                                    blitSprite(i, j, CellContentsEnum.Two);
                                    break;
                                case 3:
                                    blitSprite(i, j, CellContentsEnum.Three);
                                    break;
                                case 4:
                                    blitSprite(i, j, CellContentsEnum.Four);
                                    break;
                                case 5:
                                    blitSprite(i, j, CellContentsEnum.Five);
                                    break;
                                case 6:
                                    blitSprite(i, j, CellContentsEnum.Six);
                                    break;
                                case 7:
                                    blitSprite(i, j, CellContentsEnum.Seven);
                                    break;
                                case 8:
                                    blitSprite(i, j, CellContentsEnum.Eight);
                                    break;
                            }
                    }
            }
        }

        void blitSprite(int i, int j, CellContentsEnum e)
        {
            var writeableBmpImage = BitmapFactory.New(0, 0).FromResource("Images\\CellSprites.png");

            writeableBmp.Blit(new Point(i * c_CellSize.Width, j * c_CellSize.Height),
                                              writeableBmpImage,
                                              new Rect(imagesDictionary[e].X, imagesDictionary[e].Y,
                                                       c_CellSize.Width, c_CellSize.Height),
                                              Colors.White, WriteableBitmapExtensions.BlendMode.None);
        }

        #region imagesDictionary
        Dictionary<CellContentsEnum, Point> imagesDictionary = new Dictionary<CellContentsEnum, Point>()
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
    }
}

