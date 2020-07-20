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

namespace EvidencijaPrirodnihSpomenika.Etiketa
{
    /// <summary>
    /// Interaction logic for DodajEtiketa.xaml
    /// </summary>
    public partial class DodajEtiketa : Window, INotifyPropertyChanged
    {
        string dbConn = "Data Source=database.db;Version=3;";
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
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

        public DodajEtiketa()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OtkaziButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.oznakaTxt.Text.Equals(""))
            {
                MessageBox.Show("Polje Oznaka ne sme biti prazno", "Greška");
            }
            else
            {
                
              
                    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                    try
                    {
                        String colo = BojaPicker.SelectedColor.ToString();

                        sqliteCon.Open();
                        string query = "insert into etiketa (oznaka,opis,boja) values(@id,@opis,'" + colo + "') ";
                        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        command.Parameters.AddWithValue("@id", oznakaTxt.Text);
                        command.Parameters.AddWithValue("@opis", opisTxt.Text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Uspesno ");
                        //sqliteCon.Close();
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
      

        private void oznakaTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z|^a-z|^0-9]+").IsMatch(e.Text);
        }

        private void oznakaTxt_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[1]); //fokus na aktivan prozor

            if (focusedControl is DependencyObject)
            {

                string str =EvidencijaPrirodnihSpomenika.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                DependencyObject dp = this.Parent;
                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp(str, (MainWindow)dp);

            }
            else
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("DodajEtiketa", mw);

            }
        }
    }
}
