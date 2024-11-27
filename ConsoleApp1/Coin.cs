using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class Coin {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public int FirstYearOfMinting { get; set; }
        public int LastYearOfMinting { get; set; }
        public List<string> Mints { get; set; }
        public List<string> Monarchs { get; set; }
        public List<string> Denominations { get; set; }

        private string connectionString = "Data Source=mince.db;Version=3;";


        public Coin(int coinId) {
            Mints = new List<string>();
            Monarchs = new List<string>();
            Denominations = new List<string>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                // Načítání základních informací o minci
                string query = "SELECT nazev, material, prvni_rok_razby, posledni_rok_razby FROM Mince WHERE id = @coinId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@coinId", coinId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            Name = reader["nazev"].ToString();
                            Material = reader["material"].ToString();
                            FirstYearOfMinting = Convert.ToInt32(reader["prvni_rok_razby"]);
                            LastYearOfMinting = Convert.ToInt32(reader["posledni_rok_razby"]);
                        }
                    }
                }

                // Načítání mincovny pro tuto minci
                query = @"
                SELECT m.nazev
                FROM Mincovna m
                JOIN Mince_Mincovna mm ON m.id = mm.mincovna_id
                WHERE mm.mince_id = @coinId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@coinId", coinId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Mints.Add(reader["nazev"].ToString());
                        }
                    }
                }

                // Načítání panovníků pro tuto minci
                query = @"
                SELECT p.jmeno
                FROM Panovnik p
                JOIN Mince_Panovnik mp ON p.id = mp.panovnik_id
                WHERE mp.mince_id = @coinId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@coinId", coinId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Monarchs.Add(reader["jmeno"].ToString());
                        }
                    }
                }

                // Načítání nominálních hodnot pro tuto minci
                query = @"
                SELECT nh.hodnota
                FROM Nominální_hodnoty nh
                WHERE nh.mince_id = @coinId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@coinId", coinId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Denominations.Add(reader["hodnota"].ToString());
                        }
                    }
                }
            }
        }

        // Statická metoda pro načtení všech mincí z databáze
        public static List<Coin> LoadAllCoins() {
            List<Coin> coins = new List<Coin>();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mince.db;Version=3;")) {
                conn.Open();
                string query = "SELECT id FROM Mince";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            int coinId = Convert.ToInt32(reader["id"]);
                            coins.Add(new Coin(coinId));  // Pro každou minci zavoláme konstruktor
                        }
                    }
                }
            }

            return coins;
        }
    }
}