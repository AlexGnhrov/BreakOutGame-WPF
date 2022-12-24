using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BreakOutGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BlockInitialization();

            BoardMovment();
            BallMovment();
        }

        int CountOfHP = 3;
        int ScoreCount = 0;

        bool GameIsPaused = true;
        bool GameIsOver = false;

        int Board_X = 200;
        int BoardSpeed = 15;

        bool HitBottom = true;
        bool HitRight = false;


        float Ball_Y = 35;
        float Ball_X = 238;

        float BallSpeed;
        int ballSpeed_Y = 0;
        int ballSpeed_X = 0;

        float BallBoost = 5;

        async Task BoardMovment()
        {
            CheckBoardBorder();

            while (true)
            {
                if (!GameIsPaused)
                {
                    if (!GameIsOver)
                    {
                        if (Keyboard.IsKeyDown(Key.A))
                        {
                            Board_X -= BoardSpeed;
                            if (Board_X < 0) Board_X = 0;
                        }
                        else if (Keyboard.IsKeyDown(Key.D))
                        {
                            Board_X += BoardSpeed;
                            if (Board_X > 379) Board_X = 379;
                        }
                        Board.Margin = new Thickness(Board_X, 0, 0, 10);
                    }
                }
                await Task.Delay(1);
            }
        }


        async Task BallMovment()
        {
            BallSpeed = BallBoost;
            while (true)
            {
                if (!GameIsPaused)
                {

                    if (HitBottom && !HitRight)
                    {
                        Ball_X += BallSpeed + ballSpeed_X;
                        Ball_Y += BallSpeed + ballSpeed_Y;
                    }
                    else if (!HitBottom && !HitRight)
                    {
                        Ball_X += BallSpeed;
                        Ball_Y -= BallSpeed;
                    }
                    else if (!HitBottom && HitRight)
                    {
                        Ball_X -= BallSpeed;
                        Ball_Y -= BallSpeed;
                    }
                    else if (HitBottom && HitRight)
                    {
                        Ball_X -= BallSpeed + ballSpeed_X;
                        Ball_Y += BallSpeed + ballSpeed_Y;
                    }
                    BallDirectionChange();

                    Ball.Margin = new Thickness(Ball_X, 0, 0, Ball_Y);

                }
                await Task.Delay(1);
            }
        }
        void BallDirectionChange()
        {
            if (Ball_Y > 390)//Удар об потолок
            {
                ignoreCheckicng = false;
                HitBottom = false;
            }
            else if (Ball_Y < 7.5)
            {
                HitBottom = true;
                HitRight = false;

                Ball_Y = 35;
                Ball_X = 235;

                --CountOfHP;
                HP.Content = CountOfHP.ToString();
                if (CountOfHP < 0)
                {
                    GameOver();

                    CountOfHP = 0;
                    HP.Content = CountOfHP.ToString();
                }
                else
                {
                    PauseGame(false);
                }

            }

            else if (Ball_X > 450)//Удар об правую стенку
            {
                ignoreCheckicng = false;
                HitRight = true;

                BallSpeed = BallBoost;
                ballSpeed_Y = 0;
                ballSpeed_X = 0;
            }
            else if (Ball_X < 0)//Удар об левую стенку
            {
                ignoreCheckicng = false;
                HitRight = false;

                BallSpeed = BallBoost;
                ballSpeed_Y = 0;
                ballSpeed_X = 0;
            }
        }



        async Task CheckBoardBorder()
        {
            float ballBorderCollision;
            while (true)
            {
                ballBorderCollision = Ball_X + 12.5f;
                if (Ball_Y < 30)
                {

                    if (!HitRight)
                    {

                        if (Board_X - 25 < ballBorderCollision && Board_X + 5 > ballBorderCollision)//Левый крайний угл
                        {

                            HitBottom = true;
                            HitRight = true;


                            ignoreCheckicng = false;
                        }
                        if (Board_X + 95 < ballBorderCollision && Board_X + 125 > ballBorderCollision)//Правый крайний угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = -3;
                            ballSpeed_X = 2;
                        }
                        if (Board_X + 5 < ballBorderCollision && Board_X + 35 > ballBorderCollision)//Левый угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = 3;
                            ballSpeed_X = -2;
                        }
                        if (Board_X + 65 < ballBorderCollision && Board_X + 95 > ballBorderCollision)//Правый угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = 1;
                            ballSpeed_X = 3;
                        }
                        if (Board_X + 35 < ballBorderCollision && Board_X + 65 > ballBorderCollision)//Центр
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;
                        }

                    }
                    else if (HitRight)
                    {
                        if (Board_X + 95 < ballBorderCollision && Board_X + 125 > ballBorderCollision)//Правый крайний угл
                        {
                            ignoreCheckicng = false;

                            HitBottom = true;
                            HitRight = false;
                        }
                        if (Board_X - 25 < ballBorderCollision && Board_X + 5 > ballBorderCollision)//Левый крайний угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = -3;
                            ballSpeed_X = 2;

                        }
                        if (Board_X + 65 < ballBorderCollision && Board_X + 95 > ballBorderCollision)//Правый угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = 3;
                            ballSpeed_X = -2;
                        }
                        if (Board_X + 5 < ballBorderCollision && Board_X + 35 > ballBorderCollision)//Левый угл
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;

                            ballSpeed_Y = 1;
                            ballSpeed_X = 3;

                        }
                        if (Board_X + 35 < ballBorderCollision && Board_X + 65 > ballBorderCollision)//Центр
                        {
                            HitBottom = true;

                            ignoreCheckicng = false;
                        }
                    }
                    if (GameIsOver)
                    {
                        HitBottom = true;

                        ignoreCheckicng = false;
                    }
                }
                await Task.Delay(1);
            }
        }

        int BlockCount;
        async Task BlockInitialization()
        {
            BlockPauseKey = true;

            BlockCount = 0;
            BlockDestroyed = 0;

            int BlockX = 3;
            int BlockY = 250;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Border border = new Border();

                    ChangeBlockColorInt(border, i);

                    border.Height = 20;
                    border.Width = 50;

                    border.VerticalAlignment = VerticalAlignment.Bottom;
                    border.HorizontalAlignment = HorizontalAlignment.Left;


                    Playground.Children.Add(border);

                    border.Margin = new Thickness(BlockX, 0, 0, BlockY);

                    BlockBorderChecker(border, BlockX, BlockY);


                    BlockX += 53;
                    ++BlockCount;
                    await Task.Delay(100);
                }
                BlockX = 3;
                BlockY += 23;
            }
            await Task.Delay(1500);

            GameIsPaused = false;

            BlockPauseKey = false;
        }

        bool ignoreCheckicng = false;
        int BlockDestroyed = 0;
        bool CanselCollisionBlock = false;

        async Task BlockBorderChecker(Border border, int BlockX, int BlockY)
        {
            while (true)
            {
                if (!ignoreCheckicng)
                {
                    if (Ball_Y + 25 > BlockY && BlockY + 20 > Ball_Y
                        &&
                        BlockX < Ball_X && BlockX + 50 > Ball_X)
                    {
                        if (HitBottom)
                        {
                            HitBottom = false;

                            if (GameIsOver)
                                ignoreCheckicng = true;
                            else
                                break;
                        }
                        else
                        {
                            HitBottom = true;
                            if (GameIsOver)                       
                                ignoreCheckicng = true;
                            else
                                break;
                        }
                    }
                }
                if (CanselCollisionBlock)
                {
                    border.Background = null;
                    return;
                }
                await Task.Delay(1);
            }

            ScoreCount += 7;
            SCORE.Content = ScoreCount.ToString();

            ignoreCheckicng = true;
            border.Background = null;

            BallBoost += 0.15f;
            ++BlockDestroyed;

            if (BlockCount == BlockDestroyed)
            {
                PauseGame(true);
            }

            await Task.Delay(300);

            ignoreCheckicng = false;
        }


        async Task PauseGame(bool isLevelPassed)
        {
            Random r = new Random();

            GameIsPaused = true;

            Ball_Y = 35;
            Ball_X = 238;

            Board_X = 200;

            Ball.Margin = new Thickness(Ball_X, 0, 0, Ball_Y);
            Board.Margin = new Thickness(Board_X, 0, 0, 10);

            if (isLevelPassed)
            {
                BallBoost = 5;

                if (!GameIsOver)
                {
                    ++CountOfHP;
                    HP.Content = CountOfHP.ToString();

                    ScoreCount += 100;
                    SCORE.Content = ScoreCount.ToString();
                }

                Playground.Children.RemoveRange(9, BlockCount);

                if (r.Next(0, 2) == 1)
                    HitRight = true;
                else
                    HitRight = false;
                await BlockInitialization();
            }
            else
            {
                if (r.Next(0, 2) == 1)
                    HitRight = true;
                else
                    HitRight = false;

                await Task.Delay(1000);
                GameIsPaused = false;
            }
        }



        async Task GameOver()
        {
            GameIsOver = true;
            BlockPauseKey = true;

            Board.Width = 476;
            Board.Margin = new Thickness(0, 0, 0, 10);

            GameOverLabel.Visibility = Visibility.Visible;
            GameOverNotesLabel.Visibility = Visibility.Visible;

            while (true)
            {
                if (Keyboard.IsKeyDown(Key.R))
                {
                    CanselCollisionBlock = true;

                    Board.Width = 100;
                    Board.Margin = new Thickness(200, 0, 0, 10);

                    ScoreCount = 0;
                    SCORE.Content = "0";

                    CountOfHP = 3;
                    HP.Content = "3";

                    GameOverLabel.Visibility = Visibility.Hidden;
                    GameOverNotesLabel.Visibility = Visibility.Hidden;

                    await Task.Delay(100);
                    CanselCollisionBlock = false;

                    await PauseGame(true);

                    GameIsOver = false;
                    break;
                }
                await Task.Delay(1);
            }

        }


        void ChangeBlockColorInt(Border border, int i)
        {
            switch (i)
            {
                case 0:
                    border.Background = new SolidColorBrush(Colors.LawnGreen);
                    break;
                case 1:
                    border.Background = new SolidColorBrush(Colors.Blue);
                    break;
                case 2:
                    border.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                case 3:
                    border.Background = new SolidColorBrush(Colors.Red);
                    break;
            }
        }

        bool BlockPauseKey = false;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                if (BlockPauseKey)            
                    return;
                
                if (!GameIsPaused)
                {
                    PauseLabel.Visibility = Visibility.Visible;
                    GameIsPaused = true;
                }
                else
                {
                    PauseLabel.Visibility = Visibility.Hidden;
                    GameIsPaused = false;
                }
            }
            
        }
    }
}

