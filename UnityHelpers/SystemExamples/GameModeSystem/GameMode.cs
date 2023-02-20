// GameMode.cs - An abstract class for creating custom game modes.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This GameMode abstract class can be inherited to create custom game modes with specific settings, rules and logic.
// It provides common functionality, like handling player scores, updating the game timer, adding and removing players, and
// handling player deaths. The class is also equipped with event handlers for start/end game, player join/leave and score updates.
// A GameHandler and Scoreboard instance are required to be passed in the constructor. A timer instance will be created
// if a time limit is set.
//
// No accreditation is required but it would be highly appreciated <3

using System;

namespace UnityHelpers
{
    public abstract class GameMode
    {
        public string Name { get; protected set; }
        public int ScoreLimit { get; protected set; }
        public TimeSpan TimeElapsed { get; set; }
        public TimeSpan? TimeLimit { get; set; }
        public string WinConditionDisplay { get; protected set; }

        protected GameHandler GameHandler;
        protected readonly Scoreboard Scoreboard;
        protected readonly Timer Timer;

        public event Action OnGameStart;
        public event Action OnGameEnd;
        public event Action<Player> OnPlayerJoin;
        public event Action<Player> OnPlayerLeave;
        public event Action<int, int> OnScoreUpdated;
        public event Action<TimeSpan> OnTimeUpdated;

        public GameMode(GameHandler gameHandler, Scoreboard scoreboard)
        {
            GameHandler = gameHandler;
            Scoreboard = scoreboard;
            if (!TimeLimit.HasValue) return;
            
            Timer = new Timer(TimeLimit.Value);
            Timer.OnTimerExpired += End;
            Timer.Start();
        }

        public virtual void Start()
        {
            OnGameStart?.Invoke();
            // Start the game mode
        }

        public virtual void End()
        {
            if (Timer == null) return;
            
            OnGameEnd?.Invoke();
            Timer.Stop();
            Timer.OnTimerExpired -= End;
        }

        public virtual void Update()
        {
            if (Timer == null) return;
            
            Timer.Update();
            TimeElapsed = Timer.TimeElapsed;
            OnTimeUpdated?.Invoke(TimeElapsed);
            // Update the game mode
        }

        public virtual void OnPlayerScored(Player player)
        {
            Scoreboard.AddScore(player);
            OnScoreUpdated?.Invoke(Scoreboard.GetScore(player), ScoreLimit);
            if (Scoreboard.GetScore(player) >= ScoreLimit)
            {
                End();
            }
            // Handle a player scoring in the game mode
        }

        public virtual void AddPlayer(Player player)
        {
            // invoke the OnPlayerJoin event to notify other systems that a new player has joined the game
            OnPlayerJoin?.Invoke(player);
        }

        public virtual void OnPlayerLeaveGame(Player player)
        {
            // invoke the OnPlayerLeave event to notify other systems that a player has left the game
            OnPlayerLeave?.Invoke(player);
        }

        public virtual void OnPlayerDied(Player player)
        {
            // Handle a player dying in the game mode
        }
    }
}