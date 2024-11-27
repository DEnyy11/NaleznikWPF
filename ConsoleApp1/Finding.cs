using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class Finding {
        int id;
        string name;
        int year;
        string description;
        int findingDate;
        double depth;
        location location;
        bool coin;
        bool drySoil;
        byte[] photoPredniStrana;
        byte[] photoZadniStrana;
        byte[] photoNalez;
        List<byte[]> photosOther;

        #region "Properties"
        public string Name { get => name; set => name = value; }
        public int Year { get => year; set => year = value; }
        public string Description { get => description; set => description = value; }
        public int FindingDate { get => findingDate; set => findingDate = value; }
        public double Depth { get => depth; set => depth = value; }
        public int Id { get => id; set => id = value; }
        public byte[] PhotoPredniStrana { get => photoPredniStrana; set => photoPredniStrana = value; }
        public byte[] PhotoZadniStrana { get => photoZadniStrana; set => photoZadniStrana = value; }
        public byte[] PhotoNalez { get => photoNalez; set => photoNalez = value; }
        public bool Coin { get => coin; set => coin = value; }
        public bool DrySoil { get => drySoil; set => drySoil = value; }
        public location Location { get => location; set => location = value; }
        public List<byte[]> PhotosOther { get => photosOther; set => photosOther = value; }
        #endregion

        public Finding(int id, string name, int year, string description,int findingDate, double depth, location location) {
            Id = id;
            Name = name;
            Year = year;
            Description = description;
            FindingDate = findingDate;
            Depth = depth;
            this.Location = location;
        }
    }    
}
