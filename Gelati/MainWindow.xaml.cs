﻿using GelatoClass;
using IngredienteClass;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Gelati
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ListGelati Gelati;
        Ingredienti ingredienti;



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int idx = 0;
            try
            {
                Gelati = new ListGelati("Gelati.csv");

                dgGelati.ItemsSource = Gelati;


                StatusBar.Text = $"ho letto {Gelati.Count} tipi di gelato";

                ///////////////////////////////////////////////////////////////////////////////////////LEGGO I CONTATTI

                //Ingredienti = new Ingredienti("Ingredienti.csv");
                ingredienti = new Ingredienti("Ingredienti.csv");
            }
            catch (Exception err)
            {
                MessageBox.Show($" NUH UH\n + {err.Message} \n  alla riga {idx}");
            }
        }

        private void dgGelati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gelato p = e.AddedItems[0] as Gelato;

            if (p != null)
            {
                StatusBar.Text = $"gelato selezionato: {p.Nome} {p.Prezzo}";

                List<Ingrediente> ingredinti = new List<Ingrediente>();

                foreach (var item in ingredienti)
                {
                    if (item.IdPersona == p.Id)
                    {
                        ingredinti.Add(item);
                    }
                }
                DgIngredienti.ItemsSource = ingredinti;
            }
        }
    }
}