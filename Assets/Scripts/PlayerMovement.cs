using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    private Camera mainCamera;
    #region Server
    [Command]
 private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; } //valid position de�ilse bo� d�n.

        agent.SetDestination(hit.position);
    }

    #endregion Server

    #region Client
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    [ClientCallback] //serverda �al�o�mas�n� engelliyor sadece o clientlar�n hepsinde
    private void Update()
    {
        if (!hasAuthority) { return; } // kullanan clienta ait mi?
        if (!Input.GetMouseButtonDown(1)) { return; } //right click bas�ld� m�?
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); //cursor nerede detectle
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity/*raycasti sonsuza g�nderiyoruz bir �eye �arpana kadar durmas�n*/)) 
        { return; } //nerede bast�k ona bak, e�er scene d��� ise bas�lan yer return.
        CmdMove(hit.point);            

    }
    #endregion Client
}
