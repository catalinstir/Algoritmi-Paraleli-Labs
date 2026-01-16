namespace ex02
{
    public class PersonNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ExtraInfoJSON { get; set; }

        public List<PersonNode> Friends { get; set; } = new List<PersonNode>();
    }
}
