using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [Header("Rotation Settings")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Y;
    [SerializeField] private float rotationDegrees = 45f;
    [SerializeField] private float rotationDuration = 1f;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private float elapsedTime;
    private bool isOpened = false;

    private void Awake()
    {
        // Store the initial rotation as the "closed" state
        closedRotation = transform.localRotation;

        Vector3 axis = Vector3.up;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;

            case RotationAxis.Y:
                axis = Vector3.up;
                break;

            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
        }

        // Calculate the "opened" rotation once
        openedRotation = closedRotation * Quaternion.AngleAxis(rotationDegrees, axis);
    }

    private void OnEnable()
    {
        elapsedTime = 0f;

        startRotation = transform.localRotation;

        // Toggle between open and closed
        targetRotation = isOpened ? closedRotation : openedRotation;

        isOpened = !isOpened;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / rotationDuration);

        transform.localRotation = Quaternion.Slerp(
            startRotation,
            targetRotation,
            t);

        if (t >= 1f)
        {
            transform.localRotation = targetRotation;
            enabled = false;
        }
    }
}