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

namespace EvidencijaPrirodnihSpomenika.Tip
{
    /// <summary>
    /// Interaction logic for PregledTip.xaml
    /// </summary>
    public partial class PregledTip : Window, INotifyPropertyChanged
    {
        string dbConn = "Data Source=database.db;Version=3;";
        string icon = null;
        Boolean flag = false;
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

        public PregledTip()
        {
            InitializeComponent();
            //this.DataContext = this;
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
                string query = "select * from tip";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("tip");
                dataAdp.Fill(dt);
                grdTipovi.ItemsSource = dt.DefaultView;
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
            string upit = "select * from tip";
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
                MessageBox.Show("Ne postoji tip koji moze biti obirsan");
                return;
            }
            sqliteCon.Close();
            if (MessageBox.Show("Da li ste sigurni da zelite da obrisete?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                
                try
                {
                    sqliteCon.Open();
                    string query = "select * from spomenik where oznakaTip='" + this.oznaTxt.Text + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();
                    SQLiteDataReader dr = command.ExecuteReader();

                    int oznaka = 0;
                    while (dr.Read())
                    {
                        oznaka++;
                    }

                    if (oznaka >= 1)
                    {
                        MessageBox.Show("Tip ne moze biti obrisan jer pripeda Spomeniku", "Greska");
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
                    string query = "delete from tip where oznaka='" + this.oznaTxt.Text + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Uspesno obirsano", "Obrisano");
                    FillDataGrid();

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
            sqliteCon.Open();
            string upit = "select * from tip";
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
                MessageBox.Show("Ne postoji tip koji moze biti izmenjen");
                return;
            }
            sqliteCon.Close();

            string ikona;
            if (flag)
            {
                ikona = icon;
                flag = false;
            }
            else
            {
                if (this.Ikonica.Source != null)
                    ikona = this.Ikonica.Source.ToString();
                else
                    ikona = "";
            }

            //SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "update tip set naziv='" + this.nazivTxt.Text + "',opis='" + this.opisTxt.Text + "',ikonica='" + ikona + "'where oznaka='" + this.oznaTxt.Text + "'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                FillDataGrid();

                MessageBox.Show("Uspešno ste izvršili izmenu", "Izmenjeno");
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);



            }
        }

        private void PromeniIkonicuButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog rezultat = new Microsoft.Win32.OpenFileDialog();
            rezultat.Filter = "Image Files(*.BMP;*.JPG;*PNG)|*.BMP;*.JPG;*PNG";

            if (rezultat.ShowDialog() == true)
            {
                Ikonica.Source = new BitmapImage(new Uri(rezultat.FileName));
                icon = rezultat.FileName;
                flag = true;
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

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("PregledTipa", mw);

            }
        }

        private void DodajBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean isWindowOpen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tip.DodajTip)
                {

                    w.Activate();
                    isWindowOpen = true;
                    if (w.WindowState == WindowState.Minimized)
                        w.WindowState = WindowState.Normal;

                }
            }
           
            
            if (!isWindowOpen)
            {

                var tip = new Tip.DodajTip();
                tip.Show();
            }

            Close();
            
        }
    }
}
