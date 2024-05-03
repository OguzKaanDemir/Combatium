namespace Scripts.Interfaces
{
    public interface ICollectable
    {
        public bool IsCollectable { get; set; }

        public void Collect();
    }
}
