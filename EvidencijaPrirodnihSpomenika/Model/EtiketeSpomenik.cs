using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidencijaPrirodnihSpomenika.Model
{
    public class EtiketeSpomenik
    {
        private string _oznakaSpom;
        private string _oznakaEtiketa;

        public String OznakaSpom
        {
            get
            {
                return _oznakaSpom;
            }
            set
            {
                _oznakaSpom = value;
            }

        }
        public String OznakaEtiketa
        {
            get
            {
                return _oznakaEtiketa;
            }
            set
            {
                _oznakaEtiketa= value;

            }
        }
    }
}
