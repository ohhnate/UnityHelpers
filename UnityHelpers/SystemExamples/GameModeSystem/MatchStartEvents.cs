// MatchStartEvents.cs - A class for handling events related to starting a game mode match.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This MatchStartEvents class is responsible for handling events related to starting a game mode match, such as
// setting the game mode to "Capture the Flag" and starting the match. The class accesses the GameHandler singleton
// instance to make changes to the game state. 
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;

namespace UnityHelpers
{
    public class MatchStartEvents : MonoBehaviour
    {
        private GameHandler _gameHandler;

        private void Start()
        {
            _gameHandler = GameHandler.Instance;
        }

        // This could be called from an event or UI element to set the mode to capture the flag
        public void SetCaptureTheFlag()
        {
            _gameHandler.SetGameMode(new CaptureTheFlag(_gameHandler, new Scoreboard()));
        }

        // This could be called from an event or UI element to start the match after the mode has been set
        public void StartGameModeMatch()
        {
            _gameHandler.StartMatch();
        }
    }
}
