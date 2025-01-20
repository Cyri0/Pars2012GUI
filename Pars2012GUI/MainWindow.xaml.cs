using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;

namespace Pars2012GUI
{
    public class Atleta
    {
        public string nev;
        public string csoport;
        public string nemzet;
        public string nemzetKod;
        public List<string> dobasok;
        public double max = 0;
        public Atleta(string sor)
        {
            string[] parts = sor.Split(';');
            this.nev = parts[0];
            this.csoport = parts[1];
            
            string[] partsNemzet = parts[2].Split('(');
            this.nemzet = partsNemzet[0];
            this.nemzetKod = partsNemzet[1].Split(')')[0];

            this.dobasok = new List<string>();
            this.dobasok.Add(parts[3]);
            this.dobasok.Add(parts[4]);
            this.dobasok.Add(parts[5]);


            foreach (string dobas in dobasok)
            {
                if (dobas != "X" && dobas != "-")
                {
                    double num = double.Parse(dobas);
                    if(num > this.max) max = num;
                }
            }
        }

        public string dobasokStr()
        {
            return string.Join(";", dobasok);
        }

        override
        public string ToString()
        {
            return this.nev;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Atleta> Beolvas()
        {
            StreamReader sr = new StreamReader("Selejtezo2012.txt", encoding: Encoding.UTF8);
            List<Atleta> atletak = new List<Atleta>();
            string sor;
            sr.ReadLine();
            while((sor = sr.ReadLine()) != null)
            {
                atletak.Add(new Atleta(sor));
            }

            sr.Close();
            return atletak;
        }

        public MainWindow()
        {
            InitializeComponent();

            List<Atleta> atletak = Beolvas();

            foreach (Atleta atleta in atletak)
            {
                versenyzokBox.Items.Add(atleta);
            }
            versenyzokBox.SelectedIndex = 0;
        }

        private void versenyzokBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Atleta atleta = versenyzokBox.SelectedItem as Atleta;
            csoportEl.Content = atleta.csoport;
            nemzetEl.Content = atleta.nemzet;
            nemzetKodEl.Content = atleta.nemzetKod;
            sorozatEl.Content = atleta.dobasokStr();
            eredmenyEl.Content = atleta.max;

            Uri uri = new Uri("Images/" + atleta.nemzetKod + ".png", UriKind.Relative);
            zaszloImg.Source = new BitmapImage(uri);
        }
    }
}