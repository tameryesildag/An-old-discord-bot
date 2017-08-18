using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmileBot
{
    
    class Replacer
    {
       public string replaced = " ";
        public void Replace(string metin)
        {
          metin = metin.Replace('+', ' ');
          replaced = metin;
        }
    }
}
