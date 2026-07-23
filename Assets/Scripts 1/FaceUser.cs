using System.Collections.Generic;
using UnityEngine;

public class FaceUser : MonoBehaviour
{
    [Header("User (Camera / CenterEyeAnchor)")]
    [SerializeField] private Transform user;

    [Header("Reference Points")]
    [SerializeField] private List<Transform> referencePoints = new List<Transform>();

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Completion Thresholds")]
    [SerializeField] private float positionThreshold = 0.005f;
    [SerializeField] private float rotationThreshold = 0.5f;

    [Header("Car Center")]
    [SerializeField] private Transform carCenter;

    private int currentReferenceIndex = 0;
    private Transform currentReference;

    private void OnEnable()
    {
        if (user == null && Camera.main != null)
            user = Camera.main.transform;

        if (referencePoints == null || referencePoints.Count == 0)
        {
            Debug.LogError("No reference points assigned.");
            enabled = false;
            return;
        }

        currentReference = referencePoints[currentReferenceIndex];
    }

    private void Update()
    {
        if (user == null || currentReference == null)
            return;

        //--------------------------------------------------
        // ROTATE FIRST
        //--------------------------------------------------

        Vector3 currentForward = currentReference.forward;
        currentForward.y = 0f;

        Vector3 desiredForward = user.forward;
        desiredForward.y = 0f;

        if (currentForward.sqrMagnitude > 0.0001f &&
            desiredForward.sqrMagnitude > 0.0001f)
        {
            currentForward.Normalize();
            desiredForward.Normalize();

            float angle = Vector3.SignedAngle(
                currentForward,
                desiredForward,
                Vector3.up);

            float deltaAngle = Mathf.MoveTowards(
                0f,
                angle,
                rotationSpeed * Time.deltaTime);

            if (carCenter != null)
            {
                transform.RotateAround(
                    carCenter.position,
                    Vector3.up,
                    deltaAngle);
            }
            else
            {
                transform.RotateAround(
                    currentReference.position,
                    Vector3.up,
                    deltaAngle);
            }
        }

        //--------------------------------------------------
        // CHECK ROTATION
        //--------------------------------------------------

        currentForward = currentReference.forward;
        currentForward.y = 0f;
        currentForward.Normalize();

        desiredForward = user.forward;
        desiredForward.y = 0f;
        desiredForward.Normalize();

        bool rotationDone =
            Vector3.Angle(
                currentForward,
                desiredForward) < rotationThreshold;

        //--------------------------------------------------
        // MOVE ONLY AFTER ROTATION IS COMPLETE
        // (Ignore Y movement)
        //--------------------------------------------------

        if (rotationDone)
        {
            Vector3 offset = user.position - currentReference.position;
            offset.y = 0f;

            Vector3 targetPosition = transform.position + offset;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);
        }

        //--------------------------------------------------
        // FINISHED?
        //--------------------------------------------------

        Vector3 currentPos = currentReference.position;
        currentPos.y = 0f;

        Vector3 userPos = user.position;
        userPos.y = 0f;

        bool positionDone =
            rotationDone &&
            Vector3.Distance(
                currentPos,
                userPos) < positionThreshold;

        if (positionDone)
        {
            // Final snap so the reference point is exactly on the user (XZ only)
            Vector3 finalOffset = user.position - currentReference.position;
            finalOffset.y = 0f;

            transform.position += finalOffset;

            currentReferenceIndex++;

            if (currentReferenceIndex >= referencePoints.Count)
                currentReferenceIndex = 0;

            enabled = false;
        }
    }
}