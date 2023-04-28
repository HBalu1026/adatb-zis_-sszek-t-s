using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMySQL_gyak
{
    public class Termek
    {
        string kategoria;
        string gyarto;
        string nev;
        int ar;
        int garido;

        public Termek(string kategoria, string gyarto, string nev, int ar, int garido)
        {
            this.kategoria = kategoria;
            this.gyarto = gyarto;
            this.nev = nev;
            this.ar = ar;
            this.garido = garido;
        }

        public String ToCSVString()
        {
            return $"{this.kategoria};{this.gyarto};{this.nev};{this.ar};{this.garido}";
        }

        public static String ToCSVString(Termek par)
        {
            return $"{par.kategoria};{par.gyarto};{par.nev};{par.ar};{par.garido}";
        }

        public string Kategoria { get => kategoria; set => kategoria = value; }
        public string Gyarto { get => gyarto; set => gyarto = value; }
        public string Nev { get => nev; set => nev = value; }
        public int Ar { get => ar; set => ar = value; }
        public int Garido { get => garido; set => garido = value; }

    }
}
