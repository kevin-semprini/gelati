# Gelati

programma fatto in WPF app che presi due file di testo (in questo caso due file .csv) uno per i gelati e uno per gli ingredienti relativi a uno dei gusti presenti al primo file, li ordina in due tabelle diverse e associa a un gusto di gelato gli ingredienti richiesti, compresi di quantità e prezzo del gelato

## specifiche

gli ingredienti sono collegati al gelato anche tramite il .csv, infatti la prima colonna la riempire quando si aggiunge un gusto è l' id, questo serve poi al programma per associare un gusto a degli ingredienti

il programma usa una serie di enum, un valore a tipo di ingrediente, occorre modificare la riserva di enum se si vuole salvare un nuovo tipo di ingrediente

## foto del codice

### classi per gli ingrendienti

classe per un singolo ingrediente, prende dal file .csv abbinato e lo crea sezionandolo con il .split e assegnando ogni valore ricavato a un certo campo della classe
~~~C#
public class Ingrediente
{
    public int IdPersona { get; set; }
    public TipoIngrediente tipo { get; set; }
    public string Valore { get; set; }

    public Ingrediente(Gelato p)
    {
        //IdPersona = p.Numero;
        tipo = TipoIngrediente.Nessuno;
    }

    public Ingrediente() { }
    public Ingrediente(string riga)
    {

        string[] dati = riga.Split(';');



        int amoguss;
        int.TryParse(dati[0], out amoguss);
        this.IdPersona = amoguss;

        TipoIngrediente amogus;
        Enum.TryParse(dati[1], out amogus);
        this.tipo = amogus;

        this.Valore = dati[2];
    }

    public static Ingrediente MakeIngrediente(string riga)
    {
        string[] campi = riga.Split(";");

        TipoIngrediente c;
        Enum.TryParse(campi[1], out c);

        switch (c)
        {
            case TipoIngrediente.Latte:
                return new IngredienteLatte(riga);
        }


        return new Ingrediente(riga);
    }

}
~~~

classe fatta per gestire la lita di ingredienti, eredita da la lista preimpostata, è presente solo il costruttore overridato
~~~C#
public class ListIngredienti : List<Ingrediente>
{
    public ListIngredienti() { }
    public ListIngredienti(string nomeFile)
    {
        StreamReader fin = new StreamReader(nomeFile);
        fin.ReadLine();

        while (!fin.EndOfStream)
        {
            Add(Ingrediente.MakeIngrediente(fin.ReadLine()));
        }
    }

}
~~~


### classi per i Gelati


classe fatta per gestire il file abbinato di gelati, contiene 4 attributi e ognuno corrisponde a una colonna del file .csv, il tryParse è stato usato per i due interi presenti (prezzo e 
id), non ha metodi apparte un costruttore modificato, 

~~~C#
public class Gelato
{
    private int _id;
    private string _nome;
    private string _descrizione;
    private int _prezzo;

    public string Nome { get => _nome; set => _nome = value; }
    public string Descrizione { get => _descrizione; set => _descrizione = value; }
    public int Prezzo { get => _prezzo; set => _prezzo = value; }
    public int Id { get => _id; set => _id = value; }

    public Gelato(string riga)
    {
        // ".split()" crea una vettore di elemnti stringa
        string[] campi = riga.Split(';');
        int AmogusId = 0;
        int AmogusPrezzo = 0;

        int.TryParse(campi[0], out AmogusId);
        this.Id = AmogusId;
        string AmogusIdstr = AmogusId.ToString();
        if (AmogusIdstr == "idGelato") { this.Id = 1; }

        this.Nome = campi[1];
        this.Descrizione = campi[2];


        int.TryParse(campi[3], out AmogusPrezzo);
        this.Prezzo = AmogusPrezzo;
        string AmogusPrezzostr = AmogusPrezzo.ToString();
        if (AmogusPrezzostr == "Prezzo") { this.Id = 0; }
        //tryParse serve a convertire il valore dato come primo paramentro in un valore di tipo "int"
        //in questoc caso, e metterlo nella variabile PK che è di tipo int, è sempre stringa > var
        //in pratica lui controlla la stringa e prende  il valore richiesto dalla variabile messa
    }

    public Gelato() { }
}
~~~


classe creata per gestire la lista di gelati, eredita dalla lista standard di oggetti gelato, è presente solo il costruttore overridato
~~~C#
 public class ListGelati : List<Gelato>
 {
     public ListGelati() { }
     public ListGelati(string nomeFile)
     {
         StreamReader fin = new StreamReader(nomeFile);
         fin.ReadLine();

         while (!fin.EndOfStream)
         {
             string riga = fin.ReadLine();
             Gelato g = new Gelato(riga);
             Add(g);
         }

         fin.Close();
     }
 }
~~~


## MAIN

il main contiene le due variabili globali delle rispettive classi "ListGelati" e "ListIngredienti" che servono per creare e gestire le liste di gelato e ingrediente, usando il costruttore delle rispettive classi, sono presenti anche i metodi per interagire con l'interaffaccia grafica WPF, scritta in XAML, i metodi volti a questo scopo si occupano della selezione di una casella, azioni da svolgere dopo la fine del caricamento della pagina, e il cambio di selezione dentro la tabella
~~~C#

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
        ListIngredienti Ingredienti;



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int idx = 0;
            try
            {
                Gelati = new ListGelati("Gelati.csv");

                dgGelati.ItemsSource = Gelati;


                StatusBar.Text = $"ho letto {Gelati.Count} tipi di gelato";

                ///////////////////////////////////////////////////////////////////////////////////////LEGGO I CONTATTI

                Ingredienti = new ListIngredienti("Ingredienti.csv");
            }
            catch (Exception err)
            {
                MessageBox.Show($" NUH UH\n + {err.Message} \n  alla riga {idx}");
            }
        }

        private void dgGelati_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgGelati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gelato p = e.AddedItems[0] as Gelato;

            if (p != null)
            {
                StatusBar.Text = $"gelato selezionato: {p.Nome} {p.Prezzo}";


                List<Ingrediente> c = new List<Ingrediente>();

                foreach (var item in Ingredienti)
                {
                    if (item.IdPersona == p.Id)
                    {
                        c.Add(item);
                    }
                }

                DgIngredienti.ItemsSource = c;
            }
        }
    }
}
~~~

## Codice XAML

il codice XAML sottostante è tutta la parte grafica del sito, nonostante ci siano 4 bottoni, questi non sono usati in alcun modo per mancanza di direttive, il resto però consiste in 3 griglie, 1 per contenerne due, posizionate una a destra e una a sinistra, quella di destra sarà assegnata al caricamento dei gusti di gelato, e quella di sinistra al caricamento degli ingredienti assegnati al gusto selezionato

~~~C#
<Window x:Class="Gelati.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:Gelati"
        mc:Ignorable="d"
        Loaded="Window_Loaded"
        Title="MainWindow" Height="450" Width="800">
        
    
    
    <!--TABELLA DI BASE-->
    <Grid x:Name="MainGrid">
        <Grid.ColumnDefinitions>
            <ColumnDefinition></ColumnDefinition>
            <ColumnDefinition></ColumnDefinition>
        </Grid.ColumnDefinitions>

        <!--COLONNA DI SINISTRA-->

        <Grid x:Name="GridSx" Grid.Column="0">
            <Grid.RowDefinitions>
                <RowDefinition Height="40px"></RowDefinition>
                <RowDefinition Height="17*"></RowDefinition>
                <RowDefinition Height="20px"></RowDefinition>
            </Grid.RowDefinitions>

            <StackPanel Grid.Row="0" Orientation="Horizontal">
                <Button Height="35" Width="90">P1</Button>
                <Button Height="35" Width="90">P2</Button>
                <Button Height="35" Width="90">P3</Button>
                <Button Height="35" Width="90">P4</Button>
            </StackPanel>
            <DataGrid x:Name="dgGelati" 
                  LoadingRow="dgGelati_LoadingRow" 
                  Grid.Row="1" 
                  SelectionChanged="dgGelati_SelectionChanged">

            </DataGrid>
            <TextBlock x:Name="StatusBar" Grid.Row="2">status bar</TextBlock>
        </Grid>



        <!--COLONNA DI DESTRA-->

        <Grid x:Name="GridDx" Grid.Column="1">
            <Grid.RowDefinitions>
                <RowDefinition></RowDefinition>
                <RowDefinition></RowDefinition>
            </Grid.RowDefinitions>

            <Rectangle Fill="green" Grid.Row="0"></Rectangle>
            <DataGrid x:Name="DgIngredienti" Grid.Row="1" ></DataGrid>
        </Grid>
    </Grid>
</Window>
~~~



~~~C#
~~~v
