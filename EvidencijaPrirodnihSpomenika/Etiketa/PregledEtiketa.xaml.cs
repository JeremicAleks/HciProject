using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace EvidencijaPrirodnihSpomenika.Etiketa
{
    /// <summary>
    /// Interaction logic for PregledEtiketa.xaml
    /// </summary>
    public partial class PregledEtiketa : Window, INotifyPropertyChanged
    {
        string dbConn = "Data Source=database.db;Version=3;";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        private ICollectionView _View;
        public ICollectionView View
        {
            get
            {
                return _View;
            }
            set
            {
                _View = value;
                OnPropertyChanged("View");
            }
        }


        public PregledEtiketa()
        {
            InitializeComponent();
            FillDataGrid();

        }

        private void ZatvoriButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void FillDataGrid()
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from etiketa";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("etiketa");
                dataAdp.Fill(dt);
                dgrEtiketa.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqliteCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ObrisiButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            sqliteCon.Open();
            string upit = "select * from etiketa";
            SQLiteCommand comUpit = new SQLiteCommand(upit, sqliteCon);
            comUpit.ExecuteNonQuery();
            SQLiteDataReader drUp = comUpit.ExecuteReader();
            int postoji = 0;
            while (drUp.Read())
            {
                postoji++;
            }

            if (postoji == 0)
            {
                MessageBox.Show("Ne postoji etiketa koja moze biti obirsana");
                Close();
                return;
                
            }
            sqliteCon.Close();
            if (MessageBox.Show("Da li ste sigurni da zelite da obrisete?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    sqliteCon.Open();
                    string query = "select * from spomenik where oznakaTag=@ozn";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.Parameters.AddWithValue("@ozn", oznaTxt.Text);
                    command.ExecuteNonQuery();
                    SQLiteDataReader dr = command.ExecuteReader();

                    int oznaka = 0;
                    while (dr.Read())
                    {
                        oznaka++;
                    }

                    if (oznaka >= 1)
                    {
                        MessageBox.Show("Etiketa ne moze biti obrisana jer je dodata Spomeniku", "Greska");
                        return;
                    }

                    sqliteCon.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                try
                {
                    sqliteCon.Open();
                    string query = "delete from etiketa where oznaka=@ozn1";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.Parameters.AddWithValue("@ozn1", oznaTxt.Text);
                    command.ExecuteNonQuery();
                    string query2 = "delete from spomEtiketa where oznakaEtiketa=@ozn2";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.Parameters.AddWithValue("@ozn2", oznaTxt.Text);
                    command2.ExecuteNonQuery();

                    FillDataGrid();
                    MessageBox.Show("Etiketa je uspešno obrisana", "Obrisano");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void IzmeniButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            //SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            sqliteCon.Open();
            string upit = "select * from etiketa";
            SQLiteCommand comUpit = new SQLiteCommand(upit, sqliteCon);
            comUpit.ExecuteNonQuery();
            SQLiteDataReader drUp = comUpit.ExecuteReader();
            int postoji = 0;
            while (drUp.Read())
            {
                postoji++;
            }

            if (postoji == 0)
            {
                MessageBox.Show("Ne postoji etiketa koja moze biti izmenjena");
                Close();
                return;

            }
            sqliteCon.Close();
            try
            {
                sqliteCon.Open();
                string query = "update etiketa set opis=@opis,boja='" + this.bojaPicker.SelectedColor.ToString() + "'where oznaka=@ozn3";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.Parameters.AddWithValue("@opis", opisTxt.Text);
                command.Parameters.AddWithValue("@ozn3", oznaTxt.Text);
                command.ExecuteNonQuery();

                FillDataGrid();
                MessageBox.Show("Uspešno ste izvršili izmenu", "Izmenjeno");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[1]); //fokus na aktivan prozor

            if (focusedControl is DependencyObject)
            {

                string str = EvidencijaPrirodnihSpomenika.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                DependencyObject dp = this.Parent;
                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp(str, (MainWindow)dp);

            }
            else
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("PregledEtiketa", mw);

            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            Boolean isWindowOpen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Etiketa.DodajEtiketa)
                {

                    w.Activate();
                    isWindowOpen = true;
                    if (w.WindowState == WindowState.Minimized)
                        w.WindowState = WindowState.Normal;

                }
            }
            
            if (!isWindowOpen)
            {

                var etik = new Etiketa.DodajEtiketa();
                etik.Show();
            }
            Close();
        }
    }
}
