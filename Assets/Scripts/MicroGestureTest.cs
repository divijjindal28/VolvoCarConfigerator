using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroGestureTest : MonoBehaviour
{
    [SerializeField]
    private OVRMicrogestureEventSource leftGestureSource;

    [SerializeField]
    private OVRMicrogestureEventSource rightGestureSource;

    private bool isTeleportationON = false;

    [SerializeField]
    private GameObject teleportationInteractor;

    [Header("Gesture Labels")]
    [SerializeField]
    private Text leftGestureLabel;

    [SerializeField]
    private Text rightGestureLabel;

    [SerializeField]
    private float gestureShowDuration = 1.5f;

    [Header("Navigation Icons Left")]
    [SerializeField]
    private Image leftArrowL;

    [SerializeField]
    private Image rightArrowL;

    [SerializeField]
    private Image upArrowL;

    [SerializeField]
    private Image downArrowL;

    [SerializeField]
    private Image selectIconL;

    [Header("Navigation Icons Right")]
    [SerializeField]
    private Image leftArrowR;

    [SerializeField]
    private Image rightArrowR;

    [SerializeField]
    private Image upArrowR;

    [SerializeField]
    private Image downArrowR;

    [SerializeField]
    private Image selectIconR;

    [Header("Colors")]
    [SerializeField]
    private Color initialColor = Color.white;

    [SerializeField]
    private Color highlightColor = Color.blue;

    [SerializeField]
    private float highlightDuration = 1f;

    [SerializeField]
    private GameObject cubeInstantiatePosition;

    [SerializeField]
    private GameObject cube;

    private GameObject spawnedCube;

    [SerializeField]
    private float rotationStep = 45f;

    [SerializeField]
    private float rotationDuration = 0.25f;

    private Coroutine rotateCoroutine;

    public GameObject CarrefPoint;

    void Start()
    {
        leftGestureSource.GestureRecognizedEvent.AddListener(gesture => OnGestureRecognized(OVRPlugin.Hand.HandLeft, gesture));
        rightGestureSource.GestureRecognizedEvent.AddListener(gesture => OnGestureRecognized(OVRPlugin.Hand.HandRight, gesture));
    }

    private void RotateCube(float angle)
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateCubeCoroutine(angle));
    }

    private IEnumerator RotateCubeCoroutine(float angle)
    {
        if (cube == null || CarrefPoint == null)
        {
            rotateCoroutine = null;
            yield break;
        }

        float rotated = 0f;

        while (rotated < Mathf.Abs(angle))
        {
            float step = (Mathf.Abs(angle) / rotationDuration) * Time.deltaTime;
            step = Mathf.Min(step, Mathf.Abs(angle) - rotated);

            float signedStep = Mathf.Sign(angle) * step;

            cube.transform.RotateAround(
                CarrefPoint.transform.position,
                Vector3.up,
                signedStep);

            rotated += step;
            yield return null;
        }

        rotateCoroutine = null;
    }

    private void HighlightGesture(OVRPlugin.Hand hand, OVRHand.MicrogestureType gesture)
    {
        switch (gesture)
        {
            case OVRHand.MicrogestureType.SwipeLeft:
                Debug.Log("MicroGestureTest : SWIPE LEFT");
                if (cube != null)
                    RotateCube(rotationStep);
                break;

            case OVRHand.MicrogestureType.SwipeRight:
                Debug.Log("MicroGestureTest : SWIPE RIGHT");
                if (cube != null)
                    RotateCube(-rotationStep);
                break;

            case OVRHand.MicrogestureType.SwipeForward:
                break;

            case OVRHand.MicrogestureType.SwipeBackward:
                break;

            case OVRHand.MicrogestureType.ThumbTap:
                Debug.Log("MicroGestureTest : THUMB TAP : " + CarrefPoint.transform.position);

                if (!isTeleportationON)
                {
                    isTeleportationON = true;
                    break;
                }

                if (cube != null && isTeleportationON)
                {
                    Vector3 offset = cube.transform.position - CarrefPoint.transform.position;
                    cube.transform.position = cubeInstantiatePosition.transform.position + offset;
                    cube.transform.rotation = CarrefPoint.transform.rotation;
                    cube.SetActive(true);
                    teleportationInteractor.SetActive(false);
                }

                //if (spawnedCube == null && isTeleportationON)
                //    spawnedCube = Instantiate(cube, CarrefPoint.transform.position, CarrefPoint.transform.rotation);
                //else
                //{
                //    spawnedCube.transform.position = CarrefPoint.transform.position;
                //    spawnedCube.transform.rotation = CarrefPoint.transform.rotation;
                //}

                break;
        }
    }

    private void OnGestureRecognized(OVRPlugin.Hand hand, OVRHand.MicrogestureType gesture)
    {
        HighlightGesture(hand, gesture);
    }
}