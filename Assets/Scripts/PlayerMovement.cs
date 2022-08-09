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
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; } //valid position deðilse boþ dön.

        agent.SetDestination(hit.position);
    }

    #endregion Server

    #region Client
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    [ClientCallback] //serverda çalýoþmasýný engelliyor sadece o clientlarýn hepsinde
    private void Update()
    {
        if (!hasAuthority) { return; } // kullanan clienta ait mi?
        if (!Input.GetMouseButtonDown(1)) { return; } //right click basýldý mý?
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); //cursor nerede detectle
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity/*raycasti sonsuza gönderiyoruz bir þeye çarpana kadar durmasýn*/)) 
        { return; } //nerede bastýk ona bak, eðer scene dýþý ise basýlan yer return.
        CmdMove(hit.point);            

    }
    #endregion Client
}
