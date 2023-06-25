// Player.cs - A class representing a player in a game.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Player class contains information about a player in a game, such as their name and associated game object.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;

namespace UnityHelpers.SystemExamples.GameModeSystem
{
    public class Player
    {
        public string Name { get; }
        public Team Team { get; }
        public GameObject GameObject { get; }

        public Player(GameObject gameObject, string name, Team team)
        {
            GameObject = gameObject;
            Name = name;
            Team = team;
        }
    }
}