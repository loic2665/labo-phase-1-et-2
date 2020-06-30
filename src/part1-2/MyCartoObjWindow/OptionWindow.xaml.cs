using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow(String currentPath, Color currentBgColor, Color currentFgColor)
        {
            InitializeComponent();
            InputDirectory.Text = currentPath;
            ListBoxBackgroundColorPicker.SelectedColor = currentBgColor;
            ListBoxForegroundColorPicker.SelectedColor = currentFgColor;
        }

        private void CheckDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(InputDirectory.Text);
            if (!dir.Exists)
            {
                MessageBox.Show(
                    "Le dossier entré est inexistant, quand vous\nsauvegarderez votre profil, celui-ci sera créé.",
                    "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);


            }

        }

        public event Action<String, Color, Color> OnApply;



        private void ApplyAllButton_Click(object sender, RoutedEventArgs e)
        {

            // do my stuff
            Button btn = sender as Button;
            CheckDirectoryButton_Click(null, null);
            OnApply?.Invoke(InputDirectory.Text, ListBoxBackgroundColorPicker.SelectedColor.Value, ListBoxForegroundColorPicker.SelectedColor.Value);
            if (btn.Content.ToString() == "OK")
            {
                this.Close();
            }
        }

        private void Cancel_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
