using UnityEngine;

/// Controls visual representation of fire based on FireSystem state.
[RequireComponent(typeof(ParticleSystem))]
public class FireVisualController : MonoBehaviour
{
    private FireSystem fireSystem;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

    [Header("Visual Scaling")]

    [Header("Wind Influence")]
    [SerializeField] private float windForceMultiplier = 1.5f;
    [SerializeField] private float maxEmissionRate = 50f;
    [SerializeField] private float maxParticleSpeed = 3f;
    
    /// Initializes references to ParticleSystem and FireSystem.
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
        fireSystem = GetComponentInParent<FireSystem>();
    }
    
    /// Subscribes to FireSystem events when enabled.
    private void OnEnable()
    {
        /// Subscribe to fire events
        if (fireSystem != null)
        {
            fireSystem.OnIgnited += OnIgnited;
            fireSystem.OnExtinguished += OnExtinguished;
            fireSystem.OnIntensityChanged += OnIntensityChanged;
        }
    }
    
    /// Unsubscribes from FireSystem events when disabled.
    private void OnDisable()
    {
        /// Safety check
        if (fireSystem != null)
        {
            fireSystem.OnIgnited -= OnIgnited;
            fireSystem.OnExtinguished -= OnExtinguished;
            fireSystem.OnIntensityChanged -= OnIntensityChanged;
        }
    }

    /// Handles fire ignition event.
    private void OnIgnited()
    {
        ps.Play();
    }
    
    /// Handles fire extinguishment event.
    private void OnExtinguished()
    {
        ps.Stop();
    }
    
    /// Handles changes in fire intensity to adjust visual effects.
    private void OnIntensityChanged(float intensity)
    {
        emission.rateOverTime = intensity * maxEmissionRate;

        var main = ps.main;
        main.startSpeed = intensity * maxParticleSpeed;
        /// Apply wind influence
        if (fireSystem != null)
        {
            var forceOverLifetime = ps.forceOverLifetime;
            forceOverLifetime.enabled = true;

            Vector3 wind = fireSystem.GetWindDirection() * windForceMultiplier;
            forceOverLifetime.x = wind.x;
            forceOverLifetime.y = wind.y;
            forceOverLifetime.z = wind.z;
        }
    }
}