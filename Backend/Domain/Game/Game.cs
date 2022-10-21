using System;
using System.Collections.Generic;
using Backend.Domain.Images;
using Backend.SharedKernel;

namespace Backend.Domain.Game
{
    public class Game : BaseEntity
    {
        public Game()
        {
            Guesses = new List<Guess>();
            Players = new List<Player>();
            RevealedFragments = new List<RevealedFragment>();
        }

        public Game(Type type)
        {
            Type = type;
            StartTime = DateTime.Now.ToString("ddd, d MMM y, HH:mm");
            Winner = "";
            Guesses = new List<Guess>();
            Players = new List<Player>();
            RevealedFragments = new List<RevealedFragment>();
        }

        public int Id { get; set; }
        public int TeamScore { get; set; }
        public string Winner { get; set; }
        public bool UseOracle { get; set; }
        public Oracle Oracle { get; set; }
        public string StartTime { get; set; }
        public List<Player> Players { get; set; }
        public Type Type { get; set; }
        public ImageInfo Image { get; set; }
        public Status Status { get; set; }
        public List<Guess> Guesses { get; set; }
        public int UnlockedFragments { get; set; }
        public List<RevealedFragment> RevealedFragments { get; set; }

        public void AddPlayers(int userId, string userName, Role role)
        {
            var player = new Player(userId, role, userName);
            Players.Add(player);
        }

        public void AddGuess(string guess, Player player)
        {
            var newGuess = new Guess(guess, player);
            player.GuessCount++;
            Guesses.Add(newGuess);
        }

        public void AddOracle(int numImg)
        {
            Oracle = new Oracle(numImg);
        }

        public void AddImage(int imgId, string label)
        {
            Image = new ImageInfo(imgId, label);
        }

        public void CreateRevealedFragments(List<string> fragmentList)
        {
            foreach (var item in fragmentList)
            {
                RevealedFragments.Add(new RevealedFragment(item));
            }
        }
    }
}