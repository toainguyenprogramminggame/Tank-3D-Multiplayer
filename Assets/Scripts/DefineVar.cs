

namespace Tank3DMultiplayer
{
    public static class ConstValue
    {
        public const string KEY_PLAYER_NAME = "PlayerName";
        public const string KEY_PLAYER_READY = "Ready";
        public const string KEY_VALUE_IS_READY = "Ready";
        public const string KEY_VALUE_NOT_READY = "NotReady";

        public const int MAX_PLAYERS = 4;

        public const string LOBBY_NAME = "my lobby";

        public const float LOBBY_POLL_TIMER = 1.5f;

        public const float MIN_TIME_ENABLE = 1.2f;

        public const float LOBBY_HEART_BEAT_TIMER = 15f;

        public const int MAX_RELAY_CONNECTION = 4;
    }
    // Important: the names in the enum value should be the same as the scene you're trying to load
    public enum SceneName : byte
    {
        Bootstrap,
        Lobby,
        Room,
        CharacterSelection,
        GamePlay,
        Summary
    };

    public enum MenuName
    {
        MainMenu,
        ListTankMenu,
        SettingMenu,
        CreateAndJoinRoomMenu,
        RoomMenu
    }

    public enum TankType
    {
        Null,
        Hulk,
        Gundam,
        DarkKnight,
        Transformer,
        IronMan
    };

    public enum AuthState
    {
        Initialized,
        Authenticating,
        Authenticated,
        Error,
        TimedOut
    };
}

