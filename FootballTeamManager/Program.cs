using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//empty team instance
Team? playerTeam = null;

app.MapGet("/", () => "Hello World!");

app.MapPost("/createteam", ([FromBody] Team newTeam) =>
{
    Console.WriteLine(newTeam.Name);
    //check if a team exists first
    if (playerTeam != null && playerTeam.Name.Equals(newTeam.Name, StringComparison.OrdinalIgnoreCase)) //this is to avoid giving an error that it already exists regardless of name
    {
        return Results.Ok(new { Message = $"Team with name {playerTeam.Name} already exists" });
    }

    //create
    playerTeam = newTeam;
    return Results.Ok(new { Message = $"Team {newTeam.Name} has been created", Team = playerTeam });
});

app.MapGet("/team", () =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You haven't created a team yet" });
    }
    return Results.Ok(playerTeam.Name);
});

app.MapPost("/addplayer", ([FromBody] Player player) =>
{
    //check first
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }
    var existedPlayer = playerTeam.players.FirstOrDefault(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase));
    if (existedPlayer != null)
    {
        return Results.BadRequest(new { Message = $"Player with name {player.Name} already exists" });
    }

    //check name is empty or not 
    if (string.IsNullOrWhiteSpace(player.Name))
    {
        return Results.BadRequest(new { Message = $"Player name is required" });
    }

    playerTeam.AddPlayer(player);
    return Results.Ok(new { Message = $"Player {player.Name} has been added to the team" });
});

app.MapPut("/updateplayer/{name}", ([FromRoute] string name, [FromBody] Player updatedPlayer) =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var existedPlayer = playerTeam.players.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    if (existedPlayer == null)
    {
        return Results.NotFound(new { Message = $"Player with Name {name} not found" });
    }

    // update player details ( name, age, position, rank)
    existedPlayer.Name = updatedPlayer.Name ?? existedPlayer.Name;
    existedPlayer.Age = updatedPlayer.Age != 0 ? updatedPlayer.Age : existedPlayer.Age;
    existedPlayer.Position = updatedPlayer.Position ?? existedPlayer.Position;
    existedPlayer.Rank = updatedPlayer.Rank != 0 ? updatedPlayer.Rank : existedPlayer.Rank;

    return Results.Ok(new { Message = $"Player {name} has been successfully updated" });
});

app.MapGet("/findplayer/{name}", ([FromRoute] string name) =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var player = playerTeam.players.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    if (player == null)
    {
        return Results.NotFound(new { Message = $"Player with Name {name} not found" });
    }

    return Results.Ok(player);
});

app.MapDelete("/deleteplayer/{name}", ([FromRoute] string name) =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var existedPlayer = playerTeam.players.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    if (existedPlayer == null)
    {
        return Results.NotFound(new { Message = $"Player with Name {name} not found" });
    }

    // Remove the player from the team
    playerTeam.players.Remove(existedPlayer);

    return Results.Ok(new { Message = $"Player {name} has been deleted from the team :(" });
});

app.MapGet("/findplayerrank/{rank}", ([FromRoute] int rank) =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var player = playerTeam.players.FirstOrDefault(p => p.Rank == rank);
    if (player == null)
    {
        return Results.NotFound(new { Message = $"Player with Name {rank} not found" });
    }

    return Results.Ok(player);
});

app.MapGet("/findyoungest", () =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var youngestPlayer = playerTeam.players.OrderBy(p => p.Age).FirstOrDefault();
    if (youngestPlayer == null)
    {
        return Results.BadRequest(new { Message = "No players found in the team" });
    }

    return Results.Ok(new { Message = "Youngest player is:", Player = youngestPlayer });
});

app.MapGet("/findoldest", () =>
{
    if (playerTeam == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var oldestPlayer = playerTeam.players.OrderByDescending(p => p.Age).FirstOrDefault();
    if (oldestPlayer == null)
    {
        return Results.BadRequest(new { Message = "No players found in the team" });
    }

    return Results.Ok(new { Message = "Oldest player is:", Player = oldestPlayer });
});

app.Run();
