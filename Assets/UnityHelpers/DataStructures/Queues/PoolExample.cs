// PoolExample.cs - A usage example of the generic object pool for Unity's ParticleSystem
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This class demonstrates how to use the Pool class to efficiently manage a pool of ParticleSystem instances in a Unity project.
// It creates a pool of ParticleSystem instances, pre-populates it with a number of instances, and allows new instances to be requested
// and returned to the pool as needed. It also implements a coroutine to automatically return ParticleSystem instances to the pool
// after they have finished playing, and provides a public method for requesting a ParticleSystem to be played at a specific position
// and rotation in the game world.
//
// Usage: Attach this script to a GameObject in your Unity project, and assign a reference to a ParticleSystem prefab in the inspector.
// You can also adjust the initial and maximum size of the pool to suit your needs. Call the public PlayParticleSystem method to play
// a ParticleSystem at a specified position and rotation in your game world.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections;
using UnityEngine;

public class PoolExample : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to particle system object")] 
    private ParticleSystem particleSystemPrefab;
    [SerializeField, Tooltip("The amount for the pool to start with")] 
    private int initialPoolSize = 10;
    [SerializeField, Tooltip("Max amount for the pool to have")]
    private int maxPoolSize = 50;
    private Pool<ParticleSystem> _particleSystemPool;

    private void Awake()
    {
        // Initialize the particle system pool with the prefab and initial pool size.
        _particleSystemPool = new Pool<ParticleSystem>(particleSystemPrefab, maxPoolSize);
        // Pre-populate the pool with instances of the particle system.
        for (int i = 0; i < initialPoolSize; i++)
        {
            ParticleSystem pSystem = _particleSystemPool.Get();
            _particleSystemPool.ReturnToPool(pSystem);
        }
    }

    public void PlayParticleSystem(Vector3 position, Quaternion rotation)
    {
        // Get a particle system from the pool.
        ParticleSystem pSystem = _particleSystemPool.Get();

        // Set the position and rotation of the particle system.
        Transform pSystemTransform = pSystem.transform;
        pSystemTransform.position = position;
        pSystemTransform.rotation = rotation;

        // Play the particle system.
        pSystem.Play();

        // Return the particle system to the pool after it's done playing.
        StartCoroutine(ReturnParticleSystemToPool(pSystem));
    }

    private IEnumerator ReturnParticleSystemToPool(ParticleSystem pSystem)
    {
        // Wait for the particle system to finish playing.
        yield return new WaitForSeconds(pSystem.main.duration);

        // Stop the particle system and return it to the pool.
        pSystem.Stop();
        _particleSystemPool.ReturnToPool(pSystem);
    }
}