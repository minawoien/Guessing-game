namespace Backend.Domain.Game
{
    public class Guess
    {
        public Guess()
        {
        }

        public Guess(string guess, Player player)
        {
            Value = guess;
            Player = player;
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public Player Player { get; set; }
    }
}