namespace Models
{
    public class ItemType
    {
        public long Type { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsStocked { get; set; }
        // ... dimensions, weight, etc
    }
}
