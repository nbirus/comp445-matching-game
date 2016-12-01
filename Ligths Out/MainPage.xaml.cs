using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ligths_Out
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int GridLength = 300;
        private const int NumCells = 4;
        private const int CellLength = GridLength / NumCells;
        private bool[,] grid; //Stores on, off state of cells in grid
        private int[,] object_grid;
        private Random rand; //Generate random numbers
        private bool didPlayerWin = false;

        private int firstPosition;
        private int lastPosition;

        public MainPage()
        {
            this.InitializeComponent();
            rand = new Random();
            grid = new bool[NumCells, NumCells];
            object_grid = new int[NumCells, NumCells];

            int match_var = 1;
            //Turn entire grid on
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                {
                    if (c % 2 == 0)
                        object_grid[r, c] = match_var;
                    else
                        match_var++;

                    grid[r, c] = true;

                }

            //Add a rectangle and colors
            for (int x = 0; x < NumCells; x += 1)
                for (int y = 0; y < NumCells; y += 1)
                {

                    // rect.Fill = new SolidColorBrush(Windows.UI.Colors.White);

                    Rectangle rect = new Rectangle();
                   
                    ImageBrush imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///images/apples.jpg"));

                    rect.Fill = imgBrush;
                    rect.Width = GridLength / 3;
                    rect.Height = rect.Width;
                    rect.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                    rect.Name = x.ToString() + '-' + y.ToString();
                    Canvas.SetTop(rect, y * GridLength / 3);
                    Canvas.SetLeft(rect, x * GridLength / 3);
                    gameBoardCanvas.Children.Add(rect);

                }
            gameBoardCanvas.IsTapEnabled = false;
        }


        private void newGameClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            didPlayerWin = false;
            for (int x = 0; x < NumCells; x++)
            {
                for (int y = 0; y < NumCells; y++)
                {
                    grid[x, y] = rand.Next(2) == 1;
                }
            }
            gameBoardCanvas.IsTapEnabled = true;
            DrawGrid();
        }

        private async void gameboardClick(object sender, TappedRoutedEventArgs e)
        {
            Point mousePosition = e.GetPosition(gameBoardCanvas);
            int r = (int)(mousePosition.Y) / CellLength;
            int c = (int)(mousePosition.X) / CellLength;

            for (int i = (int)r - 1; i <= r + 1; i++)
            {
                for (int j = (int)c - 1; j <= c + 1; j++)
                {
                    if (i >= 0 && i < NumCells && j >= 0 && j < NumCells)
                    {
                        grid[i, j] = !grid[i, j];

                        FlipCard(i , j);

                        firstPosition = i;
                        firstPosition = j;

                    }
                }
            }

            IsMatch();

            lastPosition = firstPosition;
            lastPosition = firstPosition;

            //DrawGrid();
            //Check if puzzle was solved
            if (PlayerWon())
            {
                MessageDialog msgDialog = new MessageDialog("Congratulations! You've won!", "Lights Out!");
                // Add an OK button
                msgDialog.Commands.Add(new UICommand("OK"));
                // Show the message box and wait aynchrously for a button press
                IUICommand command = await msgDialog.ShowAsync();

            }

        }

        private void FlipCard(int i, int j)
        {

        }

        private void IsMatch()
        {
            
        }

        private bool PlayerWon()
        {
            didPlayerWin = true;
            for (int i = 0; i < NumCells; i++)
            {
                for (int j = 0; j < NumCells; j++)
                {
                    if (grid[i, j])
                    {
                        didPlayerWin = false;

                    }
                }
            }
            return didPlayerWin;
        }

        private void DrawGrid()
        {
            int index = 0;
            // Set each rectangle’s color
            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {


                    Rectangle rect = gameBoardCanvas.Children[index] as Rectangle;
                    index++;
                    if (grid[r, c])
                    {
                        // On
                        rect.Fill = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("ms-appx:///images/apples.jpg"))
                        };
                    }
                    else
                    {
                        rect.Fill = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("ms-appx:///images/orange.jpg"))
                        };
                    }
                }
            }
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {

            MediaElement mediaElement = new MediaElement();


            this.Frame.Navigate(typeof(SecondPage));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navManager = SystemNavigationManager.GetForCurrentView();
            if (this.Frame.CanGoBack)
            {
                // Show Back button in title bar
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove Back button from title bar
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

        }


    }




}
