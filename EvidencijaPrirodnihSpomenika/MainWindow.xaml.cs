using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EvidencijaPrirodnihSpomenika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Model.Lista> list = new List<Model.Lista>();
        string dbConn = "Data Source=database.db;Version=3;";
        
        Point startPoint = new Point();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            popuniListu();
            fillMap();
            

        }

        private void Add_Click(object sender, RoutedEventArgs e)
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

           

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    Tutorial.Tutor tutor = w as Tutorial.Tutor;
                    if (tutor.korak == 1)
                    {
                        w.Activate();
                        //Tutorial.Tutor tutor = w as Tutorial.Tutor;
                        tutor.UnosOznake();
                        
                    }
                    
                }
            }


        }


        private void pSpoemika_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    
                        w.Activate();
                        upaljen = true;  

                }
            }
            if (!upaljen)
            {
                var spoemnik = new Spomenik.PregledSpomenik();
                spoemnik.Show();
            }
            else
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }
        }

        private void pTipova_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!upaljen)
            {
                var tip = new Tip.PregledTip();
                tip.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }
            
        }

        private void pEtiketa_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!upaljen)
            {
               
                var tag = new Etiketa.PregledEtiketa();
            tag.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }

            
        }

        private void DTipBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
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
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!isWindowOpen && !upaljen)
            {
                
                var tip = new Tip.DodajTip();
                tip.Show();
            }
            if(upaljen)
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }
            

        }

        private void DEtikBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
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
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!isWindowOpen && !upaljen)
            {

                var etik = new Etiketa.DodajEtiketa();
                etik.Show();
            }
            if (upaljen)
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }


        }
        public ObservableCollection<Model.Lista> Listaspom
        {
            get;
            set;
        }

        public void popuniListu()
        {
            list = new List<Model.Lista>();
            Listaspom = new ObservableCollection<Model.Lista>();

            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from zaDodavanje";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznaka = dr.GetString(0);
                    String naziv = dr.GetString(1);
                    String ikonica = dr.GetString(2);

                    list.Add(new Model.Lista { Ikonica = ikonica, Naziv = naziv, Onaka = oznaka });

                }


                sqliteCon.Close();
                Listaspom = new ObservableCollection<Model.Lista>(list);
                ListaBox.ItemsSource = Listaspom;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        public void ubaci()

        {
            List<Model.Lista> items = new List<Model.Lista>();


        }



        private void ListaBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            

        }

        private void ListaBox_MouseMove(object sender, MouseEventArgs e)
        {

            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
               (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
               Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

                // Get the dragged ListBoxItem
                ListBox lb = sender as ListBox;
                ListBoxItem lbI = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (lbI == null)
                {
                    return;
                }

                // Find the data behind the ListViewItem
                Model.Lista ls = (Model.Lista)lb.ItemContainerGenerator.ItemFromContainer(lbI);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", ls);
                DragDrop.DoDragDrop(lbI, dragData, DragDropEffects.Move);

                

            }
        }

        private void Kanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Kanvas_Drop(object sender, DragEventArgs e)
        {
            Point PustioSliku = e.GetPosition(Kanvas);

            if (e.Data.GetDataPresent("myFormat"))
            {
                //MessageBox.Show("Dodato");
                Model.Lista ls = e.Data.GetData("myFormat") as Model.Lista;
                
                Image img = new Image() { Width = 30, Height = 30 };
                img.Source = new BitmapImage(new Uri(ls.Ikonica));
                
                img.ContextMenu = this.FindResource("Slicica") as ContextMenu;
                
                img.Name = "Oznaka_" + ls.Onaka;
                img.ToolTip = ls.Naziv;
               
                
                img.MouseDown += Slika_MouseDown;
                img.PreviewMouseLeftButtonDown += ListaBox_PreviewMouseLeftButtonDown;
                img.MouseMove += Slika_MouseMove;
                //img.PreviewMouseRightButtonUp += Desni_Klik_slika;



                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                Boolean moze = true;
                try
                {
                    sqliteCon.Open();
                    string prov = "select * from spomenikmapa";
                    SQLiteCommand coma = new SQLiteCommand(prov, sqliteCon);
                    coma.ExecuteNonQuery();
                    SQLiteDataReader dre = coma.ExecuteReader();

                    while (dre.Read())
                    {
                        
                        double xkor = (double)dre.GetValue(3);
                        double ykor = (double)dre.GetValue(4);
                        

                        if (e.GetPosition(Kanvas).X >= xkor - 10 && e.GetPosition(Kanvas).X <= xkor + 30 && e.GetPosition(Kanvas).Y >= ykor - 10 && e.GetPosition(Kanvas).Y <= ykor + 30)
                        {
                            moze = false;
                        }


                    }
                    sqliteCon.Close();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                //Boolean moze = provera(e.GetPosition(Kanvas).X, e.GetPosition(Kanvas).Y, ls.Onaka);

                if (moze)
                {


                    if (PustioSliku.Y >= 385)
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y - 40);
                    }
                    else if (PustioSliku.X >= 590)
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X - 30);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y);
                    }
                    else
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y);
                    }
                    this.Kanvas.Children.Add(img);

                    Listaspom.Remove((Model.Lista)ListaBox.SelectedItem);

                    //Ubacivanje u BAZU

                    //SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);

                    try
                    {
                        if (PustioSliku.Y >= 385)
                        {
                            sqliteCon.Open();
                            string query = "insert into spomenikmapa (oznaka,naziv,ikonica,X,Y) values('" + ls.Onaka + "','" + ls.Naziv + "','" + ls.Ikonica + "','" + e.GetPosition(this.Kanvas).X + "','" + (e.GetPosition(this.Kanvas).Y - 40) + "') ";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();

                            string query2 = "delete from zaDodavanje where oznaka='" + ls.Onaka + "'";
                            SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                            command2.ExecuteNonQuery();
                        }
                        else if (PustioSliku.X >= 590)
                        {
                            sqliteCon.Open();
                            string query = "insert into spomenikmapa (oznaka,naziv,ikonica,X,Y) values('" + ls.Onaka + "','" + ls.Naziv + "','" + ls.Ikonica + "','" + (e.GetPosition(this.Kanvas).X - 30) + "','" + e.GetPosition(this.Kanvas).Y + "') ";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();

                            string query2 = "delete from zaDodavanje where oznaka='" + ls.Onaka + "'";
                            SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                            command2.ExecuteNonQuery();
                        }
                        else
                        {
                            sqliteCon.Open();
                            string query = "insert into spomenikmapa (oznaka,naziv,ikonica,X,Y) values('" + ls.Onaka + "','" + ls.Naziv + "','" + ls.Ikonica + "','" + e.GetPosition(this.Kanvas).X + "','" + e.GetPosition(this.Kanvas).Y + "') ";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();

                            string query2 = "delete from zaDodavanje where oznaka='" + ls.Onaka + "'";
                            SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                            command2.ExecuteNonQuery();
                        }


                        //sqliteCon.Open();
                        //string query = "insert into spomenikmapa (oznaka,naziv,ikonica,X,Y) values('" + ls.Onaka+ "','" + ls.Naziv + "','" + ls.Ikonica + "','" + e.GetPosition(this.Kanvas).X + "','" + e.GetPosition(this.Kanvas).Y + "') ";
                        //SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        //command.ExecuteNonQuery();
                        //
                        //string query2 = "delete from zaDodavanje where oznaka='" + ls.Onaka + "'";
                        //SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                        //command2.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);

                    }
                }
                else
                {
                    MessageBox.Show("Ikonice se ne smeju preklapati");
                }
            
            }

            if (e.Data.GetDataPresent("Slika"))
            {
                //Lista ls = e.Data.GetData("myFormat") as Lista;
                Image slika = e.Data.GetData("Slika") as Image;

                Image img = new Image() { Width = 30, Height = 30 };
                img.Source = slika.Source;
                
                img.Name = slika.Name;
                //img.ToolTip = slika.Name;

                img.MouseDown += Slika_MouseDown;
                img.PreviewMouseLeftButtonDown += ListaBox_PreviewMouseLeftButtonDown;
                img.MouseMove += Slika_MouseMove;
                //img.PreviewMouseRightButtonUp += Desni_Klik_slika;
                img.ContextMenu = FindResource("Slicica") as ContextMenu;

                string ozna = img.Name.Substring(7);

                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                Boolean moze = true;
                try
                {
                    sqliteCon.Open();
                    string prov = "select * from spomenikmapa";
                    SQLiteCommand coma = new SQLiteCommand(prov, sqliteCon);
                    coma.ExecuteNonQuery();
                    SQLiteDataReader dre = coma.ExecuteReader();

                    while (dre.Read())
                    {
                        string oz = dre.GetString(0);
                        double xkor = (double)dre.GetValue(3);
                        double ykor = (double)dre.GetValue(4);

                        if (oz.Equals(ozna))
                        {
                            continue;
                        }

                        if (e.GetPosition(Kanvas).X >= xkor - 10 && e.GetPosition(Kanvas).X <= xkor + 30 && e.GetPosition(Kanvas).Y >= ykor - 10 && e.GetPosition(Kanvas).Y <= ykor + 30)
                        {
                            moze = false;
                        }


                    }
                    sqliteCon.Close();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                if (moze)
                {

                    if (PustioSliku.Y >= 380)
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y - 40);
                    }
                    else if (PustioSliku.X >= 590)
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X - 30);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y);
                    }
                    else
                    {
                        Canvas.SetLeft(img, e.GetPosition(this.Kanvas).X);
                        Canvas.SetTop(img, e.GetPosition(this.Kanvas).Y);
                    }


                    this.Kanvas.Children.Add(img);
                    this.Kanvas.Children.Remove(slika);

                    //Izmena koordinaata u bazi


                    //SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                    try
                    {

                        if (PustioSliku.Y >= 380)
                        {
                            sqliteCon.Open();
                            string query = "update spomenikmapa set X='" + e.GetPosition(this.Kanvas).X + "',Y='" + (e.GetPosition(this.Kanvas).Y - 40) + "'where oznaka='" + img.Name.Substring(7) + "'";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();
                        }
                        else if (PustioSliku.X >= 590)
                        {
                            sqliteCon.Open();
                            string query = "update spomenikmapa set X='" + (e.GetPosition(this.Kanvas).X - 30) + "',Y='" + e.GetPosition(this.Kanvas).Y + "'where oznaka='" + img.Name.Substring(7) + "'";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            sqliteCon.Open();
                            string query = "update spomenikmapa set X='" + e.GetPosition(this.Kanvas).X + "',Y='" + e.GetPosition(this.Kanvas).Y + "'where oznaka='" + img.Name.Substring(7) + "'";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();
                        }

                        //sqliteCon.Open();
                        //string query = "update spomenikmapa set X='" + e.GetPosition(this.Kanvas).X + "',Y='" + e.GetPosition(this.Kanvas).Y + "'where oznaka='" + img.Name.Substring(7) + "'";
                        //SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        //command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {


                        MessageBox.Show(ex.Message);



                    }
                }
                else
                {
                    MessageBox.Show("Ikonice se ne smeju preklapati");
                }


            }
        }

        //public Boolean provera(double x , double y,string oznaka)
        //{
        //    
        //    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
        //    Boolean moze = true;
        //    try
        //    {
        //        
        //        sqliteCon.Open();
        //        string query1 = "select * from spomenikmapa";
        //        SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
        //        command1.ExecuteNonQuery();
        //        SQLiteDataReader dr = command1.ExecuteReader();
        //
        //
        //        
        //
        //        string query = "select * from spomenikmapa where oznaka='" + oznaka + "'";
        //        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
        //        //command1.Parameters.AddWithValue("@ozn1", ls.Onaka);
        //        command.ExecuteNonQuery();
        //        SQLiteDataReader dre = command.ExecuteReader();
        //        
        //        while (dre.Read())
        //        {
        //            
        //            double xkor = (double)dre.GetValue(3);
        //            double ykor = (double)dre.GetValue(4);
        //            MessageBox.Show(xkor.ToString() + " " + ykor.ToString());
        //        
        //            if (x >= xkor - 10 && x <= xkor + 40 && y >= ykor - 10 && y <= ykor + 40)
        //            {
        //                moze = false;
        //            }
        //        
        //        
        //        }
        //        sqliteCon.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //
        //
        //
        //
        //    return moze;
        //
        //}
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void Slika_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Image img = (Image)sender;
                string ozn = img.Name.Substring(7);
                var Prik = new Spomenik.PrikazSpomMapa(ozn);
                Prik.ShowDialog();
            }

        }
        private void Slika_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(Kanvas);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
               (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
               Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

                // Get the dragged ListBoxItem
                Image img = sender as Image;

                String oz = img.Name.Substring(7);

                Image lbI = FindAncestor<Image>((DependencyObject)e.OriginalSource);
                // Find the data behind the ListViewItem
                Model.Lista ls = (Model.Lista)list.Find(x => x.Onaka.Equals(oz));

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("Slika", img);
                DragDrop.DoDragDrop(lbI, dragData, DragDropEffects.Copy);


            }
        }

     
        

        public void fillMap()
        {
            this.Kanvas.Children.RemoveRange(1, Kanvas.Children.Count - 1);
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from spomenikmapa";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznaka = dr.GetString(0);
                    String naziv = dr.GetString(1);
                    String ikonica = dr.GetString(2);
                    double xkor =(double) dr.GetValue(3);
                    double ykor = (double)dr.GetValue(4);

                   
                    Image img = new Image() { Width = 30, Height = 30 };
                    img.Source = new BitmapImage(new Uri(ikonica));

                    img.Name = "Oznaka_"+oznaka;
                    img.ToolTip = naziv;
                    img.MouseDown += Slika_MouseDown;
                    img.PreviewMouseLeftButtonDown += ListaBox_PreviewMouseLeftButtonDown;
                    img.MouseMove += Slika_MouseMove;
                    //img.PreviewMouseRightButtonUp += Desni_Klik_slika;
                    img.ContextMenu = FindResource("Slicica") as ContextMenu;
                    Canvas.SetLeft(img, xkor);
                    Canvas.SetTop(img, ykor);

                    this.Kanvas.Children.Add(img);
                    
                }


                sqliteCon.Close();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void proba_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!upaljen)
            {
            this.Kanvas.Children.RemoveRange(1, Kanvas.Children.Count - 1);
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
            try
            {
                sqliteCon.Open();
                string query = "select * from spomenikmapa";
                SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                command.ExecuteNonQuery();

                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    String oznaka = dr.GetString(0);
                    String naziv = dr.GetString(1);
                    String ikonica = dr.GetString(2);

                    string query1 = "insert into zadodavanje (oznaka,naziv,ikonica) values('" + oznaka + "','" + naziv + "','" + ikonica + "') ";
                    SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                    command1.ExecuteNonQuery();
                    list.Add(new Model.Lista { Ikonica = ikonica, Naziv = naziv, Onaka = oznaka });
                    Listaspom.Add(new Model.Lista { Ikonica = ikonica, Naziv = naziv, Onaka = oznaka });

                    string query2 = "delete from spomenikMapa where oznaka='"+oznaka+"'";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.ExecuteNonQuery();

                }


                sqliteCon.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            }
            else
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }
        }

        private void DSpomenikMenu_Click(object sender, RoutedEventArgs e)
        {

            var dodaj = new Spomenik.DodajSpomenik();
            dodaj.Show();
        }

        private void ObirisSpomeniTb_Click(object sender, RoutedEventArgs e)
        {

            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!upaljen)
            {

            Model.Lista ml =ListaBox.SelectedItem as Model.Lista;
            
            if (ml == null)
            {
                MessageBox.Show("Morate selektovati spomenik");
                return;
            }
           
            String oznaka = ml.Onaka;
            
            if (MessageBox.Show("Da li ste sigurni da zelite da obrisete spomenik?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                try
                {


                    sqliteCon.Open();
                    string query = "delete from spomenik where oznaka='" + oznaka + "'";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                    command.ExecuteNonQuery();

                    string query2 = "delete from zaDodavanje where oznaka='" + oznaka + "'";
                    SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                    command2.ExecuteNonQuery();

                        int idx = list.FindIndex(x => x.Onaka.Equals(oznaka));



                        list.RemoveAt(idx);
                        Listaspom.RemoveAt(idx);

                        //list.Remove(ml);
                        //Listaspom.Remove(ml);

                    }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }

            }
            }
            else
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }

        }

        private void PregledSpomTb_Click(object sender, RoutedEventArgs e)
        {
            Boolean upaljen = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!upaljen)
            {
                var spoemnik = new Spomenik.PregledSpomenik();
                spoemnik.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate zavrsiti tutorijal za dodavanje spomenika");
            }
        }


        ///TUTORIJAL

        private void TutorBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isWindowsOpen = false;

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    isWindowsOpen = true;
                    w.Activate();
                }
            }

            if (!isWindowsOpen)
            {
                var tut = new Tutorial.Tutor();
                tut.pocetak();
                tut.Show();
                
            }
            
        }

        //private void Desni_Klik_slika (object sender, RoutedEventArgs e)
        //{
        //    ContextMenu cm = this.FindResource("Slicica") as ContextMenu;
        //    cm.PlacementTarget = sender as Image;
        //    cm.IsOpen = true;
        //}

        private void ObrisiMn_Click(object sender, RoutedEventArgs e)
        {

            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                Image img = ((ContextMenu)mi.Parent).PlacementTarget as Image;

                if (img != null)
                {
                    string oznaka = img.Name.Substring(7); 
                    if (MessageBox.Show("Da li ste sigurni da zelite da obrisete spomenik?", "Obrisi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                        try
                        {


                            sqliteCon.Open();
                            string query = "delete from spomenik where oznaka='" + oznaka + "'";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                            command.ExecuteNonQuery();

                            string query2 = "delete from spomenikmapa where oznaka='" + oznaka + "'";
                            SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                            command2.ExecuteNonQuery();

                            fillMap();

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);

                        }
                    }

                }
            }
        }

        private void VratiMn_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                Image img = ((ContextMenu)mi.Parent).PlacementTarget as Image;

                if (img != null)
                {
                    string oznakaSpom = img.Name.Substring(7);
                    SQLiteConnection sqliteCon = new SQLiteConnection(dbConn);
                    try
                    {
                        sqliteCon.Open();
                        string query = "select * from spomenikmapa where oznaka='"+oznakaSpom+"'";
                        SQLiteCommand command = new SQLiteCommand(query, sqliteCon);
                        command.ExecuteNonQuery();

                        SQLiteDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            String oznaka = dr.GetString(0);
                            String naziv = dr.GetString(1);
                            String ikonica = dr.GetString(2);

                            string query1 = "insert into zadodavanje (oznaka,naziv,ikonica) values('" + oznaka + "','" + naziv + "','" + ikonica + "') ";
                            SQLiteCommand command1 = new SQLiteCommand(query1, sqliteCon);
                            command1.ExecuteNonQuery();
                            list.Add(new Model.Lista { Ikonica = ikonica, Naziv = naziv, Onaka = oznaka });
                            Listaspom.Add(new Model.Lista { Ikonica = ikonica, Naziv = naziv, Onaka = oznaka });

                            string query2 = "delete from spomenikMapa where oznaka='" + oznaka + "'";
                            SQLiteCommand command2 = new SQLiteCommand(query2, sqliteCon);
                            command2.ExecuteNonQuery();

                            fillMap();

                        }


                        sqliteCon.Close();

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);

                    }

                }
            }
        }

        private void PrikazeMn_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                Image img = ((ContextMenu)mi.Parent).PlacementTarget as Image;

                if (img != null)
                {
                    string oznaka = img.Name.Substring(7);
                    var Prik = new Spomenik.PrikazSpomMapa(oznaka);
                    Prik.ShowDialog();
                }
            }
        

        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("GlavniProzor", this);
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            EvidencijaPrirodnihSpomenika.Help.HelpProvider.ShowHelp("GlavniProzor", this);
        }

        private void AddSpom_Executed(object sender, ExecutedRoutedEventArgs e)
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



            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {
                    Tutorial.Tutor tutor = w as Tutorial.Tutor;
                    if (tutor.korak == 1)
                    {
                        w.Activate();
                        //Tutorial.Tutor tutor = w as Tutorial.Tutor;
                        tutor.UnosOznake();

                    }

                }
            }


        }

        private void AddSpom_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Boolean upaljen = false;
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
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    w.Activate();
                    upaljen = true;

                }
            }
            if (!isWindowOpen && !upaljen)
            {

                var tip = new Tip.DodajTip();
                tip.Show();
            }
            if (upaljen)
            {
                MessageBox.Show("Morate zavrsiti ili ugasiti tutorijal za dodavanje spomenika");
            }
        }

        private void AddType_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
