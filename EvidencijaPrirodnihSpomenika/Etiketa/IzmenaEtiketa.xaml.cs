using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for IzmenaEtiketa.xaml
    /// </summary>
    public partial class IzmenaEtiketa : Window
    {
        string oznakaSp;
        public List<Model.EtiketeSpomenik> dodate = new List<Model.EtiketeSpomenik>();
        public List<Model.EtiketeSpomenik> nisuDodate = new List<Model.EtiketeSpomenik>();

        string dbConn = "Data Source=database.db;Version=3;";
        public IzmenaEtiketa(string ozn)
        {
            InitializeComponent();
            fillDodato(ozn);
            fillNijeDoodato(ozn);
        }

        public ObservableCollection<Model.EtiketeSpomenik> ListaDodate{get;set;}
        public ObservableCollection<Model.EtiketeSpomenik> ListaNisuDodate{ get; set; }
        public void fillDodato(string ozn)
        {
            oznakaSp = ozn;
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from SpomEtiketa where OznakaSpom='"+ozn+"'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznakaEtike = dr.GetString(1);
                    dodate.Add(new Model.EtiketeSpomenik {OznakaSpom=ozn,OznakaEtiketa=oznakaEtike});
                }
                sqliteCon.Close();
                ListaDodate = new ObservableCollection<Model.EtiketeSpomenik>(dodate);
                ListaDodata.ItemsSource = ListaDodate;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void fillNijeDoodato(string ozn)
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "Create view tab as select * from SpomEtiketa where OznakaSpom ='"+ozn+"'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();
                string query1 = "Select OznakaSpom, Oznaka from etiketa left outer join tab on etiketa.oznaka = tab.OznakaEtiketa";
                SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                command1.ExecuteNonQuery();


                SQLiteDataReader dr = command1.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.IsDBNull(0))
                    {
                        nisuDodate.Add(new Model.EtiketeSpomenik { OznakaEtiketa = dr.GetString(1),OznakaSpom=null });
                    }
                }
                string etikete = "drop view tab";
                SQLiteCommand etikeComm = new SQLiteCommand(etikete, sqliteCon);
                etikeComm.ExecuteNonQuery();
                sqliteCon.Close();
                ListaNisuDodate = new ObservableCollection<Model.EtiketeSpomenik>(nisuDodate);
                ListaNije.ItemsSource = ListaNisuDodate;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void PotvrdiBtn_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from SpomEtiketa where OznakaSpom='"+oznakaSp+"'";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                foreach (Model.EtiketeSpomenik ls in ListaDodate)
                { 
                    if (ls.OznakaSpom == null)
                    {
                        //string query1 = "insert into spomEtiketa (oznakaSpom,oznakaEtiketa) values(@ozSp,@ozEt) ";
                        string query1 = "insert into spomEtiketa(oznakaSpom,oznakaEtiketa) values('"+oznakaSp+"','"+ls.OznakaEtiketa+"')";
                        SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                        //command1.Parameters.AddWithValue("@ozSp", oznakaSp);
                        //command1.Parameters.AddWithValue("@ozEt", ls.OznakaEtiketa);
                        command1.ExecuteNonQuery();
                    }
                   
                }
                foreach(Model.EtiketeSpomenik es in ListaNisuDodate)
                {
                    string query1 = "delete from spomEtiketa where oznakaSpom='"+oznakaSp+"'and oznakaEtiketa='"+es.OznakaEtiketa+"'";
                    SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                   // command1.Parameters.AddWithValue("@ozSp", oznakaSp);
                    //command1.Parameters.AddWithValue("@ozEt", es.OznakaEtiketa);
                    command1.ExecuteNonQuery();
                }

                sqliteCon.Close();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DodajBtn_Click(object sender, RoutedEventArgs e)
        {
            Model.EtiketeSpomenik es = ListaNije.SelectedItem as Model.EtiketeSpomenik;
            if (es == null)
            {
                MessageBox.Show("Morate selektovati etiketu koja nije dodata");
                return;
            }
            nisuDodate.Remove(es);
            ListaNisuDodate.Remove(es);
            dodate.Add(es);
            ListaDodate.Add(es);
            
        }

        private void IzbaciBtn_Click(object sender, RoutedEventArgs e)
        {
            Model.EtiketeSpomenik es = ListaDodata.SelectedItem as Model.EtiketeSpomenik;
            
            if (es == null)
            {
                MessageBox.Show("Selektujte dodatu etiketu");
                return;
            }

            dodate.Remove(es);
            ListaDodate.Remove(es);
            nisuDodate.Add(es);
            ListaNisuDodate.Add(es);
            
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[2]); //fokus na aktivan prozor

            if (focusedControl is DependencyObject)
            {

                string str = EvidencijaPrirodnihSpomenika.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                DependencyObject dp = this.Parent;
                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("IzmenaEtikete", (MainWindow)dp);
            }
            else
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("IzmenaEtikete", mw);
            }
        }
    }
}
