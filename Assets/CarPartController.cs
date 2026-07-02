using Oculus.Interaction;
using UnityEngine;

public class CarPartController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private PointableUnityEventWrapper pointableWrapper;

    [Header("Car Parts")]
    [SerializeField] private Transform leftFrontDoor;
    [SerializeField] private Transform leftRearDoor;
    [SerializeField] private Transform rightFrontDoor;
    [SerializeField] private Transform rightRearDoor;
    [SerializeField] private Transform hood;
    [SerializeField] private Transform dikki;

    [Header("Open Angles")]
    [SerializeField] private float leftDoorAngle = 70f;
    [SerializeField] private float rightDoorAngle = 70f;
    [SerializeField] private float hoodAngle = 60f;
    [SerializeField] private float dikkiAngle = 60f;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 150f; // Degrees per second

    [Header("Input")]
    [SerializeField] private OVRInput.Button tapeActionButton;

    private bool isOpen = false;

    // Closed Rotations
    private Quaternion lfClosed;
    private Quaternion lrClosed;
    private Quaternion rfClosed;
    private Quaternion rrClosed;
    private Quaternion hoodClosed;
    private Quaternion dikkiClosed;

    // Target Rotations
    private Quaternion lfTarget;
    private Quaternion lrTarget;
    private Quaternion rfTarget;
    private Quaternion rrTarget;
    private Quaternion hoodTarget;
    private Quaternion dikkiTarget;

    private void Awake()
    {
        // Store closed rotations
        lfClosed = leftFrontDoor.localRotation;
        lrClosed = leftRearDoor.localRotation;
        rfClosed = rightFrontDoor.localRotation;
        rrClosed = rightRearDoor.localRotation;
        hoodClosed = hood.localRotation;
        dikkiClosed = dikki.localRotation;

        // Initial targets are closed
        lfTarget = lfClosed;
        lrTarget = lrClosed;
        rfTarget = rfClosed;
        rrTarget = rrClosed;
        hoodTarget = hoodClosed;
        dikkiTarget = dikkiClosed;
    }

    private void Update()
    {
        // Toggle with controller button
        if (OVRInput.GetDown(tapeActionButton))
        {
            ToggleCarParts();
        }

        // Smoothly rotate every part
        leftFrontDoor.localRotation = Quaternion.RotateTowards(
            leftFrontDoor.localRotation,
            lfTarget,
            rotationSpeed * Time.deltaTime);

        leftRearDoor.localRotation = Quaternion.RotateTowards(
            leftRearDoor.localRotation,
            lrTarget,
            rotationSpeed * Time.deltaTime);

        rightFrontDoor.localRotation = Quaternion.RotateTowards(
            rightFrontDoor.localRotation,
            rfTarget,
            rotationSpeed * Time.deltaTime);

        rightRearDoor.localRotation = Quaternion.RotateTowards(
            rightRearDoor.localRotation,
            rrTarget,
            rotationSpeed * Time.deltaTime);

        hood.localRotation = Quaternion.RotateTowards(
            hood.localRotation,
            hoodTarget,
            rotationSpeed * Time.deltaTime);

        dikki.localRotation = Quaternion.RotateTowards(
            dikki.localRotation,
            dikkiTarget,
            rotationSpeed * Time.deltaTime);
    }

    private void ToggleCarParts()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            // Left Doors (+Y)
            lfTarget = lfClosed * Quaternion.Euler(0, leftDoorAngle, 0);
            lrTarget = lrClosed * Quaternion.Euler(0, leftDoorAngle, 0);

            // Right Doors (-Y)
            rfTarget = rfClosed * Quaternion.Euler(0, -rightDoorAngle, 0);
            rrTarget = rrClosed * Quaternion.Euler(0, -rightDoorAngle, 0);

            // Hood (+Z)
            hoodTarget = hoodClosed * Quaternion.Euler(0, 0, hoodAngle);

            // Dikki (-Z)
            dikkiTarget = dikkiClosed * Quaternion.Euler(0, 0, -dikkiAngle);
        }
        else
        {
            // Return to closed rotations
            lfTarget = lfClosed;
            lrTarget = lrClosed;
            rfTarget = rfClosed;
            rrTarget = rrClosed;
            hoodTarget = hoodClosed;
            dikkiTarget = dikkiClosed;
        }
    }

    private void OnEnable()
    {
        if (pointableWrapper != null)
        {
            pointableWrapper.WhenSelect.AddListener(OnPointSelected);
        }
    }

    private void OnDisable()
    {
        if (pointableWrapper != null)
        {
            pointableWrapper.WhenSelect.RemoveListener(OnPointSelected);
        }
    }

    private void OnPointSelected(PointerEvent pointerEvent)
    {
        ToggleCarParts();
    }
}