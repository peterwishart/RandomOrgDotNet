namespace RandomOrgDotNet
{
    class PregeneratedRandomization
    {
        private class PregeneratedRandomizationId : PregeneratedRandomization
        {
            public readonly string id;
            public PregeneratedRandomizationId(string id) => this.id = id;
        }
        private class PregeneratedRandomizationDate : PregeneratedRandomization
        {
            public readonly string date;
            public PregeneratedRandomizationDate(DateTime date) => this.date = date.ToUniversalTime().ToString("yyyy-MM-dd");
        }
        public static PregeneratedRandomization ForDate(DateTime value) => new PregeneratedRandomizationDate(value);
        public static PregeneratedRandomization ForIdentifier(string value) => new PregeneratedRandomizationId(value);
    }
}
