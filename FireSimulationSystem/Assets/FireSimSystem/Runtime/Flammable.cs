using UnityEngine;
/// Represents an object that can catch fire
public class Flammable : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private float fuelAmount = 8f;

    private FireSystem fireInstance;
    private Renderer rend;
    private Color originalColor;
    
    /// Initializes the Flammable object by storing its original color
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }
    
    /// Ignites the flammable object by spawning a fire instance
    public void Ignite()
    {
        /// Prevent multiple ignitions
        if (fireInstance != null) return;
        /// Error check
        if (firePrefab == null)
        {
            Debug.LogError($"No firePrefab assigned on {name}");
            return;
        }

        // Spawn fire slightly above the object
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject fireObj = Instantiate(firePrefab, spawnPos, Quaternion.identity);
        fireInstance = fireObj.GetComponent<FireSystem>();
        /// Error check
        if (fireInstance == null)
        {
            Debug.LogError("Fire prefab is missing FireSystem.");
            return;
        }

        // Add fuel & ignite
        fireInstance.AddFuel(fuelAmount);
        fireInstance.Ignite();

        // VISUAL FEEDBACK - change color to black
        if(rend != null)
        {
            rend.material.color = Color.black;
        }
    }
}