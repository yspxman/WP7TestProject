using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace AppTest.PuzzleGame
{
    public partial class PuzzlePage : PhoneApplicationPage        
    {
        private const double DoubleTapSpeed = 500;
        private const int ImageSize = 435;
        private PuzzleGameEngine game;
        private Canvas[] puzzlePieces;
        private Stream imageStream;

        public PuzzlePage()
        {
            InitializeComponent();

            //添加landscape 支持
            SupportedOrientations = SupportedPageOrientation.Portrait |
                SupportedPageOrientation.Landscape;
            
            this.game = new PuzzleGameEngine(3);
            this.game.GameStarted += delegate
            {
                this.StausPanel.Visibility = Visibility.Visible;
                this.TapToContinueTextBlock.Opacity = 0;
                this.TotalMovesTextBlock.Text = this.game.TotalMoves.ToString();
            };

            this.game.GameOver += delegate
            {
                this.TapToContinueTextBlock.Opacity = 1;
                this.StausPanel.Visibility = Visibility.Visible;
                this.TotalMovesTextBlock.Text = game.TotalMoves.ToString();
            };
           

            this.game.PieceUpdated += delegate(object sender, PieceUpdatedEventArgs args)
            {
                int pieceSize = ImageSize / this.game.ColsAndRows;
                this.AnimatePiece(this.puzzlePieces[args.PieceId], Canvas.LeftProperty, (int)args.NewPosition.X * pieceSize);
                this.AnimatePiece(this.puzzlePieces[args.PieceId], Canvas.TopProperty, (int)args.NewPosition.Y * pieceSize);
                this.TotalMovesTextBlock.Text = this.game.TotalMoves.ToString();

            };
            this.InitBoard();

        }

        private void InitBoard()
        {
            int totalPieces = this.game.ColsAndRows * this.game.ColsAndRows;
            int pieceSize = ImageSize / this.game.ColsAndRows;
            this.puzzlePieces = new Canvas[totalPieces];
            int nx = 0;
            for (int ix = 0; ix < this.game.ColsAndRows; ix++)
            {
                for (int iy = 0; iy < this.game.ColsAndRows; iy++)
                {
                    nx = (ix * this.game.ColsAndRows) + iy;
                    Image image = new Image();
                    // 设置本地默认属性
                    //关于 DependencyObject, http://www.devdiv.com/article-1834-1.html
                    image.SetValue(FrameworkElement.NameProperty, "PuzzleImage_" + nx);
                    image.Height = ImageSize;
                    image.Width = ImageSize;
                    image.Stretch = Stretch.UniformToFill;
                    //image.Name =

                    // RectangleGeometry 是几何意义上的矩形， 可以用来切割图片，
                    // rect 属性定义了矩形的位置和 大小
                    // Rectangle可以被show出来，RectangleGeometry则不能
                    //http://apps.hi.baidu.com/share/detail/16446239
                    //http://www.cnblogs.com/aihu0307/archive/2011/07/08/2100784.html
                    RectangleGeometry r = new RectangleGeometry();
                    r.Rect = new Rect((ix * pieceSize), (iy * pieceSize), pieceSize, pieceSize);
                    //裁剪，这时RectangleGeometry所代表的位置就是相对于image的位置

                    image.Clip = r;

                    image.SetValue(Canvas.TopProperty, Convert.ToDouble(iy * pieceSize * -1));
                    image.SetValue(Canvas.LeftProperty, Convert.ToDouble(ix * pieceSize * -1));

                    this.puzzlePieces[nx] = new Canvas();
                    this.puzzlePieces[nx].SetValue(FrameworkElement.NameProperty, "PuzzlePiece_" + nx);
                    this.puzzlePieces[nx].Width = pieceSize;
                    this.puzzlePieces[nx].Height = pieceSize;
                    this.puzzlePieces[nx].Children.Add(image);
                    this.puzzlePieces[nx].MouseLeftButtonDown += this.PuzzlePiece_MouseLeftButtonDown;
                    if (nx < totalPieces - 1)
                    {
                        this.GameContainer.Children.Add(this.puzzlePieces[nx]);
                    }
                }
            }
            // Retrieve image
            StreamResourceInfo imageResource = Application.GetResourceStream
                (new Uri("PuzzleGame/Puzzle.jpg", UriKind.Relative));
            this.ImageStream = imageResource.Stream;

            this.game.Reset();

        }
        private void PuzzlePiece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.game.IsPlaying)
            {
                this.game.NewGame();
            }
        }

        private void AnimatePiece(DependencyObject piece, DependencyProperty dp, double newValue)
        {
            Storyboard storyBoard = new Storyboard();
            Storyboard.SetTarget(storyBoard, piece);
            Storyboard.SetTargetProperty(storyBoard, new PropertyPath(dp));
            storyBoard.Children.Add(new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                From = Convert.ToInt32(piece.GetValue(dp)),
                To = Convert.ToDouble(newValue),
                EasingFunction = new SineEase()
            });
            storyBoard.Begin();
        }

        public Stream ImageStream
        {
            get { return this.imageStream; }
            set {
                this.imageStream = value;
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(value);
                // previewimage 是定义的image 控件, 充当底色图， 定义了opacity， 使得图像变暗
                this.PreviewImage.Source = bitmap;

                int i = 0;
                for (int ix = 0; ix < this.game.ColsAndRows; ix++)
                {
                    for (int iy = 0; iy < this.game.ColsAndRows; iy++)
                    {
                        //这里相当于pieceImage是个指针，把puzzlePieces做了转换，
                        //并不是定义了一个pieceImage对象
                        // 搜索一下C#中的类型转换
                        Image pieceImage = this.puzzlePieces[i].Children[0] as Image;
                        pieceImage.Source = bitmap;
                        i++;
                    }
                }
            }
        }

        private void SolveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.game.Reset();
            this.game.CheckWinner();

        }

    }

}