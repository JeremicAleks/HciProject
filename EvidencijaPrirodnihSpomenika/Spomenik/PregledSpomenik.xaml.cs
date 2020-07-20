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

namespace EvidencijaPrirodnihSpomenika.Spomenik
{
    /// <summary>
    /// Interaction logic for PregledSpomenik.xaml
    /// </summary>
    public partial class PregledSpomenik : Window, INotifyPropertyChanged
    {
        String ikonicaaa;
        Boolean flag = false;
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


        public PregledSpomenik()
        {
            InitializeComponent();
            this.DataContext = this;
            FillDataGrid();
            FillFilterTip();
           
            FillTipDropDown();
        }

        private void FillTipDropDown()
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
                    
                    this.TipDropDown.Items.Add(oznka);
                }
                sqliteCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void FillFilterTip()
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
                    this.FilterTip.Items.Add(oznka);

                }
                sqliteCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        private void FillDataGrid()
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from spomenik";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("spomenik");
                dataAdp.Fill(dt);
                grdSpomenik.ItemsSource = dt.DefaultView;
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
            string upit = "select * from spomenik";
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
                MessageBox.Show("Ne postoji spomenik koji moze biti obirsan");
                return;
            }
            sqliteCon.Close();

            if (MessageBox.Show("Da li ste sigurni da zelite da obrisete?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string ozn = this.oznaTxt.Text;
                    sqliteCon.Open();
                    string query = "delete from spomenik where oznaka=@ozn";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.Parameters.AddWithValue("@ozn", this.oznaTxt.Text);
                    command.ExecuteNonQuery();

                    string brisEti = "delete from spomEtiketa where oznakaSpom=@ozsp";
                    SQLiteCommand commandBrEt = new SQLiteCommand(brisEti, sqliteCon);
                    commandBrEt.Parameters.AddWithValue("@ozsp", this.oznaTxt.Text);
                    commandBrEt.ExecuteNonQuery();


                    MessageBox.Show("Uspesno obirsano", "Obrisano");
                    FillDataGrid();


                    string query1 = "select * from spomenikmapa where oznaka=@ozn1";
                    SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                    command1.Parameters.AddWithValue("@ozn1", ozn);
                    command1.ExecuteNonQuery();
                    SQLiteDataReader dr = command1.ExecuteReader();

                    int oznaka = 0;
                    while (dr.Read())
                    {                       
                        oznaka++;
                    }

                    if (oznaka >= 1)
                    {
                        string query2 = "delete from spomenikmapa where oznaka=@ozn2";
                        SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                        command2.Parameters.AddWithValue("@ozn2", ozn);
                        command2.ExecuteNonQuery();
                        MainWindow mw = (MainWindow)Application.Current.MainWindow;
                        mw.fillMap();
                    }
                    else
                    {

                        string query2 = "delete from zaDodavanje where oznaka=@ozn2";
                        SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                        command2.Parameters.AddWithValue("@ozn2", ozn);
                        command2.ExecuteNonQuery();

                        MainWindow mw = (MainWindow)Application.Current.MainWindow;
                        
                        
                          int idx= mw.list.FindIndex(x => x.Onaka.Equals(ozn));

                        

                        mw.list.RemoveAt(idx);
                        mw.Listaspom.RemoveAt(idx);
                        
                        //mw.Listaspom.Remove((Model.Lista)mw.list.Find(x => x.Onaka.Equals(ozn)));

                        //mw.list.Remove((Model.Lista)mw.list.Find(x => x.Onaka.Equals(ozn)));

                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void ZatvoriButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IzmeniButton_Click(object sender, RoutedEventArgs e)
        {

            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            sqliteCon.Open();
            string upitpro = "select * from spomenik";
            SQLiteCommand comUpitpro = new SQLiteCommand(upitpro, sqliteCon);
            comUpitpro.ExecuteNonQuery();
            SQLiteDataReader drUp = comUpitpro.ExecuteReader();
            int postoji = 0;
            while (drUp.Read())
            {
                postoji++;
            }

            if (postoji == 0)
            {
                MessageBox.Show("Ne postoji spomenik koji moze biti izmenjen");
                return;
            }
            sqliteCon.Close();

            string ikona;
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
            string datum = Datum.SelectedDate.Value.Month.ToString() + "." + Datum.SelectedDate.Value.Day.ToString() + "." + Datum.SelectedDate.Value.Year.ToString() + ".";

            if (flag)
            {
                ikona = ikonicaaa;
                flag = false;
            }
            else
            {
                ikona = Ikonica.Source.ToString();
            }
            //SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);


            string[] split = this.TipDropDown.Text.Split('-');
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

                if (oznaka < 1)
                {
                    MessageBox.Show("Uneti tip ne postoji");
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
                string upit = "select * from spomenik where oznaka=@ozn3";
                SQLiteCommand upitComm = new SQLiteCommand(upit, sqliteCon);
                upitComm.Parameters.AddWithValue("@ozn3", oznaTxt.Text);
                upitComm.ExecuteNonQuery();

                SQLiteDataReader dr = upitComm.ExecuteReader();
                string oznTipa = null;
                string ikoTipa = null;
                while (dr.Read())
                {
                    oznTipa = dr.GetString(9);
                }

                if (!oznTipa.Equals(ozn))
                {
                    if (MessageBox.Show("Promenili ste tip.Da li želite da postavite ikonicu tog tipa za ikonicu spomenika?", "Promenjen tip", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        
                        string nadjiIkon = "select * from tip where oznaka='" + ozn + "'";
                        SQLiteCommand ikonComm = new SQLiteCommand(nadjiIkon, sqliteCon);
                        ikonComm.ExecuteNonQuery();

                        SQLiteDataReader drikon = ikonComm.ExecuteReader();


                        while (drikon.Read())
                        {
                            ikoTipa = drikon.GetString(3);

                        }
                        ikona = ikoTipa;

                    }
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
                string query = "update spomenik set naziv=@naziv,opis=@opis,klima='" + this.KlimaDropdown.SelectionBoxItem.ToString() + "',tstatus='" + TstatusDropdown.SelectionBoxItem.ToString() + "',eugrozen='" + eugrozen + "',suvrsta='" + suvrsta + "',nasregion='" + nasregion + "',prihod=@prihod,oznakaTip='" + ozn + "',oznakaTag='" + "this.tagTxt.Text" + "',datum=@datum,icon=@icon where oznaka=@oznaka";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.Parameters.AddWithValue("@naziv", this.nazivTxt.Text);
                command.Parameters.AddWithValue("@opis", this.opisTxt.Text);
                command.Parameters.AddWithValue("@prihod", this.prihodTxt.Text);
                command.Parameters.AddWithValue("@oznaka", this.oznaTxt.Text);
                command.Parameters.AddWithValue("@datum", datum);
                command.Parameters.AddWithValue("@icon", ikona);
                command.ExecuteNonQuery();
                string ozna = oznaTxt.Text;

                //FillDataGrid();

                string query1 = "select * from spomenikmapa where oznaka=@ozn";
                SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                command1.Parameters.AddWithValue("@ozn", ozna);
                command1.ExecuteNonQuery();
                SQLiteDataReader dr = command1.ExecuteReader();

                int oznaka = 0;
                while (dr.Read())
                {
                    //MessageBox.Show("usao");
                    oznaka++;
                }

                if (oznaka >= 1)
                {
                    string query2 = "update spomenikmapa set naziv=@nazive ,ikonica=@ikona where oznaka=@ozn";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.Parameters.AddWithValue("@nazive", this.nazivTxt.Text);
                    command2.Parameters.AddWithValue("@ikona", ikona);
                    command2.Parameters.AddWithValue("@ozn", ozna);
                    command2.ExecuteNonQuery();
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    mw.fillMap();
                }
                else
                {
                    string query2 = "update zaDodavanje set naziv=@nazive,ikonica=@ikona where oznaka=@ozn";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.Parameters.AddWithValue("@nazive", this.nazivTxt.Text);
                    command2.Parameters.AddWithValue("@ikona", ikona);
                    command2.Parameters.AddWithValue("@ozn", ozna);
                    command2.ExecuteNonQuery();

                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    //mw.popuniListu();
                    //string dod = ((Model.Lista)mw.list.Find(x => x.Onaka.Equals(ozn))).Naziv;
                    //MessageBox.Show("NASAO JE OVAJ SPOMENIK" + dod);
                    int ind = mw.list.FindIndex(x => x.Onaka.Equals(ozna));
                    mw.Listaspom.RemoveAt(ind);
                    mw.list.RemoveAt(ind);
                    mw.list.Add(new Model.Lista { Ikonica = ikona, Naziv = nazivTxt.Text, Onaka = ozna });
                    mw.Listaspom.Add(new Model.Lista { Ikonica = ikona, Naziv = nazivTxt.Text, Onaka = ozna });
                    //mw.Listaspom.Remove((Model.Lista)mw.list.Find(x => x.Onaka.Equals(ozn)));

                    //mw.list.Remove((Model.Lista)mw.list.Find(x => x.Onaka.Equals(ozn)));

                }




                //Menjanje Liste 
                //MainWindow mw = (MainWindow)Application.Current.MainWindow;

                //mw.Listaspom.Add(new Lista { Ikonica = null, Naziv = "proba", Onaka = "123" });
                //mw.popuniListu();

                //mw.ListaBox.Items.Refresh();

                FillDataGrid();
                MessageBox.Show("Uspesno izmenjeno!");
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);



            }
        }

        

        private void PromeniTagButton_Click(object sender, RoutedEventArgs e)
        {
            //var izTag = new Etiketa.IzborEtiketa();
            //izTag.ShowDialog();
            var izTag = new Etiketa.IzmenaEtiketa(this.oznaTxt.Text);
            izTag.ShowDialog();
            //if (izTag.DialogResult == true)
            //{
            //    tagTxt.Text = izTag.OznakaTag;
            //}
        }

        //private void DetTipButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var prikaz = new Tip.PrikazTip();
        //    prikaz.Show();
        //    prikaz.PrikazoznakaTxt.Text = Tiptxt.Text;
        //    string naziv = null;
        //    string opis = null;
        //    string icon = null;
        //
        //    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //    try
        //    {
        //        sqliteCon.Open();
        //        string query = "select * from tip where oznaka='" + Tiptxt.Text + "'";
        //        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //        command.ExecuteNonQuery();
        //
        //        SQLiteDataReader dr = command.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            naziv = dr.GetString(1);
        //            opis = dr.GetString(2);
        //            if (!dr.IsDBNull(3) && !dr.GetString(3).Equals(""))
        //            {
        //                icon = dr.GetString(3);
        //            }
        //
        //        }
        //
        //
        //        sqliteCon.Close();
        //
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //
        //    prikaz.PrikaznazivTxt.Text = naziv;
        //    prikaz.PrikazopisTxt.Text = opis;
        //    if (icon != null && !icon.Equals(""))
        //    {
        //        prikaz.Prikazikonica.Source = new BitmapImage(new Uri(icon));
        //    }
        //}

        //private void DetTagButton_Click(object sender, RoutedEventArgs e)
        //{
        //
        //    var prikaz = new Etiketa.PrikazEtiketa();
        //    prikaz.Show();
        //    prikaz.PrikazoznakaTxt.Text = tagTxt.Text;
        //    string opis = null;
        //    string boja = null;
        //
        //
        //    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //    try
        //    {
        //        sqliteCon.Open();
        //        string query = "select * from etiketa where oznaka='" + tagTxt.Text + "'";
        //        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //        command.ExecuteNonQuery();
        //
        //        SQLiteDataReader dr = command.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            opis = dr.GetString(1);
        //            boja = dr.GetString(2);
        //
        //        }
        //
        //        //SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //        //DataTable dt = new DataTable("spomenik");
        //        //dataAdp.Fill(dt);
        //        //grdSpomenik.ItemsSource = dt.DefaultView;
        //        //dataAdp.Update(dt);
        //        sqliteCon.Close();
        //
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //
        //
        //    prikaz.PrikazopisTxt.Text = opis;
        //    if (boja != null)
        //    {
        //        prikaz.PrikazBojaPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(boja);
        //    }
        //}



        private void FilterTip_DropDownClosed(object sender, EventArgs e)
        {
            if (FilterTip.SelectionBoxItem.ToString().Equals("Bez filtera"))
            {
                FillDataGrid();
            }
            else if (!FilterTip.Text.Equals("Bez filtera"))
            {

                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                try
                {
                    sqliteCon.Open();
                    string query = "select * from spomenik where oznakaTip='" + FilterTip.Text + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();

                    SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable("spomenik");
                    dataAdp.Fill(dt);
                    grdSpomenik.ItemsSource = dt.DefaultView;
                    dataAdp.Update(dt);
                    sqliteCon.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    




        //private void FilterTip_DropDownClosed(object sender, EventArgs e)
        //{
        //    if (FilterTip.SelectionBoxItem.ToString().Equals("Bez filtera") && FilterTag.Text.Equals("Bez filtera"))
        //    {
        //        FillDataGrid();
        //    }
        //    else if (!FilterTip.Text.Equals("Bez filtera") && !FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTip='" + FilterTip.Text + "' and oznakaTag='" + FilterTag.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else if (!FilterTip.Text.Equals("Bez filtera") && FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTip='" + FilterTip.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else if (FilterTip.Text.Equals("Bez filtera") && !FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTag='" + FilterTag.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        //private void FilterTag_DropDownClosed(object sender, EventArgs e)
        //{
        //    if (FilterTip.SelectionBoxItem.ToString().Equals("Bez filtera") && FilterTag.Text.Equals("Bez filtera"))
        //    {
        //        FillDataGrid();
        //    }
        //    else if (!FilterTip.Text.Equals("Bez filtera") && !FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTip='" + FilterTip.Text + "' and oznakaTag='" + FilterTag.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else if (FilterTip.Text.Equals("Bez filtera") && !FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTag='" + FilterTag.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else if (!FilterTip.Text.Equals("Bez filtera") && FilterTag.Text.Equals("Bez filtera"))
        //    {
        //
        //        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //        try
        //        {
        //            sqliteCon.Open();
        //            string query = "select * from spomenik where oznakaTip='" + FilterTip.Text + "'";
        //            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //            command.ExecuteNonQuery();
        //
        //            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
        //            DataTable dt = new DataTable("spomenik");
        //            dataAdp.Fill(dt);
        //            grdSpomenik.ItemsSource = dt.DefaultView;
        //            dataAdp.Update(dt);
        //            sqliteCon.Close();
        //
        //        }
        //
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        private void PretragaTxt_KeyUp(object sender, KeyEventArgs e)
        {
            FilterTip.SelectedIndex = 0;

            
            if (pretragaCb.SelectionBoxItem.ToString().Equals("Naziv"))
            {
                if (PretragaTxt.Text.Equals(""))
                {
                    FillDataGrid();
                }
                else
                {

                    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                    try
                    {
                        sqliteCon.Open();
                        string src = PretragaTxt.Text + "%";
                        var query = "select * from spomenik where naziv like @name";
                        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        command.Parameters.AddWithValue("@name", src);
                        command.ExecuteNonQuery();

                        SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                        DataTable dt = new DataTable("spomenik");
                        dataAdp.Fill(dt);
                        grdSpomenik.ItemsSource = dt.DefaultView;
                        dataAdp.Update(dt);
                        sqliteCon.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                if (PretragaTxt.Text.Equals(""))
                {
                    FillDataGrid();
                }
                else
                {

                    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                    try
                    {
                        sqliteCon.Open();
                        string src = PretragaTxt.Text + "%";
                        var query = "select * from spomenik where oznaka like @name";
                        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        command.Parameters.AddWithValue("@name", src);
                        command.ExecuteNonQuery();

                        SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(command);
                        DataTable dt = new DataTable("spomenik");
                        dataAdp.Fill(dt);
                        grdSpomenik.ItemsSource = dt.DefaultView;
                        dataAdp.Update(dt);
                        sqliteCon.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        public void PromeniIkonicuButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog rezultat = new Microsoft.Win32.OpenFileDialog();
            rezultat.Filter = "Image Files(*.BMP;*.JPG;*PNG)|*.BMP;*.JPG;*PNG";

            if (rezultat.ShowDialog() == true)
            {


                this.Ikonica.Source = new BitmapImage(new Uri(rezultat.FileName));


                ikonicaaa = rezultat.FileName;
                flag = true;
            }

        }

        private void PregledEtiketa_Click(object sender, RoutedEventArgs e)
        {
            var izTag = new Etiketa.IzmenaEtiketa(this.oznaTxt.Text);
            izTag.ShowDialog();
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

                EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("PregledSpomenika", mw);
            }
        }

        private void DodajSpomebtn_Click(object sender, RoutedEventArgs e)
        {
            bool isWindowsOpen = false;

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Spomenik.DodajSpomenik)
                {
                    isWindowsOpen = true;
                    w.Activate();
                    if (w.WindowState == WindowState.Minimized)
                        w.WindowState = WindowState.Normal;
                }
            }

            if (!isWindowsOpen)
            {
                var dodaj = new Spomenik.DodajSpomenik();
                dodaj.Show();

            }
            Close();
        }

        private void prkTipbtn_Click(object sender, RoutedEventArgs e)
        {
            var prikaz = new Tip.PrikazTip();
                prikaz.Show();
                prikaz.PrikazoznakaTxt.Text = TipDropDown.Text;
                string naziv = null;
                string opis = null;
                string icon = null;
            
                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                try
                {
                    sqliteCon.Open();
                    string query = "select * from tip where oznaka='" + TipDropDown.Text + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();
            
                    SQLiteDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        naziv = dr.GetString(1);
                        opis = dr.GetString(2);
                        if (!dr.IsDBNull(3) && !dr.GetString(3).Equals(""))
                        {
                            icon = dr.GetString(3);
                        }
            
                    }
            
            
                    sqliteCon.Close();
            
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
                prikaz.PrikaznazivTxt.Text = naziv;
                prikaz.PrikazopisTxt.Text = opis;
                if (icon != null && !icon.Equals(""))
                {
                    prikaz.Prikazikonica.Source = new BitmapImage(new Uri(icon));
                }
        }
    }
}
