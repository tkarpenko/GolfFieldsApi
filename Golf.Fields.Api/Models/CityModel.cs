namespace Golf.Fields.Api
{
    public class CityModel
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public CountryModel? Country { get; set; }
    }
}

