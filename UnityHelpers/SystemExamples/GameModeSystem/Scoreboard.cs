// Scoreboard.cs - A class for managing player scores in a game mode.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Scoreboard class manages player scores in a game mode. It provides functionality to add, remove and retrieve
// the scores of players in the game mode. The class is also equipped with an event handler for score updates.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;

namespace UnityHelpers
{
    public class Scoreboard
    {
        private readonly Dictionary<Player, int> _scores;

        public Scoreboard()
        {
            _scores = new Dictionary<Player, int>();
        }

        public void AddScore(Player player)
        {
            if (!_scores.ContainsKey(player))
            {
                _scores[player] = 0;
            }

            _scores[player]++;
        }

        public int GetScore(Player player)
        {
            return _scores.ContainsKey(player) ? _scores[player] : 0;
        }

        public void ResetScores()
        {
            _scores.Clear();
        }
    }
}