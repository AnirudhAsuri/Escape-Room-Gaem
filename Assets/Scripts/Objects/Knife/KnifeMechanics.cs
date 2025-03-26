using UnityEngine;

public class KnifeMechanics : MonoBehaviour
{
    public Vector3 rotationOffset = new Vector3(90, 90, 90);
    public Vector3 positionOffset = new Vector3(0, 0, -0.5f);

    public PickUpObjects pickUpScript;
    private Animator knifeAnimator;
    private Camera mainCamera;

    void Start()
    {
        knifeAnimator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (pickUpScript.pickedObject == transform && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        knifeAnimator.SetTrigger("Slice");
        TryRemoveDial();
    }

    private void TryRemoveDial()
    {
        RaycastHit hit;
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, 3f))
        {
            MainDialBoxMechanics dialBox = hit.collider.GetComponentInParent<MainDialBoxMechanics>();
            if (dialBox != null)
            {
                dialBox.RemoveDial();
            }
        }
    }

    public Quaternion GetRotationOffset()
    {
        return Quaternion.Euler(rotationOffset);
    }

    public Vector3 GetPositionOffset()
    {
        return positionOffset;
    }
}
