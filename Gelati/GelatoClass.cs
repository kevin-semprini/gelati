using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GelatoClass
{
    //  CONTENT DEL FILE .CSV PER I GELATI
    //idGelato;Nome;Descrizione;Prezzo
    //1;Puffo;Gelato azzurro;10,1
    //2;Panna e Cioccolato;Gelato a base di soia;10,2
    //3;Tiramisù;Semifreddo con nocciole e caffè;10,3
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
}
