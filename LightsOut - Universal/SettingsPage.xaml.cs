using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LightsOut___Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private int origGridSize;
        private int gridSize;
        private Color gridColor;

        public SettingsPage()
        {
            this.InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Restore state
            //ApplicationData.Current.LocalSettings.Values.Clear();

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("settings"))
            {
                var settings = ApplicationData.Current.LocalSettings.Values["settings"] as ApplicationDataCompositeValue;

                gridSize = (int)settings["gridSize"];
                origGridSize = gridSize;

                string colorRgb = settings["gridColor"] as string;
                gridColor = ColorConverter.ConvertFromString(colorRgb);
            }
            else
            {
                // Defaults
                gridSize = 3;
                gridColor = Colors.White;
            }

            if (gridSize == 3)
            {
                radioButtonSizeThree.IsChecked = true;
            }
            else if (gridSize == 4)
            {
                radioButtonSizeFour.IsChecked = true;
            }
            else
            {
                radioButtonSizeFive.IsChecked = true;
            }

            if (gridColor == Colors.White)
            {
                colorPicker.Visibility = Visibility.Collapsed;
                radioButtonWhite.IsChecked = true;
            }
            else if (gridColor == Colors.Blue)
            {
                colorPicker.Visibility = Visibility.Collapsed;
                radioButtonBlue.IsChecked = true;
            }
            else
            {
                colorPicker.Visibility = Visibility.Visible;
                radioButtonOther.IsChecked = true;
                //colorPicker.SelectedColor.Color = gridColor;
                colorPicker.BlueValue = gridColor.B;
                colorPicker.RedValue = gridColor.R;
                colorPicker.GreenValue = gridColor.G;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var settings = new ApplicationDataCompositeValue();
            settings["gridSize"] = gridSize;

            if (radioButtonOther.IsChecked == true)
            {
                gridColor = colorPicker.SelectedColor.Color;
            }
            settings["gridColor"] = gridColor.ToString();
            ApplicationData.Current.LocalSettings.Values["settings"] = settings;

            // Must remove state if grid size is changed
            if (gridSize != origGridSize)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("game");
            }            
        }

        private void radioButtonColor_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string color = rb.Tag.ToString();
                switch (color)
                {
                    case "white":
                        colorPicker.Visibility = Visibility.Collapsed;
                        gridColor = Colors.White;
                        break;
                    case "blue":
                        colorPicker.Visibility = Visibility.Collapsed;
                        gridColor = Colors.Blue;
                        break;
                    case "other":
                        colorPicker.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void radioButtonSize_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                gridSize = Convert.ToInt32(rb.Tag);
            }
        }
    }
}
