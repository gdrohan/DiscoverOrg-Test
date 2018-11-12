namespace Models
{
    public class ItemTypeState
    {
        public long Type { get; set; }
        public string Name { get; set; }
        public double FillFactor { get; set; }
        // include capacity for information (assuming this is human readable or will be written to a log)
        public int Capacity { get; set; }
    }
}
