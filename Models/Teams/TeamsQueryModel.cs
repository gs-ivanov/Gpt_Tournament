namespace Tournament.Models.Teams
{
    public class TeamsQueryModel
    {
        public int Id { get; set; }

        public string Name { get; init; }

        public string City { get; init; }

        public string Trener { get; init; }

        public int Wins { get; init; }

        public int Losts { get; init; }

        public bool IsEditable { get; set; }

    }
}
