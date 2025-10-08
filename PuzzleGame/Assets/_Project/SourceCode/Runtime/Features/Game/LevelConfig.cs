using UnityEngine;

namespace Runtime.Features.Game
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public Sprite Image;
    }
}