

namespace Tank3DMultiplayer
{
    // Important: the names in the enum value should be the same as the scene you're trying to load
    public enum SceneName : byte
    {
        Bootstrap,
        Lobby,
        Room,
        CharacterSelection,
        GamePlay,
        Summary


        // Add more scenes states if needed
    };

    public enum MenuName
    {
        MainMenu,
        SelectGameModeMenu
    }


    public enum GameMode
    {
        SoloWithFriend,
        SoloQuickMatch,
        TeamWithFriend,
        TeamQuickMatch
    }


    // States the player can have on the game
    public enum ConnectionState : byte
    {
        connected,
        disconnected,
        ready
    }

    public enum TankType
    {
        Null,
        Hulk,
        Gundam,
    }

    public enum ButtonActions : byte
    {
        lobby_ready,
        lobby_not_ready,
    }
}

