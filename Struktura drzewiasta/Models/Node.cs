namespace Struktura_drzewiasta.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();
    }
}