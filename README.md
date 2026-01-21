# HOI4 Announcer

HOI4 Announcer is a Discord bot designed to manage and announce Hearts of Iron IV (HOI4) multiplayer games. It allows organizers to schedule games, manage factions and nations, and enables players to join specific nations via Discord slash commands.

## Features

- **Game Scheduling**: Set a start time for your HOI4 games.
- **Faction & Nation Management**: Add or remove nations and factions for each game.
- **Player Signups**: Players can join nations directly through Discord.
- **Automated Notifications**: Sends DM notifications to joined players before the game starts.
- **Game Locking**: Lock games to prevent further signups.
- **Logging**: Log game-related activities to a dedicated channel.

## Hosting Guide

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A Discord Bot Token (obtainable from the [Discord Developer Portal](https://discord.com/developers/applications))

### Installation & Setup

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/yourusername/HOI4Announcer.git
    cd HOI4Announcer
    ```

2.  **Configuration**:
    When you first run the bot, it will generate a `config.yml` file. You need to fill in your bot token and channel IDs.
    
    Example `config.yml`:
    ```yaml
    bot:
      token: "YOUR_DISCORD_BOT_TOKEN"
      log-channel: 123456789012345678 # ID of the channel for logs
      game-channel: 123456789012345678 # ID of the channel where game embeds are posted
      blocked-users: []
    ```

3.  **Build and Run**:
    ```bash
    dotnet build
    dotnet run
    ```

### Hosting with Docker (Optional)

You can also containerize the application for easier hosting.

```dockerfile
# Example Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "HOI4Announcer.dll"]
```

## Commands Usage

The bot uses Discord Slash Commands. Below is a list of (some) available commands:

### Game Management
- `/newgame <starttime>`: Start a new game with a specified start time (YYYY-MM-DD HH:MM).
- `/setgametime <starttime>`: Update the start time of the current game.
- `/lockgame`: Lock the current game to prevent more players from joining.
- `/unlockgame`: Unlock the current game.
- `/endgame [winner]`: End the current game and clear the status. Add a winner to announce in chat and to write into the savefile.
- `/setnotification <minutes>`: Set how many minutes before the start time players should be notified.

### Player Commands
- `/joinnation <nation>`: Join a specific nation in the current game.
- `/leavenation`: Leave your currently joined nation.

### Nation & Faction Management
- `/addnation <faction> <nation> [maxplayers]`: Add a nation to a faction in the current game.
- `/removenation <nation>`: Remove a nation from the current game.
- `/clearfaction <faction>`: Remove all nations from a specific faction.
- `/setmaxplayers <nation> <count>`: Set the maximum number of players for a nation.

### Default Configuration (Templates)
- `/adddefaultnation <faction> <nation> [maxplayers]`: Add a nation to the default template.
- `/removedefaultnation <nation>`: Remove a nation from the default template.
- `/cleardefaultfaction <faction>`: Clear all nations from a default faction.
- `/setdefaultmaxplayers <nation> <count>`: Set default max players for a nation.

### Admin Commands
- `/blockuser <user>`: Block a user from joining games.
- `/unblockuser <user>`: Unblock a user.
- `/adduser <nation> <user>`: Forcefully add a user to a nation.
- `/removeuser <user>`: Forcefully remove a user from their nation.

## Contributions

Contributions are welcome! If you'd like to improve HOI4 Announcer, please follow these steps:

1.  **Fork the repository**.
2.  **Create a new branch** for your feature or bugfix:
    ```bash
    git checkout -b feature/your-feature-name
    ```
3.  **Make your changes** and ensure the code follows the existing style.
4.  **Commit your changes**:
    ```bash
    git commit -m "Add some feature"
    ```
5.  **Push to the branch**:
    ```bash
    git push origin feature/your-feature-name
    ```
6.  **Open a Pull Request**.

Please ensure your code is well-documented and any new commands are added to the README.

## Acknowledgments

I would like to thank Karl "KarlOfDuty" Essinger for his contributions. A lot of the code is based on his previous work [RoleBoi](https://github.com/KarlOfDuty/RoleBoi) and he has given plenty of mentoring and guidance in the execution of this project. 
Code is partly generated through Jetbrains AI using Junie to generate simple, repetitive segments.

## License

This project is not licensed yet, pending a choice- see the [LICENSE](LICENSE) file for details (when available).
