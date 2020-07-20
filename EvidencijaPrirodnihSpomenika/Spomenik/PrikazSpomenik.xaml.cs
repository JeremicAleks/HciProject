using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PrikazSpomenik.xaml
    /// </summary>
    public partial class PrikazSpomenik : Window
    {
        string dbConn = "Data Source=database.db;Version=3;";
        public PrikazSpomenik()
        {
            InitializeComponent();
        }
        public PrikazSpomenik(String oznakaSlike)
        {
            InitializeComponent();
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            string oznaka = null;
            string naziv = null;
            try
            {
                sqliteCon.Open();
                string query = "select * from spomenik where oznaka='"+oznakaSlike+"'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
               
                while (dr.Read())
                {
                   
                    oznaka = dr.GetString(0);
                    naziv = dr.GetString(1);
                    

                }
                sqliteCon.Close();
                OznakaText.Text = oznaka;
                NazivText.Text = naziv;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void IkonicaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IzaberiTipButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IzaberiEtietuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OtkaziButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void prihod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
