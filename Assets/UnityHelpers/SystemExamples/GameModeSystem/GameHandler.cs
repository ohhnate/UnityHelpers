// GameHandler.cs - A class for managing game sessions and players.
// Version 1.1.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// The GameHandler class provides functionality for managing game sessions and players.
// It includes methods for adding and removing players, setting the game mode, and initializing the scoreboard.
// It also holds a list of players and the current game mode, which can be set to Classic, CaptureTheFlag, TimeAttack, or Survival.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityHelpers.SystemExamples.GameModeSystem
{
    public class GameHandler : Handler<GameHandler>
    {
        // a prefab of the player game object
        public GameObject PlayerPrefab { get; private set; }
        private List<Player> _players;
        public GameMode CurrentGameMode { get; private set; }
        private MatchRunner _matchRunner;

        // Would be called from a UI invent to Initiate the match
        public void StartMatch()
        {
            if (_matchRunner != null)
            {
                Debug.LogError("Match already running!");
                return;
            }
            _matchRunner = new MatchRunner(CurrentGameMode);
            _matchRunner.Start();
        }

        private void Update()
        {
            _matchRunner?.Update();
        }

        public void StopMatch()
        {
            if (_matchRunner == null)
            {
                Debug.LogError("Match is not running!");
                return;
            }
            _matchRunner.Stop();
            _matchRunner = null;
        }

        public void OnUserJoinGame(string username)
        {
            // create a new player game object from the player prefab
            GameObject newPlayerGo = Instantiate(PlayerPrefab);

            // determine the team to assign to the player based on the number of players already on each team
            int blueCount = _players.Count(player => player.Team == Team.Blue);
            int redCount = _players.Count(player => player.Team == Team.Red);
            Team playerTeam = blueCount <= redCount ? Team.Blue : Team.Red;
            // initialize the new player object with the username and add it to the list of players
            Player newPlayer = new Player(newPlayerGo, username, playerTeam);
            _players.Add(newPlayer);

            // add the new player to the current game mode
            CurrentGameMode.AddPlayer(newPlayer);
        }

        public void OnUserLeaveGame(string username)
        {
            // find the player with the given username and remove them from the list of players
            Player playerToRemove = _players.Find(p => p.Name == username);
            if (playerToRemove == null) return;

            _players.Remove(playerToRemove);

            // invoke the OnPlayerLeave event to notify other systems that the player has left the game
            CurrentGameMode?.OnPlayerLeaveGame(playerToRemove);
        }

        // Would be called from a UI invent to set the GameMode based on selection
        public void SetGameMode(GameMode mode)
        {
            if (_matchRunner != null)
            {
                Debug.LogWarning("Match is already running, stopping it to change game mode...");
                StopMatch();
            }
            CurrentGameMode = mode;
            // Perform any other necessary setup for the new game mode
        }

        // MatchRunner.cs - A class for running a chosen GameMode.
        // Version 1.0.0
        // Author: Nate
        // Website: https://github.com/ohhnate
        //
        // The MatchRunner class is responsible for starting, stopping, and updating a chosen GameMode.
        // The class also has error handling for cases where the Match is not running when methods are called.
        // 
        // No accreditation is required but it would be highly appreciated <3

        private class MatchRunner
        {
            private readonly GameMode _gameMode;
            private bool _isRunning;

            public MatchRunner(GameMode gameMode)
            {
                _gameMode = gameMode;
            }

            public void Start()
            {
                if (_isRunning)
                {
                    Debug.LogError("Match already running!");
                    return;
                }
                _gameMode.Start();
                _isRunning = true;
            }

            public void Stop()
            {
                if (!_isRunning)
                {
                    Debug.LogError("Match not running!");
                    return;
                }
                _gameMode.End();
                _isRunning = false;
            }

            public void Update()
            {
                if (!_isRunning)
                {
                    Debug.LogError("Match not running!");
                    return;
                }
                _gameMode.Update();
            }
        }
    }
}