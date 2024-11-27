
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib {
    public class NaleznikController {
        private IList<Finding> findings;

        public IList<Finding> Findings { get => findings; set => findings = value; }

        public NaleznikController() {
            Findings = new ObservableCollection<Finding>();
            
        }

        public void CheckDirectoryOrCreate() {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Naleznik");
            Directory.CreateDirectory(folderPath);

            string dbPath = Path.Combine(folderPath, "Nalezy.db");

            using (var connection = new SqliteConnection("Data Source=" + dbPath)) {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Nalezy (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nazev TEXT NOT NULL,
                        Rok INTEGER,
                        Hloubka REAL,
                        DatumNalezu TEXT,
                        Typ TEXT,
                        Puda TEXT,
                        Popis TEXT,
                        Latitude REAL,
                        Longitude REAL,
                        PhotoPredniStrana BLOB,
                        PhotoZadniStrana BLOB,
                        PhotoNalez BLOB
                    )";

                using (var command = new SqliteCommand(createTableQuery, connection)) {
                    command.ExecuteNonQuery();
                }
            }


        }
        public void LoadFindings() {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Naleznik");
            string dbPath = Path.Combine(folderPath, "Nalezy.db");

            using (var connection = new SqliteConnection("Data Source=" + dbPath)) {
                connection.Open();

                string selectQuery = "SELECT * FROM Nalezy";
                using (var command = new SqliteCommand(selectQuery, connection)) {
                    using (var reader = command.ExecuteReader()) {
                        Findings.Clear(); // Vymazání seznamu před načtením nových dat

                        while (reader.Read()) {
                            // Načítání dat z databáze
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string name = reader.GetString(reader.GetOrdinal("Nazev"));
                            int year = reader.IsDBNull(reader.GetOrdinal("Rok")) ? 0 : reader.GetInt32(reader.GetOrdinal("Rok"));
                            string description = reader.GetString(reader.GetOrdinal("Popis"));
                            int findingDate = reader.IsDBNull(reader.GetOrdinal("Rok")) ? 0 : reader.GetInt32(reader.GetOrdinal("DatumNalezu"));
                            double depth = reader.IsDBNull(reader.GetOrdinal("Hloubka"))
                                ? 0.0
                                : reader.GetDouble(reader.GetOrdinal("Hloubka"));

                            double latitude = reader.IsDBNull(reader.GetOrdinal("Latitude"))
                                ? 0.0
                                : reader.GetDouble(reader.GetOrdinal("Latitude"));
                            double longitude = reader.IsDBNull(reader.GetOrdinal("Longitude"))
                                ? 0.0
                                : reader.GetDouble(reader.GetOrdinal("Longitude"));
                            location loc = new location(latitude, longitude);

                            bool coin = reader.IsDBNull(reader.GetOrdinal("Typ"))
                                ? false
                                : reader.GetString(reader.GetOrdinal("Typ")).Equals("coin", StringComparison.OrdinalIgnoreCase);

                            bool drySoil = reader.IsDBNull(reader.GetOrdinal("Puda"))
                                ? false
                                : reader.GetString(reader.GetOrdinal("Puda")).Equals("dry", StringComparison.OrdinalIgnoreCase);

                            byte[] photoPredniStrana = reader.IsDBNull(reader.GetOrdinal("PhotoPredniStrana"))
                                ? null
                                : (byte[])reader["PhotoPredniStrana"];
                            byte[] photoZadniStrana = reader.IsDBNull(reader.GetOrdinal("PhotoZadniStrana"))
                                ? null
                                : (byte[])reader["PhotoZadniStrana"];
                            byte[] photoNalez = reader.IsDBNull(reader.GetOrdinal("PhotoNalez"))
                                ? null
                                : (byte[])reader["PhotoNalez"];

                            // Vytvoření instance Finding
                            var finding = new Finding(
                                id,
                                name,
                                year,
                                description,
                                findingDate,
                                depth,
                                loc
                            ) {
                                PhotoPredniStrana = photoPredniStrana,
                                PhotoZadniStrana = photoZadniStrana,
                                PhotoNalez = photoNalez
                            };

                            Findings.Add(finding);
                        }
                    }
                }
            }
        }

        public IList<Finding> GetFindings() {
            return Findings;
        }

        public void AddFinding(Finding finding) {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Naleznik");
            string dbPath = Path.Combine(folderPath, "Nalezy.db");

            using (var connection = new SqliteConnection("Data Source=" + dbPath)) {
                connection.Open();

                // Vytvoříme SQL dotaz pro vložení nového nálezu
                string insertQuery = @"
            INSERT INTO Nalezy (Nazev, Rok, Hloubka, DatumNalezu, Typ, Puda, Popis, Latitude, Longitude)
            VALUES (@Nazev, @Rok, @Hloubka, @DatumNalezu, @Typ, @Puda, @Popis, @Latitude, @Longitude)";

                using (var command = new SqliteCommand(insertQuery, connection)) {
                    // Parametry pro SQL dotaz
                    command.Parameters.AddWithValue("@Nazev", finding.Name);
                    command.Parameters.AddWithValue("@Rok", finding.Year); // Předáme rok
                    command.Parameters.AddWithValue("@Hloubka", finding.Depth);
                    command.Parameters.AddWithValue("@DatumNalezu", finding.FindingDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Typ", finding.Coin ? "coin" : "other");
                    command.Parameters.AddWithValue("@Puda", finding.DrySoil ? "dry" : "other");
                    command.Parameters.AddWithValue("@Popis", finding.Description);
                    command.Parameters.AddWithValue("@Latitude", finding.Location.Latitude);
                    command.Parameters.AddWithValue("@Longitude", finding.Location.Longitude);

                    // Spustíme SQL dotaz
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
