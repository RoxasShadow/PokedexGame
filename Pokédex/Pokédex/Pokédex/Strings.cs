using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Pokédex {
    public static class Strings {
        public static Hashtable GetString() {
            Hashtable strings = new Hashtable();
            switch(Utils.Language) {
                case RNGReporter.Language.English:
                    strings.Add("intro", "Enter => play\nP     => Pause\nAlt   => (de)active the sound\n-/+   => (de)/(in)crease the volume\nE/I   => English/Italian\n\nAlign the Pokemons with the same abilities and win!");
                    strings.Add("last", "Last");
                    strings.Add("points", "Points");
                    strings.Add("ability", "Ability");
                    strings.Add("mf", "M/F");
                    strings.Add("hp", "HP");
                    strings.Add("atk", "Atk");
                    strings.Add("def", "Def");
                    strings.Add("spatk", "SpAtk");
                    strings.Add("spdef", "SpDef");
                    strings.Add("speed", "Speed");
                    break;
                case RNGReporter.Language.Italian:
                    strings.Add("intro", "Invio => gioca\nP     => Pausa\nAlt   => (dis)attiva il suono\n-/+   => aumenta/riduci il volume\nE/I   => Inglese/Italiano\nAllinea i Pokemon con la stessa abilita' e vinci!");
                    strings.Add("last", "Ultimo");
                    strings.Add("points", "Punti");
                    strings.Add("ability", "Abilita'");
                    strings.Add("mf", "M/F");
                    strings.Add("hp", "PS");
                    strings.Add("atk", "Atk");
                    strings.Add("def", "Dif");
                    strings.Add("spatk", "AtkSp");
                    strings.Add("spdef", "DifSp");
                    strings.Add("speed", "Vel");
                    break;
            }
            return strings;
        }
    }
}
