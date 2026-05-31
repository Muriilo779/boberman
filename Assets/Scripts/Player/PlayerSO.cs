using System;
using UnityEngine;

namespace Scripts.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName="ScriptableObjects/PlayerSO")]
    public class PlayerSO: ScriptableObject
    {
        public string characterName;
        public int maxHealth;
        public int initialExplosionRadius;
        public int maxExplosionRadius;
        public int maxBombs;
        public float maxMoveSpeed;
        public float moveSpeed;
        public Sprite icon;
    }
}