using GelatoClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngredienteClass;
using System.Windows.Controls;

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
        Calorie,
        Lattosio,
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

        public Ingrediente() { }
        public Ingrediente(Gelato p)
        {
            //IdPersona = p.Numero;
            tipo = TipoIngrediente.Nessuno;
        }

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

            if (campi.Length > 3){ 
                switch (c)
                {
                    case TipoIngrediente.Latte:
                        return new IngredienteLat(riga);
                    case TipoIngrediente.Panna:
                        return new IngredienteCal(riga);
                    case TipoIngrediente.Colorante:
                        return new IngredienteCol(riga);
                    default:
                        return new Ingrediente(riga);
                }
            } else
            {
                return new Ingrediente(riga);
            }
        }

    }

    public class Ingredienti : List<Ingrediente>
    {
        public Ingredienti() { }
        public Ingredienti(string nomeFile)
        {
            StreamReader fin = new StreamReader(nomeFile);
            fin.ReadLine();

            while (!fin.EndOfStream)
            {
                Add(Ingrediente.MakeIngrediente(fin.ReadLine()));
            }
        }
    }



    public class IngredienteCol : Ingrediente
    {
        protected string colorante;
        public string Colorante { get; set; }
        public IngredienteCol() { }
        public IngredienteCol(string riga) : base(riga)
        {
            string[] stringa = riga.Split(";");

            //string amoguss;
            //string.TryParse(stringa[3], out amoguss);  
            Colorante = stringa[3].ToString();
        }
    }

    public class IngredienteCal : Ingrediente
    {
        protected string panna;
        public string Panna { get; set; }

        public IngredienteCal() { }

        public IngredienteCal(string riga) : base(riga)
        {
            string[] stringa = riga.Split(";");

            //TipoIngrediente amogus;
            //Enum.TryParse(stringa[3], out amogus);
            //Panna = amogus;
            Panna = stringa[3].ToString();
        }
    }

    public class IngredienteLat : Ingrediente
    {
        protected string latte;
        public string Latte { get; set; }

        public IngredienteLat() { }

        public IngredienteLat(string riga) : base(riga)
        {
            string[] stringa = riga.Split(";");

            //TipoIngrediente amogus;
            //Enum.TryParse(stringa[3], out amogus);
            Latte = stringa[3].ToString();
            //Latte = amogus;
        }
    }
}