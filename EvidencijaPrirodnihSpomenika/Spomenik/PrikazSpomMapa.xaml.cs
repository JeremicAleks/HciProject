using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace EvidencijaPrirodnihSpomenika.Spomenik
{
    /// <summary>
    /// Interaction logic for PrikazSpomMapa.xaml
    /// </summary>
    public partial class PrikazSpomMapa : Window
    {
        public PrikazSpomMapa()
        {
            InitializeComponent();
        }
        string dbConn = "Data Source=database.db;Version=3;";
        public PrikazSpomMapa(String oznakaSlike)
        {
            InitializeComponent();
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            string oznaka = null;
            string naziv = null;
            string opis = null;
            string ikonica = null;
            try
            {
                sqliteCon.Open();
                string query = "select * from spomenik where oznaka='" + oznakaSlike + "'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {

                    oznaka = dr.GetString(0);
                    naziv = dr.GetString(1);
                    opis = dr.GetString(2);
                    ikonica = dr.GetString(11);

                }
                sqliteCon.Close();
                OznakaText.Text = oznaka;
                NazivText.Text = naziv;
                OpisText.Text = opis;
                this.ikonica.Source = new BitmapImage(new Uri(ikonica));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void PregledBtn_Click(object sender, RoutedEventArgs e)
        {
            var pspo = new Spomenik.PregledSpomenik();
            pspo.Show();
            Close();
        
        }



        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ObrisiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da zelite da obrisete spomenik?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                try
                {


                    sqliteCon.Open();
                    string query = "delete from spomenik where oznaka='" + OznakaText.Text + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();

                    string query2 = "delete from spomenikmapa where oznaka='" + OznakaText.Text + "'";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.ExecuteNonQuery();




                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    mw.fillMap();
                    Close();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
        }
    }
}
