

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
    };

    public enum TankType
    {
        Null,
        Hulk,
        Gundam,
        DarkKnight,
        Transformer,
        IronMan
    }
}

