using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpDrops : MonoBehaviour
{

    [SerializeField] private Transform PlayerOrientation;
    [SerializeField] private LayerMask DropLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTryPickup(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case { phase: InputActionPhase.Started }:
                break;

            case { phase: InputActionPhase.Performed }:
                if (Physics.SphereCast(PlayerOrientation.position, 2, PlayerOrientation.forward, out RaycastHit hitInfo, 10, DropLayer))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent<AmmoPack>(out AmmoPack ammoPack))
                    {

                    }
                    if (hitInfo.collider.gameObject.TryGetComponent<HealthPack>(out HealthPack healthPack))
                    {
                        GetComponent<PlayerHealth>().AddHealthToPlayer(healthPack.HealthGain);
                        Debug.Log("adding health to player");
                    }

                    Destroy(hitInfo.collider.transform.parent.gameObject);
                }

                break;

            case { phase: InputActionPhase.Canceled }:
                Debug.Log("released the E button");
                break;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(PlayerOrientation.position, PlayerOrientation.position + PlayerOrientation.forward * 5);
    }
}
