using Lib;
using Microsoft.Win32;
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





namespace NaleznikWPF {
    /// <summary>
    /// Interaction logic for NewFind.xaml
    /// </summary>
    public partial class NewFind : UserControl {
        private NaleznikController controller;
        public NewFind(NaleznikController controller) {
            InitializeComponent();
            this.controller = controller;
        }

        private void AddFindingButton_Click(object sender, RoutedEventArgs e) {
            try {
                // Načítání hodnot z UI
                string name = NameTextBox.Text;
                int year = 0; //YearDatePicker.SelectedDate ?? DateTime.Now; // Používáme aktuální datum, pokud není vybráno
                string description = DescriptionTextBox.Text;
                double depth = double.TryParse(DepthTextBox.Text, out double d) ? d : 0;
                double latitude = double.TryParse(LatitudeTextBox.Text, out double lat) ? lat : 0;
                double longitude = double.TryParse(LongitudeTextBox.Text, out double lon) ? lon : 0;
                
                // Vytvoření nového nálezu
                var newFinding = new Finding(
                    0,  // ID je automaticky generováno v databázi
                    name,
                    year = 10,
                    description,
                    999,  // Používáme aktuální datum pro datum nálezu
                    depth,
                    new location(latitude, longitude)
                );

                // Přidání nálezu do databáze
                controller.AddFinding(newFinding);

                
                MessageBox.Show("Lukáši jsi párek a zapomněl jsi napsat souřadnice","Uloženo.");
            }
            catch (Exception ex) {
                MessageBox.Show($"Chyba ukládání nálezu {ex.Message}");
            }
        }

        private void btFrontSideImage_click(object sender, RoutedEventArgs e) {
            LoadImage(ImageBoxFrontSide,btImageFrontSide);
        }

        private void LoadImage(Image imagebox, Button button) {
            OpenFileDialog openFileDialog = new OpenFileDialog() {
                Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*",
                Title = "Vyberte soubor"
            };

            if (openFileDialog.ShowDialog() == true) {
                string filepath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                imagebox.Source = bitmap;

                button.Visibility = Visibility.Collapsed;
                imagebox.Visibility = Visibility.Visible;
            }
        }

        private void ImageBoxFrontSide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            ShowBiggerImage(ImageBoxFrontSide);
        }

        private void ShowBiggerImage(Image imagebox) {
            if (imagebox.Source != null) {
                LargeImage.Source = imagebox.Source; 
                Overlay.Visibility = Visibility.Visible; 
            }
        }

        private void CloseOverlay_Click(object sender, RoutedEventArgs e) {
            // Skryje překryvnou vrstvu
            Overlay.Visibility = Visibility.Collapsed;
        }

        private void ImageBoxBackSide_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ShowBiggerImage(ImageBoxBackSide);
        }

        private void btImageBackSide_Click(object sender, RoutedEventArgs e) {
            LoadImage(ImageBoxBackSide, btImageBackSide);
        }

        private void LatitudeTextBox_GotFocus(object sender, RoutedEventArgs e) {
            FocusChanged(LatitudeTextBox, "Zadej zeměpisnou šířku");
        }

        private void LongitudeTextBox_GotFocus(object sender, RoutedEventArgs e) {
            FocusChanged(LongitudeTextBox,"Zadej zeměpisnou délku");
        }

        private void LongitudeTextBox_LostFocus(object sender, RoutedEventArgs e) {
            FocusChanged(LongitudeTextBox, "Zadej zeměpisnou délku");
        }

        private void LatitudeTextBox_LostFocus(object sender, RoutedEventArgs e) {
            FocusChanged(LatitudeTextBox, "Zadej zeměpisnou šířku");
        }

        private void FocusChanged(TextBox box,string text) {
            if (box.Text == text) {
                box.Text = string.Empty;
            }
            else if (string.IsNullOrWhiteSpace(box.Text)){
                box.Text = text;
            }

            
        }

        private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e) {
            FocusChanged(DescriptionTextBox, "Vlož popis");
            DescriptionTextBox.TextAlignment = TextAlignment.Left;
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e) {
            FocusChanged(DescriptionTextBox, "Vlož popis");
            DescriptionTextBox.TextAlignment = TextAlignment.Center;
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e) {
            FocusChanged(NameTextBox, "Zadej název");
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e) {
            FocusChanged(NameTextBox, "Zadej název");
        }

		private void RadioButtonCoin_Checked(object sender, RoutedEventArgs e) {
			ComboBoxName.Visibility = Visibility.Visible;
			NameTextBox.Visibility = Visibility.Collapsed;
		}

		private void RadioButtonArtifact_Checked(object sender, RoutedEventArgs e) {
			ComboBoxName.Visibility = Visibility.Collapsed;
			NameTextBox.Visibility = Visibility.Visible;
		}

		private void ImageBoxFindCondition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			ShowBiggerImage(ImageBoxFindCondition);
		}

		private void ImageBoxOtherPhotos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			ShowBiggerImage(ImageBoxOtherPhotos);
		}

		private void btFindConditionImage_Click(object sender, RoutedEventArgs e) {
			LoadImage(ImageBoxFindCondition, btImageFindCondition);
		}

		private void btOtherPhotos_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog() {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*",
                Title = "Vyberte soubor"

            };

			if (openFileDialog.ShowDialog() == true) {
				
				string filepath = openFileDialog.FileName;
				BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
				ImageBoxOtherPhotos.Source = bitmap;

				this.Visibility = Visibility.Collapsed;
				ImageBoxOtherPhotos.Visibility = Visibility.Visible;
			}
		}
	}
}

