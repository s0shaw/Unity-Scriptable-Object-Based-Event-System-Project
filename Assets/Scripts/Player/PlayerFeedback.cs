using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] private Transform modelToAffect;
    [SerializeField] private float squashStretchAmount = 0.2f;
    [SerializeField] private float squashStretchSpeed = 10f;
    
    [SerializeField] private VisualEffect playerVisualEffect;
    [SerializeField] private string landEventName = "OnLand";
    [SerializeField] private string jumpEventName = "OnJump";

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    [SerializeField] private float soundCooldown = 0.2f;

    private Vector3 initialScale;
    private Coroutine runningCoroutine;
    private float lastLandSoundTime = -1f;
    private float lastJumpSoundTime = -1f;

    private AudioManager audioManager;

    private void Awake()
    {
        if (modelToAffect == null)
        {
            modelToAffect = this.transform;
        }
        initialScale = modelToAffect.localScale;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayLandEffect()
    {
        if (playerVisualEffect != null)
        {
            playerVisualEffect.SendEvent(landEventName);
        }

        if (landSound != null && Time.time >= lastLandSoundTime + soundCooldown)
        {
            lastLandSoundTime = Time.time;
            audioManager.PlaySound(landSound);
        }

        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(SquashAndStretch(initialScale.y * (1 - squashStretchAmount), initialScale.y * (1 + squashStretchAmount)));
    }
    
    public void PlayJumpEffect()
    {
        if (playerVisualEffect != null)
        {
            playerVisualEffect.SendEvent(jumpEventName);
        }

        if (jumpSound != null && Time.time >= lastJumpSoundTime + soundCooldown)
        {
            lastJumpSoundTime = Time.time;
            audioManager.PlaySound(jumpSound);
        }
        
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(SquashAndStretch(initialScale.y * (1 + squashStretchAmount), initialScale.y * (1 - squashStretchAmount)));
    }

    private IEnumerator SquashAndStretch(float startY, float endY)
    {
        float currentY = startY;
        
        while (Mathf.Abs(currentY - endY) > 0.01f)
        {
            currentY = Mathf.Lerp(currentY, endY, Time.deltaTime * squashStretchSpeed);
            modelToAffect.localScale = new Vector3(initialScale.x, currentY, initialScale.z);
            yield return null;
        }
        
        currentY = endY;
        while (Mathf.Abs(currentY - initialScale.y) > 0.01f)
        {
            currentY = Mathf.Lerp(currentY, initialScale.y, Time.deltaTime * squashStretchSpeed);
            modelToAffect.localScale = new Vector3(initialScale.x, currentY, initialScale.z);
            yield return null;
        }
        
        modelToAffect.localScale = initialScale;
    }
} 