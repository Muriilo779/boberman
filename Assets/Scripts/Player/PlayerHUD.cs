using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerHUD : NetworkBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        public TextMeshProUGUI isMoonwalkingText;
        private void Start()
        {
            isMoonwalkingText = GameObject.Find("IsMoonwalkingText").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            isMoonwalkingText.text = _playerManager.WalkingState.IsMoonwalking ? "1" : "0";
        }
    }
}