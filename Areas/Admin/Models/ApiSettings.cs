namespace Dash.Areas.Admin.Models;

public class ApiSettings
{
    public string ApiKey { get; set; }
}

public class GeocodeResult
{
    public List<Item> Items { get; set; }

    public class Item
    {
        public Position Position { get; set; }
    }

    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}