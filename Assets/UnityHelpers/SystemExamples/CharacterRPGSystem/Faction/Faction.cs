// Faction.cs - A class representing a faction in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Faction class is an abstract class and represents a faction in a game. It contains properties for the faction's name, leader, ranks, members, and associated groups.
// A faction can have multiple ranks, each with a name and priority. The faction leader must have a minimum rank specified during faction creation.
// The faction also maintains a list of members who belong to the faction and a list of groups associated with the faction.
//
// The Faction class provides methods to set the faction leader, add ranks, add members with their respective ranks, remove members, and manage groups.
// The faction leader must be one of the faction's members, and their rank must meet the minimum rank requirement specified during faction creation.
// Members can be added or removed from the faction, and they can be assigned to a group within the faction. Groups can be added or removed from the faction.
//
// The FactionRank class is an abstract class representing a rank within a faction. It contains properties for the rank's name and priority.
// The priority is used to determine the order of ranks within the faction. Lower values indicate higher priority.
//
// The FactionMember class is an abstract class representing a member of a faction. It contains properties for the member's name, rank, and assigned group.
// Members can be assigned a rank within the faction, and they can be assigned to a group. A member can only belong to 1 faction or group.
//
// The Group class is an abstract class representing a group within a faction. It contains properties for the group's name, leader minimum rank, leader, and members.
// The group leader must be one of the group's members, and their rank must meet the minimum rank requirement specified during group creation.
// The group maintains a list of members who belong to the group.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Faction
{
    public abstract class Faction
    {
        public string Name { get; }
        public FactionRank LeaderMinRank { get; }
        public FactionMember Leader { get; private set; }
        public List<FactionRank> Ranks { get; }
        public List<FactionMember> Members { get; }
        public List<Group> Groups { get; }

        public Faction(string name, FactionRank leaderMinRank)
        {
            Name = name;
            LeaderMinRank = leaderMinRank;
            Ranks = new List<FactionRank>();
            Members = new List<FactionMember>();
            Groups = new List<Group>();
        }

        public void SetLeader(FactionMember member)
        {
            if (Members.Contains(member))
            {
                Leader = member;
            }
        }

        public void AddRank(FactionRank rank)
        {
            Ranks.Add(rank);
        }

        public void AddMember(FactionMember member, FactionRank rank)
        {
            if (!Ranks.Contains(rank)) return;
            
            // Remove the member from any existing faction or group
            member.RemoveFromFactionAndGroup();
            member.SetRank(rank);
            member.SetFaction(this);
            Members.Add(member);
        }
        
        public void RemoveMember(FactionMember member)
        {
            if (!Members.Contains(member)) return;
            
            if (member == Leader)
            {
                Leader = null;
            }
            if (member.AssignedGroup != null)
            {
                member.AssignedGroup.RemoveMember(member);
                member.UnassignGroup();
            }
            Members.Remove(member);
        }
        
        public void AddGroup(Group group)
        {
            Groups.Add(group);
        }
        
        public void RemoveGroup(Group group)
        {
            if (!Groups.Contains(group)) return;
            
            Groups.Remove(group);
            foreach (FactionMember member in Members)
            {
                if (member.AssignedGroup == group)
                {
                    member.UnassignGroup();
                }
            }
        }
    }

    public abstract class FactionRank
    {
        public string Name { get; }
        public int Priority { get; }

        public FactionRank(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }
    }

    public abstract class FactionMember
    {
        public string Name { get; }
        public FactionRank Rank { get; private set; }
        public Group AssignedGroup { get; private set; }
        public Faction Faction { get; private set; }

        public FactionMember(string name)
        {
            Name = name;
        }

        public void SetRank(FactionRank rank)
        {
            Rank = rank;
        }
        
        public void SetFaction(Faction faction)
        {
            Faction = faction;
        }
        
        public void SetGroup(Group group)
        {
            AssignedGroup = group;
        }
        
        public void UnassignGroup()
        {
            AssignedGroup = null;
        }
        
        public void RemoveFromFactionAndGroup()
        {
            if (Faction != null)
            {
                Faction.RemoveMember(this);
                Faction = null;
            }
            if (AssignedGroup != null)
            {
                AssignedGroup.RemoveMember(this);
                UnassignGroup();
            }
        }
    }
    
    public abstract class Group
    {
        public string Name { get; }
        public Faction Faction { get; }
        public FactionRank LeaderMinRank { get; }
        public FactionMember GroupLeader { get; private set; }
        public List<FactionMember> Members { get; }

        public Group(string name, Faction faction, FactionRank leaderMinRank)
        {
            Name = name;
            Faction = faction;
            LeaderMinRank = leaderMinRank;
            Members = new List<FactionMember>();
        }

        public void SetGroupLeader(FactionMember member)
        {
            if (Members.Contains(member) && member.Rank.Priority >= LeaderMinRank.Priority)
            {
                GroupLeader = member;
            }
        }

        public void AddMember(FactionMember member)
        {
            if (!Faction.Members.Contains(member) || member.AssignedGroup != null) return;
            
            // Remove the member from any existing group
            member.UnassignGroup();
            member.SetGroup(this); // Set the group for the member
            Members.Add(member);
        }
        
        public void RemoveMember(FactionMember member)
        {
            if (!Members.Contains(member)) return;
            
            if (member == GroupLeader)
            {
                GroupLeader = null;
            }
            member.UnassignGroup();
            Members.Remove(member);
        }
    }
}