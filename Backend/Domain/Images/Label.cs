namespace Backend.Domain.Images
{
    public class Label
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public Label(string value)
        {
            Value = value;
        }
    }
}