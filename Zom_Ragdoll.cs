using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zom_Ragdoll : MonoBehaviour
{
    // isKinematic = true -- DISABLES rigidbody
    // isKinematic = false-- ENABLESrigidbody

    private Rigidbody[] rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
