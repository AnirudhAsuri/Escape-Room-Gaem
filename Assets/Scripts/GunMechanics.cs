using UnityEngine;

public class GunMechanics : MonoBehaviour
{
    public float fireRate = 0.3f;
    public int maxAmmo = 6;
    private int currentAmmo;
    private bool canShoot = true;

    public float damage = 10f;
    public float impactForce; // Force applied to hit objects
    public LayerMask hitMask; // Layers the gun interacts with

    public GameObject verticalFlashObject;  // Assign Fire Point Vertical GameObject
    private SpriteRenderer verticalFlashRenderer;

    public float flashDuration = 0.05f; // Adjust as needed

    private void Start()
    {
        currentAmmo = maxAmmo;

        // Get SpriteRenderer component from the assigned GameObject
        if (verticalFlashObject != null)
            verticalFlashRenderer = verticalFlashObject.GetComponent<SpriteRenderer>();

        if (verticalFlashRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on the muzzle flash object!");
        }

        // Ensure muzzle flash is initially hidden
        if (verticalFlashRenderer != null)
        {
            verticalFlashRenderer.enabled = false; // Ensure muzzle flash is not visible at start
        }
    }

    public void Shoot()
    {
        if (!canShoot)
        {
            Debug.Log("Can't shoot yet! Wait for fire rate cooldown.");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Reload needed.");
            return;
        }

        Debug.Log("Shot Fired! Ammo left: " + currentAmmo);

        // Only play muzzle flash if the gun is picked up and the component is valid
        if (verticalFlashRenderer != null && gameObject.activeInHierarchy)
        {
            verticalFlashRenderer.enabled = true; // Enable muzzle flash sprite
            Invoke(nameof(HideMuzzleFlash), flashDuration); // Hide the muzzle flash after the duration
        }

        // Raycast logic for detecting hits
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask)) // Added hitMask to filter raycast hits
        {
            Debug.Log("Hit: " + hit.collider.name);

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = hit.point - transform.position;
                forceDirection.Normalize();
                rb.AddForce(forceDirection * impactForce, ForceMode.Impulse);
            }
        }

        // Decrease ammo and reset shooting availability
        currentAmmo--;
        canShoot = false;
        Invoke(nameof(ResetShot), fireRate); // Reset shot availability after cooldown
    }

    private void ResetShot()
    {
        canShoot = true; // Allow the next shot
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Reloaded!");
    }

    private void HideMuzzleFlash()
    {
        if (verticalFlashRenderer != null)
        {
            verticalFlashRenderer.enabled = false; // Disable the muzzle flash sprite after flash duration
        }
    }
}