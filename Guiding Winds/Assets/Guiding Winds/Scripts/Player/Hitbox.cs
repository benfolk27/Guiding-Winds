using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Attack attackScript;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit collider");
        attackScript.HandleHit(other);
    }
}
