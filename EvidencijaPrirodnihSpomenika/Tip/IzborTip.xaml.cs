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
    /// Interaction logic for IzborTip.xaml
    /// </summary>
    public partial class IzborTip : Window, INotifyPropertyChanged
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
        public IzborTip()
        {
            InitializeComponent();
            FillDataGrid();
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

        private void ZatvoriButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            var dodaj = new DodajTip();
            dodaj.Show();
            Close();
        }

        private void IzaberiButton_Click(object sender, RoutedEventArgs e)
        {
            OznakaTip = oznaTxt.Text;
            this.DialogResult = true;

        }

        private string _oznakaTip;

        public string OznakaTip
        {
            get { return _oznakaTip; }
            set { _oznakaTip = value; }
        }
    }
}
