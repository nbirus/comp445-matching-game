using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightsOut___Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LightsOutGame game;
        private Color gridColor = Colors.White;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //ApplicationData.Current.LocalSettings.Values.Clear();

            game = new LightsOutGame();
            CreateGrid();
            DrawGrid();
        }              

        private void CreateGrid()
        {
            // Remove all previously-existing rectangles
            paintCanvas.Children.Clear();
            
            int rectSize = (int)paintCanvas.Width / game.GridSize;
            
            // Turn entire grid on and create rectangles to represent it
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;

                    int x = c * rectSize;
                    int y = r * rectSize;

                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);

                    // Add the new rectangle to the canvas' children
                    paintCanvas.Children.Add(rect);
                }
            }
        }

        private void DrawGrid()
        {
            int index = 0;

            SolidColorBrush blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            SolidColorBrush lightBrush = new SolidColorBrush(gridColor);

            // Set colors of each rectangle based on grid values
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = paintCanvas.Children[index] as Rectangle;
                    index++;

                    if (game.GetGridValue(r, c))
                    {
                        // On
                        string letter = game.GetObjectGridValue(r, c);
                        rect.Fill = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("ms-appx:///images/" + letter +".jpg"))
                        };
                        rect.Stroke = blackBrush;
                    }
                    else
                    {
                        // Off
                        rect.Fill = lightBrush;
                        rect.Stroke = blackBrush;
                    }
                }
            }
        }

        private async void paintCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int rectSize = (int)paintCanvas.Width / game.GridSize;

            // Find row, col of mouse press
            Point mousePosition = e.GetPosition(paintCanvas);
            int row = (int)(mousePosition.Y) / rectSize;
            int col = (int)(mousePosition.X) / rectSize;

            game.Move(row, col);

            // Redraw the board
            DrawGrid();

            if (game.IsGameOver())
            {
                MessageDialog msgDialog = new MessageDialog("Congratulations!  You've won!", "Lights Out!");

                // Add an OK button
                msgDialog.Commands.Add(new UICommand("OK"));

                // Show the message box and wait aynchrously for a button press
                IUICommand command = await msgDialog.ShowAsync();

                // This executes *after* the OK button was pressed
                game.NewGame();
                DrawGrid();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Restore state
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("settings"))
            {
                var settings = ApplicationData.Current.LocalSettings.Values["settings"] as ApplicationDataCompositeValue;

                int gridSize = (int)settings["gridSize"];
                game.GridSize = gridSize;

                string colorRgb = settings["gridColor"] as string;
                gridColor = ColorConverter.ConvertFromString(colorRgb);

                CreateGrid();                
            }
            
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("game"))
            {
                string gridJson = ApplicationData.Current.LocalSettings.Values["game"] as string;
                game = JsonConvert.DeserializeObject<LightsOutGame>(gridJson);
            }

            DrawGrid();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Save state in case app is terminated later
            string gridJson = JsonConvert.SerializeObject(game);
            ApplicationData.Current.LocalSettings.Values["game"] = gridJson;            
        }

        private void AppBarButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            game.NewGame();
            DrawGrid();
        }

        private void AppBarButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void AppBarButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
    }
}
