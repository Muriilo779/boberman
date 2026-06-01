using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class LobbyCharacterButton : MonoBehaviour
{
    [SerializeField] private GameObject charButton;
    private int _charCount;
    private void Start()
    {
        _charCount = 0;
        SetCharacter();
    }

    public void OnClickCharacter()
    {
        _charCount++;
        SetCharacter();
    }

    private void SetCharacter()
    {
        SteamLobby.currentLobby.SetData("character_" + SteamClient.SteamId, _charCount.ToString());
        Debug.Log("character_" + SteamClient.SteamId);
        Debug.Log(_charCount);
    }
}
