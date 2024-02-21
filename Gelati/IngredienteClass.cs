using GelatoClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngredienteClass;

namespace IngredienteClass
{

    // CONTENT DEL FILE .CSV PER GLI INGREDIENTI
    //idGelato;Tipo;Valore
    //1;Panna;100gr
    //1;Colorante;10gr
    //1;Aroma;Nocciola
    //2;PannaSoia;120gr
    //2;Cacao;30gr
    //3;Latte;130gr
    //3;Caffe;15gr
    //3;Mascarpone;50gr
    //3;Uovo;1

    public enum TipoIngrediente
    {
        Nessuno,
        Panna,
        Colorante,
        Aroma,
        Cacao,
        Latte,
        Mascarpone,
        Uovo,
        PannaSoia,
        Caffe,
    }
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



    public class IngredienteLatte : Ingrediente
    {
        public IngredienteLatte() { }
        public IngredienteLatte(string riga) : base(riga)
        {

        }
    }

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
}