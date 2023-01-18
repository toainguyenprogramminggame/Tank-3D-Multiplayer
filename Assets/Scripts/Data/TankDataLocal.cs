using UnityEngine.UI;
using UnityEngine;

namespace Tank3DMultiplayer.Data
{
    [CreateAssetMenu(fileName = "TankDataLocal", menuName = "Tank/TankDataLocal", order = 1)]
    public class TankDataLocal:ScriptableObject
    {
        [SerializeField]
        private TankType tankType;

        [SerializeField]
        private Sprite tankAvatar;

        [SerializeField]
        private GameObject tankPrefab;

        public TankType TankType { get => tankType; set => tankType = value; }
        public Sprite TankAvatar { get => tankAvatar; set => tankAvatar = value; }
        public GameObject TankPrefab { get => tankPrefab; set => tankPrefab = value; }
    }
}

