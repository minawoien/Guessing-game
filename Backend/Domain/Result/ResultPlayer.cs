namespace Backend.Domain.Result
{
    public class ResultPlayer
    {
        public ResultPlayer()
        {
        }

        public ResultPlayer(string userName)
        {
            UserName = userName;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
    }
}