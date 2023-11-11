﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ProjectSnake
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
            {
                {GridValue.Empty, Images.Empty },
                {GridValue.Snake, Images.Body },
                {GridValue.Food, Images.Food }

            };

        private readonly Dictionary<Direction, int> dirToRotation = new()
            {
                {Direction.Up, 0},
                {Direction.Down, 180},
                {Direction.Right, 90},
                {Direction.Left, 270},
            };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImage;
        private State gameState;
        private bool gameRunning;

        public MainWindow()
            {
            InitializeComponent();
            gridImage = SetupGrid();
            gameState = new State(rows, cols);
            }

        private async Task RunGame()
            {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new State(rows, cols);
            }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
            {
            if (Overlay.Visibility == Visibility.Visible)
                {
                e.Handled = true;
                };

            if(!gameRunning)
                {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
                }
            }

        private void Window_KeyDown(object sender, KeyEventArgs e)
            {
            if (gameState.GameOver)
                {
                return;
                }
            switch (e.Key)
                {
                case Key.Left:
                gameState.ChangeDirection(Direction.Left); break;
                case Key.Right:
                gameState.ChangeDirection(Direction.Right); break;
                case Key.Up:
                gameState.ChangeDirection(Direction.Up); break;
                case Key.Down:
                gameState.ChangeDirection(Direction.Down); break;
                }
            }

        private async Task GameLoop()
            {
            while (!gameState.GameOver)
                {
                await Task.Delay(200);
                gameState.Move();
                Draw();
                }
            }

        private Image[,] SetupGrid()
            {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++)
                {
                for (int c = 0; c < cols; c++)
                    {
                    Image image = new Image
                        {
                        Source = Images.Empty
                        };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                    }

                }

            return images;
            }

        private void Draw()
            {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Score: {gameState.Score}";
            }

        private void DrawGrid()
            {
            for (int r = 0; r < rows; r++)
                {
                for (int c = 0; c < cols; c++)
                    {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImage[r, c].Source = gridValToImage[gridVal];
                    }
                }
            }

        private void DrawSnakeHead()
            {
            Position headPos = gameState.HeadPosition();
            Image image = gridImage[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
            }

        private async Task ShowCountDown()
            {
            for(int i = 3;i >= 0; i--)
                {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
                }
            }

        private async Task ShowGameOver()
            {
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Press any key to START!";
            }
        }
    }
