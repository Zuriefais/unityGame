using UnityEngine;
namespace SaveDate
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }

    [System.Serializable]
    public class PlayersPositions
    {
        public string playerName;
        public Vector2Int playerPosition;
    }

    public class Settings
    {
        public bool isFulscreen;
    }
}