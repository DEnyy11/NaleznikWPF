namespace Lib {
    public class location {
        double latitude;
        double longitude;

        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public location(double latitude, double longtitude) {
            Latitude = latitude;
            Longitude = longtitude;
        }
    }
    
}
