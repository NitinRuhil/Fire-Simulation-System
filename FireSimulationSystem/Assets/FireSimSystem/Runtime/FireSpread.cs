using UnityEngine;

/// Controls how fire spreads to nearby flammable objects.
[RequireComponent(typeof(FireSystem))]
public class FireSpread : MonoBehaviour
{
    [Header("Spread Settings")]
    [SerializeField] private float spreadRadius = 2f;
    [SerializeField] private float spreadChancePerSecond = 0.3f;

    private FireSystem fireSystem;
    
    /// Initializes the FireSystem reference.
    private void Awake()
    {
        fireSystem = GetComponent<FireSystem>();
    }

    /// Checks for nearby flammable objects and attempts to ignite them based on spread chance.
    private void Update()
    {
        if (fireSystem.GetIntensity() <= 0.3f) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach (Collider hit in hits)
        {
            Flammable flammable = hit.GetComponent<Flammable>();
            if (flammable == null) continue;
            float chance = spreadChancePerSecond * Time.deltaTime;
            if (Random.value < chance)
            {
                flammable.Ignite();
            }
        }
        
    }

    /// Visualize the spread radius in the editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}