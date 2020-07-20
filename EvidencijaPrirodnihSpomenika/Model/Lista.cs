using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidencijaPrirodnihSpomenika.Model
{
    public class Lista
    {

        public String _ikonica;
        private String _naziv;
        private String _oznaka;


        public String Ikonica
        {
            get
            {
                return _ikonica;
            }
            set
            {
                _ikonica = value;
            }

        }
        public String Naziv
        {
            get
            {
                return _naziv;
            }
            set
            {
                _naziv = value;

            }
        }
        public String Onaka
        {
            get
            {
                return _oznaka;
            }
            set
            {
                _oznaka = value;
            }
        }

    }
}
