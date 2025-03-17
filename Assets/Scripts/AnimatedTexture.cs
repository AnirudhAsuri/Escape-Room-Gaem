using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour
{
    public float fps = 30.0f;
    public Sprite[] frames; // Use Sprites instead of Textures

    private int frameIndex;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found!");
            return;
        }

        NextFrame();
        InvokeRepeating("NextFrame", 1f / fps, 1f / fps);
    }

    void NextFrame()
    {
        if (frames.Length == 0) return;

        spriteRenderer.sprite = frames[frameIndex];
        frameIndex = (frameIndex + 1) % frames.Length;
    }

    public void Play()
    {
        frameIndex = 0;
        CancelInvoke(nameof(NextFrame)); // Ensure no duplicate invokes
        InvokeRepeating(nameof(NextFrame), 0f, 1f / fps);
    }

    public void Stop()
    {
        CancelInvoke(nameof(NextFrame)); // Stop animation updates
        spriteRenderer.sprite = null; // Clear the last frame
        gameObject.SetActive(false); // Hide the muzzle flash
    }


}