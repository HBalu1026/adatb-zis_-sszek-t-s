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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.IO;

namespace WpfAppMySQL_gyak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string kapcsolatLeiro = "datasource=127.0.0.1;port=3306;username=root;password=;database=hardver;charset=utf8";
        List<Termek> termekek = new List<Termek>();
        MySqlConnection SQLkapcsolat;
        
        public MainWindow()
        {
            InitializeComponent();

            AdatbazisMegnyitas();
            KategoriaBetoltese();
            GyartokBetoltese();
            
            TermekBetolteseListaba();
            
            //AdatbazisLezarasa();
        }

        private void btnMentes_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.DefaultExt = "csv";
            sfd.Filter = "csv files (*.csv) | *.csv";

            StreamWriter sw = new StreamWriter("tablazat.csv");
            foreach (var item in termekek)
            {
                sw.WriteLine(item.ToCSVString());
            }
            sw.Close();

        }

        private void btnSzukit_Click(object sender, RoutedEventArgs e)
        {
            termekek.Clear();
            string SQLSzukitettLista = SzukitettListaEloallitasa();

            MySqlCommand SQLparancs = new MySqlCommand(SQLSzukitettLista, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();

            while (eredmenyOlvaso.Read())
            {
                Termek uj = new Termek(eredmenyOlvaso.GetString("Kategória"),
                    eredmenyOlvaso.GetString("Gyártó"),
                    eredmenyOlvaso.GetString("Név"),
                    Convert.ToInt32(eredmenyOlvaso.GetString("Ár")),
                    Convert.ToInt32(eredmenyOlvaso.GetString("Garidő")));
                termekek.Add(uj);
            }
            eredmenyOlvaso.Close();
            dgTermekek.Items.Refresh();
        }

        private void TermekBetolteseListaba()
        {
            string SQLOsszesTermek = "SELECT * FROM termékek;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLOsszesTermek, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();

            while (eredmenyOlvaso.Read())
            {
                Termek uj = new Termek(eredmenyOlvaso.GetString("Kategória"),
                    eredmenyOlvaso.GetString("Gyártó"),
                    eredmenyOlvaso.GetString("Név"),
                    Convert.ToInt32(eredmenyOlvaso.GetString("Ár")),
                    Convert.ToInt32(eredmenyOlvaso.GetString("Garidő")));

                termekek.Add(uj);
            }
            eredmenyOlvaso.Close();

            dgTermekek.ItemsSource = termekek;
        }

        private void KategoriaBetoltese()
        {
            string SQLKategoriaRendezve = "SELECT DISTINCT kategória FROM termékek ORDER BY kategória;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLKategoriaRendezve, SQLkapcsolat);
            MySqlDataReader eredmenyolvaso = SQLparancs.ExecuteReader();

            cbKategoria.Items.Add(" - Nincs megadva - ");
            while (eredmenyolvaso.Read())
            {
                cbKategoria.Items.Add(eredmenyolvaso.GetString("Kategória"));
            }
            eredmenyolvaso.Close();
            cbKategoria.SelectedIndex = 0;

        }

        private void AdatbazisMegnyitas()
        {
            try
            {
                SQLkapcsolat = new MySqlConnection(kapcsolatLeiro);
                SQLkapcsolat.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("Nem tud kapcsolódni az adatbázishoz!");
                this.Close();
            }
        }

        private void GyartokBetoltese()
        {
            string SQLGyartokRendezve = "SELECT DISTINCT gyártó FROM termékek ORDER BY gyártó;";

            MySqlCommand SQLparancs = new MySqlCommand(SQLGyartokRendezve, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLparancs.ExecuteReader();

            cbGyarto.Items.Add(" - Nincs megadva - ");
            while (eredmenyOlvaso.Read())
            {
                cbGyarto.Items.Add(eredmenyOlvaso.GetString("Gyártó"));
            }
            eredmenyOlvaso.Close();
            cbGyarto.SelectedIndex = 0;
        }

        private void AdatbazisLezarasa()
        {
            SQLkapcsolat.Close();
            SQLkapcsolat.Dispose();
        }

        private string SzukitettListaEloallitasa()
        {
            bool vanFeltetel = false;
            string SQlszukites = "SELECT * FROM termékek ";

            if (cbGyarto.SelectedIndex != 0 || cbKategoria.SelectedIndex != 0 || txtTermek.Text != "")
            {
                SQlszukites += "WHERE ";
            }

            if (cbGyarto.SelectedIndex > 0)
            {
                SQlszukites += $"gyártó='{cbGyarto.SelectedIndex}'";
                vanFeltetel = true;
            }

            if (cbKategoria.SelectedIndex > 0)
            {
                if (vanFeltetel)
                {
                    SQlszukites += " AND ";
                }
                SQlszukites += $"név LIKE '%{txtTermek.Text}%'";
            }
            return SQlszukites;
        }
    }
}
