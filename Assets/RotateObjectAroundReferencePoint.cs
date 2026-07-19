using UnityEngine;

public class RotateObjectAroundReferencePoint : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private Transform referencePoint;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private void Update()
    {
        if (objectToRotate == null || referencePoint == null)
            return;

        objectToRotate.RotateAround(
            referencePoint.position,
            rotationAxis.normalized,
            rotationSpeed * Time.deltaTime);
    }
}