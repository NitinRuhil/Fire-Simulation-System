using UnityEngine;
using System;

/// Controls the lifecycle and behavior of a fire instance
public class FireSystem : MonoBehaviour
{
    /// Possible states of the fire
    public enum FireState
    {
        Unlit,
        Igniting,
        Burning,
        Dying,
        Extinguished
    }

    [Header("Fire State")]
    [SerializeField] private FireState currentState = FireState.Unlit;

    [Header("Fire Parameters")]
    [Tooltip("Maximum fuel available for this fire instance.")]
    [SerializeField] private float maxFuel = 10f;

    [SerializeField, Tooltip("Current fuel amount (runtime)")]
    private float fuel = 10f;

    [Tooltip("Rate at which fuel is consumed per second.")]
    [SerializeField] private float burnRate = 1f;

    [Tooltip("Current fire intensity (0 to 1).")]
    [Range(0f, 1f)]
    [SerializeField] private float intensity = 0f;

    [Header("Environment")]
    [Tooltip("Wind direction affecting the fire.")]
    [SerializeField] private Vector3 windDirection = Vector3.forward;

    [Tooltip("Strength of the wind influence.")]
    [SerializeField] private float windStrength = 1f;

    public event Action OnIgnited;
    public event Action OnExtinguished;
    public event Action<float> OnIntensityChanged;
    
    /// Updates the fire state each frame
    private void Update()
    {
        UpdateFire(Time.deltaTime);
    }
    
    /// Updates the fire based on its current state and fuel level
    private void UpdateFire(float deltaTime)
    {
        if (currentState == FireState.Burning)
        {
            fuel -= burnRate * deltaTime;
            fuel = Mathf.Max(fuel, 0f);

            intensity = Mathf.Clamp01(fuel / maxFuel);
            OnIntensityChanged?.Invoke(intensity);

            if (fuel <= 0f)
            {
                Extinguish();
            }
        }
    }

    /// Ignites the fire
    [ContextMenu("Ignite Fire")]
    public void Ignite()
    {
        if (currentState == FireState.Unlit || currentState == FireState.Extinguished)
        {
            currentState = FireState.Burning;
            OnIgnited?.Invoke();
        }
    }

    /// Adds fuel to the fire
    public void AddFuel(float amount)
    {
        fuel = Mathf.Clamp(fuel + amount, 0f, maxFuel);

        if (fuel > 0f && currentState == FireState.Extinguished)
        {
            Ignite();
        }
    }
    [ContextMenu("Add Fuel")]
    private void AddFuelContextMenu()
    {
        AddFuel(5f);
    }

    /// Extinguishes the fire
    [ContextMenu("Extinguish Fire")]
    public void Extinguish()
    {
        currentState = FireState.Extinguished;
        intensity = 0f;
        OnExtinguished?.Invoke();
    }

    /// Sets the wind parameters affecting the fire
    public void SetWind(Vector3 direction, float strength)
    {
        windDirection = direction.normalized;
        windStrength = strength;
    }

    /// Gets the current fire intensity
    public float GetIntensity() => intensity;
    
    /// Gets the wind direction affecting the fire
    public Vector3 GetWindDirection() => windDirection;
}