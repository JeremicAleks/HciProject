using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvidencijaPrirodnihSpomenika.Tutorial
{
    /// <summary>
    /// Interaction logic for Tutor.xaml
    /// </summary>
    public partial class Tutor : Window
    {
        public int korak = 0;
        public int slikaKorak = 0;
         public Tutor()
        {
            InitializeComponent();
        }

        public void pocetak()
        {
            korak = 1;
            Naslov.Text = "Dobrodosli u tutorijal za kreiranje spomenika";
            Koraci.Text = "Za kreiranje spomenika potrebno je misem kliknuti na ikonicu (pogledati sliku) za dodavanje spomenika.Nakon klika otvara se dijalog za dodavanje spomenika koji je potrebno popuniti.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Kliknite na ikonicu za dodavanje.";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/klikIkonica.JPG"));
        }

        public void UnosNaziva()
        {
            korak = 3;
            Naslov.Text = "Nakon sto unesete oznaku spomenika,potrebno je uneti naziv";
            Koraci.Text = "Ispod polja u kom ste unosili oznaku nalazi se polje za unos naziva prirodnog spomenika.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "U polje upisite naziv prirodnog spomenika.";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/NazivTuto.JPG"));

        }

        public void UnosOznake()
        {
            korak = 2;
            Naslov.Text = "Uspesno ste otvorili dijalog za dodavanje spomenika";
            Koraci.Text = "Potrebno je uneti jedinstvenu oznaku spomenika.U polje Oznaka unesite oznaku spomenika.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "U polje unesite oznaku prirodnog spomenika";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/oznakaTuto.jpg"));
        }
        public void IzborTipa()
        {
            korak = 4;
            Naslov.Text = "Sledeci obavezan korak je izbor tipa";
            Koraci.Text = "Kada ste uneli oznaku i naziv,sledece sto je neophodno uraditi je izabrati tip spomenika iz padajuce liste ili unosom oznake.Pre izbora tipa mozete uneti opis spomenika, i izabrati ikonicu.Opis se unosi na identican nacin kao unos naziva iz prethodog koraka.Ukoliko je potrebno uneti ikonicu, kliknite na dugme \"Prikazi\" za tutorijal.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Izaberite Tip";
            Ikonica.Visibility = Visibility.Visible;
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/tipTuto.JPG"));
            CloseSlika.Visibility = Visibility.Hidden;
            Close.Visibility = Visibility.Visible;
        }

        public void IzborTipa1()
        {
            korak = 4;
            Naslov.Text = "Sledeci obavezan korak je izbor tipa";
            Koraci.Text = "Uspesno ste dodali ikonicu prirodnog spomenika. Ona ce biti koriscenja za prikazivanje spomenika na mapi.Sledece sto je neophodno uraditi je izabrati tip spomenika iz padajuce liste. ";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Izaberite Tip";
            Ikonica.Visibility = Visibility.Hidden;
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/tipTuto.JPG"));
            Close.Visibility = Visibility.Visible;
        }

       

        public void IzborSlikeKorak1()
        {
            CloseSlika.Visibility = Visibility.Visible;
            Close.Visibility = Visibility.Hidden;

            slikaKorak = 1;
            Naslov.Text = "Izbor Slike ";
            Koraci.Text = "Klikom na dugme za izbor slike, otvara se dijalog gde je potrebno izabrati sliku sa racunara.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Kliknite da dugme \"Izaberi\",nakon izbora slike kliknite na dugme \"Open\"";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/IkonicaTuto.JPG"));
            Ikonica.Visibility = Visibility.Hidden;
        }

        public void IzborDatuma()
        {
            CloseSlika.Visibility = Visibility.Hidden;
            Close.Visibility = Visibility.Visible;

            korak = 5;
            Ikonica.Visibility = Visibility.Hidden;
            Naslov.Text = "Unos podataka,izbor datuma";
            Koraci.Text = "U ovom koraku potrebno je uneti podatke o spomeniku.Klima i turisticki status se biraju iz padajuce liste.U polju prihod mozete unositi samo brojeve.Ostale podatke unesite preko CheckBoxa,po defaultu je ne  .Klikom na kalendar pored polja otvara se kalendar iz kog je potrebno izabrati odgovarajuci datum.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Kliknite na ikonicu kalendara, i izaberite datum";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/DatumTuto.JPG"));
        }
        public void KlikniDugme()
        {
            korak = 6;
            Naslov.Text = "Dodavanje spomenika sa unetim podacima";
            Koraci.Text = "Nakon sto ste uneli sve podatke spomenika,kliknite misem na dugme \"Dodaj\".Ukoliko ste sve uneli kako treba prikazace se poruka o uspesnosti dodavanja. ";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Kliknite na dugme \"Dodaj\"";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/DugmeTuto.JPG"));
        }

        public void uspeh()
        {
            korak = 7;
            Naslov.Text = "Prirodni spomenik je uspesno dodat";
            Koraci.Text = "Uspesno ste dodali prirodni spomenik.Kliknite na dugme \"Ok\" da ugasite poruku.Ovim korakom je tutorijal zavrsen.";
            Koraci.TextWrapping = TextWrapping.Wrap;
            Sta.Text = "Kliknite na dugme \"Ok\"";
            Slika.Source = new BitmapImage(new Uri("pack://application:,,,/Images/DodatoTuto.JPG"));
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       

        private void CloseSlika_Click_1(object sender, RoutedEventArgs e)
        {
           
            IzborTipa();
        }

        private void Ikonica_Click(object sender, RoutedEventArgs e)
        {
            IzborSlikeKorak1();
        }
    }
}
