

namespace Tank3DMultiplayer
{
    public static class ConstValue
    {
        public const string KEY_RELAY_JOIN_CODE = "RelayID";
        public const string KEY_PLAYER_NAME = "PlayerName";
        public const string KEY_PLAYER_READY = "Ready";
        public const string KEY_PLAYER_AVATAR = "Avatar";
        public const string KEY_VALUE_IS_READY = "Ready";
        public const string KEY_VALUE_NOT_READY = "NotReady";

        public const int MAX_PLAYERS = 4;

        public const string LOBBY_NAME = "my lobby";

        public const float LOBBY_POLL_TIMER = 1.5f;

        public const float MIN_TIME_ENABLE = 1.2f;

        public const float LOBBY_HEART_BEAT_TIMER = 15f;

        public const int MAX_RELAY_CONNECTION = 4;

        public const float TIME_TO_START_GAME = 5f;

        public const float TIME_TO_SELECT_TANK = 10f;

        public const float DEFAULT_Y_SHOOT = 0f;

        public const int PREWARM_COUNT_SPAWN = 5;

        public const string TAG_TANK = "Tank";

        public const float OFFSET_CAMERA_Y = 65f;

        public const float TIME_REMAIN_GAMEPLAY = 40f;

        public const float TIME_TO_RESPAWN = 6f;

        public const int REWARD_PER_KILL = 5;

        public const string LOAD_DATA_TANKS = "http://127.0.0.1:3000/data/tanks";

        public const string PREF_KEY_PLAYER_NAME = "PlayerName";
        public const string PREF_KEY_PLAYER_AVATAR = "PlayerAvatar";
    }
    // Important: the names in the enum value should be the same as the scene you're trying to load
    public enum SceneName : byte
    {
        Bootstrap,
        Lobby,
        CharacterSelection,
        GamePlay,
        Result
    };

    public enum MenuName
    {
        MainMenu,
        ListTankMenu,
        SettingMenu,
        CreateAndJoinRoomMenu,
        RoomMenu,
        DetailTankMenu,
        ProfileMenu
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

    public enum ProfileAvatar
    {
        Hulk,
        Gundam,
        DarkKnight,
        Transformer,
        IronMan
    }
}

