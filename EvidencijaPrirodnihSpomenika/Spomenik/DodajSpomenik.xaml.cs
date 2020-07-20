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

namespace EvidencijaPrirodnihSpomenika.Spomenik
{
    /// <summary>
    /// Interaction logic for DodajSpomenik.xaml
    /// </summary>
    public partial class DodajSpomenik : Window, INotifyPropertyChanged
    {
        string dbConn = "Data Source=database.db;Version=3;";

        static int VALIDATION_DELAY = 1500;
        System.Threading.Timer timer = null;

        BitmapImage icon = null;
        string ikonicaTip = null;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        
        //TODO 2: SLIKA U BAZU 
        public DodajSpomenik()
        {
            InitializeComponent();
            fillTipCombo();
            fillEtiketaBox();
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

        private String _oznakaValTip;

        public String OznakaValTip
        {
            get
            {
                return _oznakaValTip;
            }
            set
            {
                if (value != _oznakaValTip)
                {
                    _oznakaValTip = value;
                    OnPropertyChanged("OznakaValTip");
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

        private String _prihodVal;

        public String PrihodVal
        {
            get
            {
                return _prihodVal;
            }
            set
            {
                if (value != _prihodVal)
                {
                    _prihodVal = value;
                    OnPropertyChanged("PrihodVal");
                }
            }
        }

        public void fillEtiketaBox()
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from etiketa";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznka = dr.GetString(0);
                    //this.TipCombo.Items.Add(oznka + " - " + naziv);
                    this.ListaEtikete.Items.Add(oznka);
                }
                sqliteCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.OznakaText.Text.Equals(""))
            {
                MessageBox.Show("Polje Oznaka ne sme biti prazno", "Greška");
            }
            else if (this.NazivText.Text.Equals(""))
            {
                MessageBox.Show("Polje Naziv ne sme biti prazno", "Greška");
            }
            else if (TipCombo.Text.Equals(""))
            {
                MessageBox.Show("Morate izabrati tip spomenika", "Greška");
            }
            else
            {
                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);

                string[] split = this.TipCombo.Text.Split('-');
                string ozn = "";
                foreach (char c in split[0])
                {
                    if (!c.Equals(' '))
                    {
                        ozn += c;
                    }
                }
                


                try
                {
                    sqliteCon.Open();
                    string query1 = "select * from tip where oznaka=@ozn1";
                    SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                    command1.Parameters.AddWithValue("@ozn1", ozn);
                    command1.ExecuteNonQuery();
                    SQLiteDataReader dr = command1.ExecuteReader();                

                    int oznaka = 0;
                    while (dr.Read())
                    {
                        oznaka++;
                    }

                    if (oznaka < 1 )
                    {
                        MessageBox.Show("Uneti tip ne postoji");
                        return;
                    }


                    sqliteCon.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }






                foreach (var item in ListaEtikete.SelectedItems) 
                {
                    sqliteCon.Open();
                    string etike = "insert into spomEtiketa (OznakaSpom,OznakaEtiketa) values (@ozn,'" + item.ToString() + "')";
                    SQLiteCommand etikeComm = new SQLiteCommand(etike, sqliteCon);
                    etikeComm.Parameters.AddWithValue("@ozn", this.OznakaText.Text);
                    etikeComm.ExecuteNonQuery();
                    sqliteCon.Close();

                }
                
                try
                {
                    string eugrozen = "false";
                    if (eUgrozen.IsChecked.Equals(true))
                    {
                        eugrozen = "true";
                    }

                    string suvrsta = "false";
                    if (uVrste.IsChecked.Equals(true))
                    {
                        suvrsta = "true";
                    }
                    string nasregion = "false";
                    if (nasRegion.IsChecked.Equals(true))
                    {
                        nasregion = "true";
                    }
                    //string[] split = this.TipCombo.SelectionBoxItem.ToString().Split('-');
                    //string ozn = "";
                    //foreach (char c in split[0])
                    //{
                    //    if (!c.Equals(' '))
                    //    {
                    //        ozn += c;
                    //    }
                    //}

                    if (icon == null)
                    {
                        try
                        {
                            sqliteCon.Open();
                            string queryikona = "select * from tip where oznaka=@ozn1";
                            SQLiteCommand commandikona = new SQLiteCommand(queryikona, sqliteCon);
                            commandikona.Parameters.AddWithValue("@ozn1", ozn);
                            commandikona.ExecuteNonQuery();
                            string iconaa = "";
                            SQLiteDataReader dr = commandikona.ExecuteReader();
                            while (dr.Read())
                            {
                                 iconaa = dr.GetString(3);
                                

                            }
                            sqliteCon.Close();
                            icon = new BitmapImage(new Uri(iconaa));

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }


                    sqliteCon.Open();
                    string datum = Datum.SelectedDate.Value.Month.ToString() + "." + Datum.SelectedDate.Value.Day.ToString() + "." + Datum.SelectedDate.Value.Year.ToString() + ".";
                    //TODO 1: INSERT INTO OZNAKATAG PROMENI SA NECIM
                    string query = "insert into spomenik (oznaka,naziv,opis,klima,tstatus,eugrozen,suvrsta,nasregion,prihod,oznakaTip,oznakaTag,icon,datum) values (@oznaka,@naziv,@opis,'" + KlimaDropdown.SelectionBoxItem.ToString() + "','" + TstatusDropdown.SelectionBoxItem.ToString() + "','" + eugrozen + "','" + suvrsta + "','" + nasregion + "',@prihod,@oznTip,'" + "Etiketa" + "',@icon,@datum)";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.Parameters.AddWithValue("@oznaka", this.OznakaText.Text);
                    command.Parameters.AddWithValue("@naziv", this.NazivText.Text);
                    command.Parameters.AddWithValue("@opis", this.OpisText.Text);
                    command.Parameters.AddWithValue("@prihod", this.prihod.Text);
                    command.Parameters.AddWithValue("@oznTip", ozn);
                    command.Parameters.AddWithValue("@icon", icon);
                    command.Parameters.AddWithValue("@datum",datum);
                    command.ExecuteNonQuery();
                    sqliteCon.Close();
                    sqliteCon.Open();
                   
                    string query1 = "insert into zaDodavanje (oznaka,naziv,ikonica) values (@ozne,@nazive,@iconi)";
                    SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                    command1.Parameters.AddWithValue("@ozne", this.OznakaText.Text);
                    command1.Parameters.AddWithValue("@nazive", this.NazivText.Text);
                    command1.Parameters.AddWithValue("@iconi", icon);

                    command1.ExecuteNonQuery();
                    sqliteCon.Close();


                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    //mw.popuniListu();  
                    mw.list.Add(new Model.Lista { Ikonica = icon.ToString(), Naziv = NazivText.Text, Onaka = OznakaText.Text });
                    mw.Listaspom.Add(new Model.Lista { Ikonica = icon.ToString(), Naziv = NazivText.Text, Onaka = OznakaText.Text });


                    var hehe = new Tutorial.Dodato();
                    hehe.Show();

                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is Tutorial.Tutor)
                        {

                            w.Activate();
                            Tutorial.Tutor tutor = w as Tutorial.Tutor;
                            tutor.uspeh();

                        }
                    }


                    
                    

                    Close();


                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Oznaka vec postoji!");

                    }else if (ex.Message.Contains("Nullable object"))
                    {
                        MessageBox.Show("Morate uneti datum");
                    }
                    else 
                    {
                        MessageBox.Show(ex.Message);
                    }
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
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Tutorial.Tutor)
                    {
                        Tutorial.Tutor tutor = w as Tutorial.Tutor;

                        if (tutor.slikaKorak == 1)
                        {
                            w.Activate();
                            tutor.IzborTipa1();
                        }

                    }
                }
                this.ikonica.Source = new BitmapImage(new Uri(rezultat.FileName));
                icon = new BitmapImage(new Uri(rezultat.FileName));
            }


        }

        

        private void fillTipCombo()
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from tip";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznka = dr.GetString(0);
                    
                    this.TipCombo.Items.Add(oznka);

                }
                sqliteCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        //private void OznakaText_KeyUp(object sender, KeyEventArgs e)
        //{
        //    foreach (Window w in Application.Current.Windows)
        //    {
        //        if (w is Tutorial.Tutor)
        //        {
        //            Tutorial.Tutor tutor = w as Tutorial.Tutor;
        //
        //            if (tutor.korak == 2)
        //            {
        //                w.Activate();
        //                tutor.UnosNaziva();
        //            }
        //
        //        }
        //    }
        //}

        //private void NazivText_KeyUp(object sender, KeyEventArgs e)
        //{
        //    foreach (Window w in Application.Current.Windows)
        //    {
        //        if (w is Tutorial.Tutor)
        //        {
        //
        //            Tutorial.Tutor tutor = w as Tutorial.Tutor;
        //
        //            if (tutor.korak == 3)
        //            {
        //                w.Activate();
        //                tutor.IzborTipa();
        //            }
        //
        //        }
        //    }
        //}

        private void TipCombo_DropDownClosed(object sender, EventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 4)
                    {
                        w.Activate();
                        tutor.IzborDatuma();
                    }

                }
            }
        }

        private void Datum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 5)
                    {
                        w.Activate();
                        tutor.KlikniDugme();
                    }
                }
            }
        }

        private void TimerElapsed(Object obj)
        {
            CheckSyntaxAndReport();
            DisposeTimer();
        }

        private void DisposeTimer()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
               
            }
            
        }

        private void OznakaText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            if (!origin.IsFocused)
            {

                return;
            }


            DisposeTimer();
            timer = new System.Threading.Timer(TimerElapsed, null, VALIDATION_DELAY, VALIDATION_DELAY);
            

        }

        private void CheckSyntaxAndReport()
        {
            this.Dispatcher.Invoke(() => oznakaTuto());
            

        }
        private void CheckSyntaxAndReport2()
        {
            this.Dispatcher.Invoke(() => NazivTuto());


        }
        public void oznakaTuto()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 2)
                    {
                        w.Activate();
                        tutor.UnosNaziva();
                    }

                }
            }
        }
        public void NazivTuto()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 3)
                    {
                        w.Activate();
                        tutor.IzborTipa();
                    }

                }
            }
        }

        private void NazivText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            if (!origin.IsFocused)
            {

                return;
            }


            DisposeTimer();
            timer = new System.Threading.Timer(TimerElapsed2, null, VALIDATION_DELAY, VALIDATION_DELAY);
        }
        private void TimerElapsed2(Object obj)
        {
            CheckSyntaxAndReport2();
            DisposeTimer();
        }

        private void OznakaText_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 2)
                    {
                        w.Activate();
                        tutor.UnosNaziva();
                        
                    }

                }
            }
        }

        private void NazivText_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 3)
                    {
                        w.Activate();
                        tutor.IzborTipa();
                    }

                }
            }
        }

       

        private void OznakaText_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void prihod_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
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

                string str = EvidencijaPrirodnihSpomenika.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                DependencyObject dp = this.Parent;
               EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp(str, (MainWindow)dp);
            }
            else
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("DodajSpomenik", mw);
            }
        }

        private void TipCombo_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 4)
                    {
                        w.Activate();
                        tutor.IzborDatuma();
                    }

                }
            }
        }

        private void TipCombo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Tab)
            {
                
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Tutorial.Tutor)
                    {

                        Tutorial.Tutor tutor = w as Tutorial.Tutor;

                        if (tutor.korak == 4)
                        {
                            w.Activate();
                            tutor.IzborDatuma();
                        }

                    }
                }
            }
        }
    }
}

