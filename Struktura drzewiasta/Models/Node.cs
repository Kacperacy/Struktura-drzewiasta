namespace Struktura_drzewiasta.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public Node Parent { get; set; }
    }
}
