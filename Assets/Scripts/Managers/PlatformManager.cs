using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private ObjectPool platformPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int initialPlatforms = 5;

    [Header("Spawning Logic")]
    [SerializeField] private Vector3 platformSpawnDirection = Vector3.forward;
    [SerializeField] private float spawnTriggerDistance = 20f;

    [Header("Difficulty Settings (Player Abilities)")]
    [SerializeField] private float playerMoveSpeed = 5f;
    [SerializeField] private float playerMaxJumpForce = 10f;
    [SerializeField] private float playerGravity = 9.81f;

    [Header("Jump Challenge")]
    [Range(0, 1)]
    [SerializeField] private float minRequiredJumpPower = 0.1f;
    [Range(0, 1)]
    [SerializeField] private float maxRequiredJumpPower = 0.9f;


    [Header("Despawning")]
    [SerializeField] private float despawnDistance = 15f;
    
    private Vector3 lastPlatformPosition;
    private List<GameObject> activePlatforms = new List<GameObject>();

    private void Start()
    {
        platformSpawnDirection.Normalize();

        GameObject initialPlatform = GameObject.FindWithTag("Ground");
        if (initialPlatform != null)
        {
            lastPlatformPosition = initialPlatform.transform.position;
            activePlatforms.Add(initialPlatform);
        }
        else
        {
            Debug.LogError("PlatformManager: No initial platform with 'Ground' tag found.");
            return;
        }
        
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnNextPlatform();
        }
    }

    private void Update()
    {
        float playerProjection = Vector3.Dot(playerTransform.position, platformSpawnDirection);
        float lastPlatformProjection = Vector3.Dot(lastPlatformPosition, platformSpawnDirection);

        if (playerProjection > lastPlatformProjection - spawnTriggerDistance)
        {
            SpawnNextPlatform();
        }

        CleanupPlatforms();
    }

    private void SpawnNextPlatform()
    {
        GameObject platform = platformPool.GetObject();
        if (platform == null) return;
        
        float requiredPower = Random.Range(minRequiredJumpPower, maxRequiredJumpPower);
        
        float verticalVelocity = requiredPower * playerMaxJumpForce;
        float timeToApex = verticalVelocity / playerGravity;
        float totalFlightTime = timeToApex * 2;
        
        float horizontalDistance = playerMoveSpeed * totalFlightTime;
        float jumpPeakHeight = (verticalVelocity * verticalVelocity) / (2 * playerGravity);
        
        float targetYOffset = Random.Range(jumpPeakHeight * 0.5f, jumpPeakHeight * 0.85f);
        
        Vector3 spawnOffset = platformSpawnDirection * horizontalDistance;
        spawnOffset.y = targetYOffset;

        Vector3 newPosition = lastPlatformPosition + spawnOffset;
        
        platform.transform.position = newPosition;
        platform.transform.rotation = Quaternion.identity;
        
        lastPlatformPosition = newPosition;
        activePlatforms.Add(platform);
    }

    private void CleanupPlatforms()
    {
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = activePlatforms[i];
            
            float platformProjection = Vector3.Dot(platform.transform.position, platformSpawnDirection);
            float playerProjection = Vector3.Dot(playerTransform.position, platformSpawnDirection);

            if (playerProjection - platformProjection > despawnDistance)
            {
                platformPool.ReturnObject(platform);
                activePlatforms.RemoveAt(i);
            }
        }
    }
} 