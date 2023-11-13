using Newtonsoft.Json;
using ProjectSnake.IO;
using System;
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


        private FileWriter writer = new FileWriter();
        private FileReader reader = new FileReader();
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImage;
        private State gameState;
        private bool gameRunning;
        private int speed = 200;

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

            if (!gameRunning)
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
                await Task.Delay(speed);
                gameState.Move();

                Draw();
                }


            }

        private Image[,] SetupGrid()
            {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            GameGrid.Width = GameGrid.Height * (cols / (double)rows);

            for (int r = 0; r < rows; r++)
                {
                for (int c = 0; c < cols; c++)
                    {
                    Image image = new Image
                        {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
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

            IncreaseSpeed();

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
                    gridImage[r, c].RenderTransform = Transform.Identity;
                    }
                }
            }

        private async void IncreaseSpeed()
            {
            await Task.Delay(100);
            switch (gameState.Score)
                {
                case 5:
                speed = 175;
                break;
                case 10:
                speed = 150;
                break;
                case 15:
                speed = 125;
                break;
                case 20:
                speed = 100;
                break;
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

        private async Task DrawDeadSnake()
            {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for (int i = 0; i < positions.Count; i++)
                {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImage[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
                }
            }

        private async Task ShowCountDown()
            {
            for (int i = 3; i >= 0; i--)
                {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
                }
            }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
            {
            OverlayText.Text = "Settings";
            Task.Delay(500);

            SettingsButton.Visibility = Visibility.Collapsed;
            ScoresButton.Visibility = Visibility.Collapsed;


            BackButton.Visibility = Visibility.Visible;
            }

        private void ScoresButton_Click(object sender, RoutedEventArgs e)
            {
            OverlayText.Text = "HighScores";
            Task.Delay(500);
            SettingsButton.Visibility = Visibility.Collapsed;
            ScoresButton.Visibility = Visibility.Collapsed;

            ScoresTextTable.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;

            ShowScore();
            }

        private void BackButton_Click(object sender, RoutedEventArgs e)
            {
            OverlayText.Text = "Press any key to START!";
            Task.Delay(500);


            SettingsButton.Visibility = Visibility.Visible;
            ScoresButton.Visibility = Visibility.Visible;

            ScoresTextTable.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
            }

        private void ShowScore()
            {
            StringBuilder sb = new StringBuilder();
            string tempscores = reader.ReadFile();
            var scores = JsonConvert.DeserializeObject<Dictionary<int, int>>(tempscores);

            foreach (var s in scores)
                {
                sb.AppendLine($"{s.Key}: {s.Value}");
                }

            ScoresTextTable.Text = sb.ToString().Trim();
            }

        private async Task SaveScore()
            {
            string tempscores = reader.ReadFile();
            var scores = JsonConvert.DeserializeObject<Dictionary<int, int>>(tempscores);

            int currentScore = gameState.Score;

            for (int i = 1; i <= scores.Count; i++)
                {
                int highestNum = 0;
                if (currentScore > scores[i])
                    {
                    highestNum = scores[i];
                    scores[i] = currentScore;
                    currentScore = highestNum;
                    }
                }
            string result = JsonConvert.SerializeObject(scores);
            await Console.Out.WriteLineAsync(result);
            writer.WriteLine(result);
            }

        private async Task ShowGameOver()
            {
            await SaveScore();
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Press any key to START!";
            }
        }
    }