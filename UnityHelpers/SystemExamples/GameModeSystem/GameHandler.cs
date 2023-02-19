// GameHandler.cs - A class for managing game sessions and players.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The GameHandler class provides functionality for managing game sessions and players.
// It includes methods for adding and removing players, setting the game mode, and initializing the scoreboard.
// It also holds a list of players and the current game mode, which can be set to Classic, CaptureTheFlag, TimeAttack, or Survival.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Classic,
    CaptureTheFlag,
    TimeAttack,
    Survival
}

public class GameHandler : Handler<GameHandler>
{
    // a prefab of the player game object
    public GameObject PlayerPrefab { get; private set; }
    private List<Player> players;
    private Scoreboard scoreboard;
    public GameMode CurrentGameMode { get; private set; }
    
    public void OnUserJoinGame(string username)
    {
        // create a new player game object from the player prefab
        GameObject newPlayerGO = Instantiate(PlayerPrefab);

        // initialize the new player object with the username and add it to the list of players
        Player newPlayer = new Player(newPlayerGO, username);
        players.Add(newPlayer);

        // add the new player to the current game mode
        if (CurrentGameMode != null)
        {
            CurrentGameMode.AddPlayer(newPlayer);
        }
    }
    
    public void OnUserLeaveGame(string username)
    {
        // find the player with the given username and remove them from the list of players
        Player playerToRemove = players.Find(p => p.Name == username);
        if (playerToRemove != null)
        {
            players.Remove(playerToRemove);

            // invoke the OnPlayerLeave event to notify other systems that the player has left the game
            if (CurrentGameMode != null)
            {
                CurrentGameMode.OnPlayerLeave?.Invoke(playerToRemove);
            }
        }
    }

    public void SetGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
        scoreboard = new Scoreboard();
        CurrentGameMode.SetScoreboard(scoreboard);
        // Perform any other necessary setup for the new game mode
    }

    // Other game manager methods and properties
}