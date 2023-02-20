// CaptureTheFlag.cs - A custom game mode for capturing the flag.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This CaptureTheFlag class inherits from the GameMode abstract class to create a custom game mode with specific settings,
// rules and logic for capturing the flag. The class sets the name, time limit, score limit, and win condition display for
// the game mode. It provides methods for starting, ending and updating the game mode, as well as handling player scores
// and deaths. The class overrides the OnPlayerScored and OnPlayerDied methods to implement custom behavior for the game mode.
//
// No accreditation is required but it would be highly appreciated <3

using System;

namespace UnityHelpers
{
    public enum Team
    {
        Red,
        Blue
    }
    
    public class CaptureTheFlag : GameMode
    {
        public int BlueScore { get; private set; }
        public int RedScore { get; private set; }

        public event Action OnBlueTeamScored;
        public event Action OnRedTeamScored;        
        
        public CaptureTheFlag(GameHandler gameHandler, Scoreboard scoreboard) : base(gameHandler, scoreboard)
        {
            Name = "Capture the Flag";
            TimeLimit = TimeSpan.FromSeconds(10);
            ScoreLimit = 5;
            WinConditionDisplay = "Capture the flag";
        }

        public override void Start()
        {
            base.Start();
            // Start the Capture the Flag game mode
        }

        public override void End()
        {
            base.End();
            // End the Capture the Flag game mode
        }

        public override void Update()
        {
            base.Update();
            // Update the Capture the Flag game mode
        }

        public override void OnPlayerScored(Player player)
        {
            switch (player.Team)
            {
                case Team.Blue:
                {
                    BlueScore++;
                    OnBlueTeamScored?.Invoke();

                    if (BlueScore >= ScoreLimit)
                    {
                        End();
                    }
                    break;
                }
                case Team.Red:
                {
                    RedScore++;
                    OnRedTeamScored?.Invoke();
                    if (RedScore >= ScoreLimit)
                    {
                        End();
                    }
                    break;
                }
            }
        }

        public override void OnPlayerDied(Player player)
        {
            // Handle a player dying in the Capture the Flag game mode
        }
    }
}