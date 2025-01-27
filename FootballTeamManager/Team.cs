public class Team
{
    public string Name { get; set; } = "";
    public List<Player> players = new List<Player>();

    public Team() { }


    //add new player to the gang

    public string AddPlayer(Player player)
    {
        var existedPlayer = players.FirstOrDefault(p => p.Name == player.Name);

        if (existedPlayer != null)
        {
            return $"Player with name {player.Name} already exists"; //not added yet
        }

        players.Add(player);
        return $"Player {player.Name} has been added";
    }

    public string RemovePlayer(string playerName)
    {
        var existedPlayer = players.FirstOrDefault(p => p.Name == playerName);

        if (existedPlayer == null)
        {
            return $"Player {playerName} not found";
        }
        players.Remove(existedPlayer);
        return $"Player {playerName} has been removed from the team";
    }

    public string FindPlayerByRank(int playerRank)
    {
        var rankedPlayers = players.Where(p => p.Rank == playerRank).ToList();

        if (rankedPlayers.Count == 0)
        {
            return $"No players found with rank {playerRank}.";
        }

        return $"Players with rank {playerRank}: " + string.Join(", ", rankedPlayers.Select(p => $"{p.Name}"));
    }
    public string FindPlayerByName(string playerName)
    {
        var player = players.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            return $"Player {playerName} not found.";
        }

        return $"Player found: {player.Name}, Age: {player.Age}, Position: {player.Position}, Rank: {player.Rank}.";
    }

    public string UpdatePlayer(string playerName, string newName, int newAge, string newPosition, int newRank)
    {
        var player = players.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            return $"Player {playerName} not found.";
        }

        player.Name = newName;
        player.Age = newAge;
        player.Position = newPosition;
        player.Rank = newRank;

        return $"Player {playerName} updated. New details: Name: {newName}, Age: {newAge}, Position: {newPosition}, Rank: {newRank}.";
    }

    public string GetTeamAges()
    {
        if (players.Count == 0)
        {
            return "No players found in the team.";
        }

        var oldestPlayer = players.OrderByDescending(p => p.Age).FirstOrDefault();
        var youngestPlayer = players.OrderBy(p => p.Age).FirstOrDefault();

        //i'm unsure why is this giving a warning but the code works lol
        return $"Oldest player: {oldestPlayer.Name}, Age: {oldestPlayer.Age}; " + $"Youngest player: {youngestPlayer.Name}, Age: {youngestPlayer.Age}.";
    }

}