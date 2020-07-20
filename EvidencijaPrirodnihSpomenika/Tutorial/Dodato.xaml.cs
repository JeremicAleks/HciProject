using System;
using System.Collections.Generic;
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

namespace EvidencijaPrirodnihSpomenika.Tutorial
{
    /// <summary>
    /// Interaction logic for Dodato.xaml
    /// </summary>
    public partial class Dodato : Window
    {
        public Dodato()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Tutorial.Tutor)
                {

                    Tutorial.Tutor tutor = w as Tutorial.Tutor;

                    if (tutor.korak == 7)
                    {
                       
                        tutor.Close();
                    }
                }
            }
        }
    }
}
