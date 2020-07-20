using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EvidencijaPrirodnihSpomenika.Validacija
{
    class Validacija
    {
    }

    public class ValidacijaTekstaa : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                if (s.Equals("") || s == null)
                {
                    return new ValidationResult(false, "Polje ne sme biti prazno!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greska.");
            }


        }

    }

    public class ValidacijaDuzinaNaziv : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                if (s.Length>40)
                {
                    return new ValidationResult(false, "Ne možete uneti više od 40 karakatera!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greška.");
            }


        }

    }

    public class ValidacijaDuzinaOznaka : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                if (s.Length > 20)
                {
                    return new ValidationResult(false, "Ne možete uneti više od 20 karakatera!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greška.");
            }


        }

    }

    public class ValidacijaDuzinaOpis : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                if (s.Length > 500)
                {
                    return new ValidationResult(false, "Ne možete uneti više od 500 karakatera!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greška.");
            }


        }

    }

    public class ValidacijaSadrzaja : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                string pattern = @"^[0-9A-Za-z]+$";
                if (!Regex.IsMatch(s, pattern))
                {

                    return new ValidationResult(false, "Polje mora da sadrzi samo slova i brojeve!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greska.");
            }


        }

    }



    public class ValidacijaSlova : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                string pattern = @"^[A-Za-z]+$";
                if (!Regex.IsMatch(s, pattern))
                {

                    return new ValidationResult(false, "Polje može da sadrzi samo slova!");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greska.");
            }


        }

    }

    public class ValidacijaRazmak : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                if (String.IsNullOrWhiteSpace(s))
                {
                    return new ValidationResult(false, "Polje se ne sme sastojati samo od razmaka");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greska.");
            }
        }
    }

    public class ValidacijaPrihod : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;

                try
                {

                    if (s.Equals(""))
                    {
                        return new ValidationResult(true, "");
                    }

                    double prihod = Double.Parse(s);

                    
                    if (prihod < 0)
                    {
                        
                        return new ValidationResult(false, "Dozvoljeni samo pozitivni brojevi");
                        
                    }
                    else
                    {
                        return new ValidationResult(true, "");
                    }
                    
                }
                catch
                {
                    return new ValidationResult(false, "Dozvoljeni samo pozitivni brojevi");
                }
            }
            catch
            {
                return new ValidationResult(false, " Nepoznata greska.");
            }
        }
    }

}
