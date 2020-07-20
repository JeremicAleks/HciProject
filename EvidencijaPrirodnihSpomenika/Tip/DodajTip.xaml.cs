using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EvidencijaPrirodnihSpomenika.Tip
{
    /// <summary>
    /// Interaction logic for DodajTip.xaml
    /// </summary>
    public partial class DodajTip : Window, INotifyPropertyChanged
    {
        string dbConn = "Data Source=database.db;Version=3;";
        BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/icon.png"));
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public DodajTip()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private String _oznakaVal;

        public String OznakaVal
        {
            get
            {
                return _oznakaVal;
            }
            set
            {
                if (value != _oznakaVal)
                {
                    _oznakaVal = value;
                    OnPropertyChanged("OznakaVal");
                }
            }
        }

        private String _nazivVal;

        public String NazivVal
        {
            get
            {
                return _nazivVal;
            }
            set
            {
                if (value != _nazivVal)
                {
                    _nazivVal = value;
                    OnPropertyChanged("NazivVal");
                }
            }
        }
        private void OtkaziButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void IkonicaButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog rezultat = new Microsoft.Win32.OpenFileDialog();
            rezultat.Filter = "Image Files(*.BMP;*.JPG;*PNG)|*.BMP;*.JPG;*PNG";

            if (rezultat.ShowDialog() == true)
            {
                this.ikonica.Source = new BitmapImage(new Uri(rezultat.FileName));
                icon = new BitmapImage(new Uri(rezultat.FileName));
            }
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.oznakaTxt.Text.Equals(""))
            {
                MessageBox.Show("Polje Oznaka ne sme biti prazno", "Greška");
            }
            else if (this.nazivTxt.Text.Equals(""))
            {
                MessageBox.Show("Polje Naziv ne sme biti prazno", "Greška");
            }
            else
            {

                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                try
                {
                    sqliteCon.Open();
                    string query = "insert into tip (oznaka,naziv,opis,ikonica) values('" + this.oznakaTxt.Text + "','" + this.nazivTxt.Text + "','" + this.opisTxt.Text + "','" + icon + "') ";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ");
                    Close();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Oznaka vec postoji!");

                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }


        private void oznakaTxt_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void oznakaTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z|^a-z|^0-9]+").IsMatch(e.Text);
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

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("DodajTip", mw);

            }
        }
    }
}
