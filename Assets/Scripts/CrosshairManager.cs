using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Canvas canvas; // Assign the UI Canvas in the Inspector
    public PickUpObjects pickUpScript; // Reference to the pick-up script

    private Image verticalLine;
    private Image horizontalLine;
    private Color defaultColor = Color.black;
    private Color highlightColor = Color.red;

    public LayerMask interactactable;

    void Start()
    {
        GenerateCrosshair();
    }

    void Update()
    {
        UpdateCrosshairColor();
    }

    void GenerateCrosshair()
    {
        // Create Crosshair Parent
        GameObject crosshair = new GameObject("Crosshair");
        crosshair.transform.SetParent(canvas.transform, false);
        RectTransform crosshairRect = crosshair.AddComponent<RectTransform>();
        crosshairRect.anchoredPosition = Vector2.zero;

        // Create Vertical Line
        verticalLine = CreateLine("VerticalLine", crosshair.transform, new Vector2(2, 20));

        // Create Horizontal Line
        horizontalLine = CreateLine("HorizontalLine", crosshair.transform, new Vector2(20, 2));

        // Default color
        SetCrosshairColor(defaultColor);
    }

    Image CreateLine(string name, Transform parent, Vector2 size)
    {
        GameObject line = new GameObject(name);
        line.transform.SetParent(parent, false);

        Image lineImage = line.AddComponent<Image>();
        lineImage.color = defaultColor;

        RectTransform rect = line.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = Vector2.zero;

        return lineImage;
    }

    void UpdateCrosshairColor()
    {
        if (pickUpScript == null) return;

        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        // Combine layers into a single LayerMask
        LayerMask combinedLayerMask = pickUpScript.grabableLayer | interactactable;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, pickUpScript.pickUpRange, combinedLayerMask))
        {
            // Check if it's a grabbable object
            if (((1 << hit.collider.gameObject.layer) & pickUpScript.grabableLayer) != 0)
            {
                if (hit.rigidbody != null && hit.rigidbody.mass < 40f && pickUpScript.pickedObject == null)
                {
                    SetCrosshairColor(highlightColor); // Change to red
                    return;
                }
            }

            // Check if it's an interactable object
            if (((1 << hit.collider.gameObject.layer) & interactactable) != 0)
            {
                SetCrosshairColor(highlightColor);
                return;
            }
        }

        // Default color when not aiming at anything important
        SetCrosshairColor(defaultColor);
    }

    void SetCrosshairColor(Color color)
    {
        verticalLine.color = color;
        horizontalLine.color = color;
    }
}