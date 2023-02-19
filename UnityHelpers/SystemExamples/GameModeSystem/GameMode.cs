// GameMode.cs - An abstract class for creating custom game modes.
// Version 1.0.0
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

public abstract class GameMode
{
    public string Name { get; protected set; }
    public int ScoreLimit { get; protected set; }
    public TimeSpan TimeElapsed { get; set; }
    public TimeSpan? TimeLimit { get; set; } = null;
    public string OptionalParameter { get; set; } = "Default value";
    public string WinConditionDisplay { get; protected set; }
    
    protected GameHandler gameHandler;
    protected Scoreboard scoreboard;
    protected Timer timer;
    
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action<Player> OnPlayerJoin;
    public event Action<Player> OnPlayerLeave;
    public event Action<int, int> OnScoreUpdated;
    public event Action<TimeSpan> OnTimeUpdated;
    
    public GameMode(GameHandler gameHandler, Scoreboard scoreboard)
    {
        this.gameHandler = gameHandler;
        this.scoreboard = scoreboard;
        if (TimeLimit.HasValue)
        {
            timer = new Timer(TimeLimit.Value);
            timer.OnTimerExpired += EndGame;
            timer.Start();
        }
    }

    public virtual void Start()
    {
        OnGameStart?.Invoke();
        // Start the game mode
    }

    public virtual void End()
    {
        if (timer != null)
        {
            OnGameEnd?.Invoke();
            timer.Stop();
            timer.OnTimerExpired -= EndGame;
        }
    }

    public virtual void Update()
    {
        if (timer != null)
        {
            timer.Update();
            TimeElapsed = timer.TimeElapsed;
            OnTimeUpdated?.Invoke(TimeElapsed);
        }
        // Update the game mode
    }

    public virtual void OnPlayerScored(Player player)
    {
        scoreboard.AddScore(player);
        OnScoreUpdated?.Invoke(scoreboard.GetScore(player), ScoreLimit);
        if (scoreboard.GetScore(player) >= ScoreLimit)
        {
            EndGame();
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