namespace Backend.Domain.Game
{
    public class RevealedFragment
    {
        public RevealedFragment(string fileName)
        {
            FileName = fileName;
            Unlocked = false;
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public bool Unlocked { get; set; }
    }
}