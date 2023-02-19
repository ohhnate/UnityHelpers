// Scoreboard.cs - A class for managing player scores in a game mode.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Scoreboard class manages player scores in a game mode. It provides functionality to add, remove and retrieve
// the scores of players in the game mode. The class is also equipped with an event handler for score updates.
//
// No accreditation is required but it would be highly appreciated <3

public class Scoreboard
{
    private Dictionary<Player, int> scores;

    public Scoreboard()
    {
        scores = new Dictionary<Player, int>();
    }

    public void AddScore(Player player)
    {
        if (!scores.ContainsKey(player))
        {
            scores[player] = 0;
        }
        scores[player]++;
    }

    public int GetScore(Player player)
    {
        return scores.ContainsKey(player) ? scores[player] : 0;
    }

    public void ResetScores()
    {
        scores.Clear();
    }
}