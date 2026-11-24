using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class Attack : MonoBehaviour
{
    public string name;
    [Header("References")] 
    public GameObject hitbox;
    public InputActionReference attackAction;
    public StaminaBar staminaBar;

    [Header("Attacking")] 
    public float attackDuration = 1f;
    public bool isAttacking = false;
    public float damage = -20f;
    public float staminaCost = 20f;
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    
    [Header("Cooldown")]
    public float attackCooldown = 0.08f;
    private float attackCdTimer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackAction.action.Enable();
        attackAction.action.performed += onAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCdTimer > 0f)
            attackCdTimer -= Time.deltaTime;
    }

    private void onAttack(InputAction.CallbackContext context)
    {
        if (isAttacking || attackCdTimer > 0f || staminaBar.slider.value < staminaCost) return;
        staminaBar.decrement(staminaCost);
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        hitTargets.Clear();
        attackCdTimer = attackCooldown;
        hitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        hitbox.SetActive(false);
        isAttacking = false;
    }

    public void HandleHit(Collider other)
    {
        Debug.Log("handling hit: " + other.gameObject.name);
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy") || hitTargets.Contains(other.gameObject)) return;
        hitTargets.Add(other.gameObject);
        other.gameObject.GetComponent<HealthBar>().change(damage);
    }
}
