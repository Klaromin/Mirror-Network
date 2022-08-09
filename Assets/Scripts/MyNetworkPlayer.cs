using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Linq;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerNameText = null;

    [SerializeField] private Renderer playerColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandlePlayerColorUpdated))]
    [SerializeField]
    private Color playerColor = Color.black;



    #region Server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetPlayerColor(Color newPlayerColor)
    {
        playerColor = newPlayerColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        string[] notAllowed = {"." , ",", "!", "?", "‏", "צ", "ü", "נ"};
        if (newDisplayName.Length < 2 || newDisplayName.Length > 15 || notAllowed.Any(v => newDisplayName.Contains(v))){
            Debug.Log("This Nickname no Allowed"); return; } 
        
        SetDisplayName(newDisplayName);
        RpcSetLogName("ננננננננ");
    }

    #endregion Server



    #region Client
    private void HandlePlayerColorUpdated(Color previousColor, Color nextColor)
    {
        playerColorRenderer.material.SetColor("_BaseColor", nextColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        playerNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetServerName()
    {
        CmdSetDisplayName("ננננננננ");
    }

    [ClientRpc]
    
    public void RpcSetLogName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    } 
    #endregion Client
}
