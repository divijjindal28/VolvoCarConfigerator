
using System.Collections.Generic;
using UnityEngine;

public class InteriorChangeSalesPitch : MonoBehaviour
{
    [Header("Interiors")]
    [SerializeField] private List<GameObject> interiors = new List<GameObject>();

    // Keeps track of the currently active interior
    private int currentInteriorIndex = -1;

    private void Awake()
    {
        if (interiors == null || interiors.Count == 0)
        {
            Debug.LogError("No interiors assigned.");
            enabled = false;
            return;
        }

        // Check if one of the interiors is already active
        for (int i = 0; i < interiors.Count; i++)
        {
            if (interiors[i] != null && interiors[i].activeSelf)
            {
                currentInteriorIndex = i;
                break;
            }
        }

        // If none are active, activate the first one
        if (currentInteriorIndex == -1)
        {
            currentInteriorIndex = 0;

            for (int i = 0; i < interiors.Count; i++)
            {
                if (interiors[i] != null)
                    interiors[i].SetActive(i == currentInteriorIndex);
            }
        }
    }

    private void OnEnable()
    {
        if (interiors == null || interiors.Count == 0)
        {
            enabled = false;
            return;
        }

        // Move to the next interior
        currentInteriorIndex = (currentInteriorIndex + 1) % interiors.Count;

        // Activate only the selected interior
        for (int i = 0; i < interiors.Count; i++)
        {
            if (interiors[i] != null)
                interiors[i].SetActive(i == currentInteriorIndex);
        }

        // Disable this component until VR Builder enables it again
        enabled = false;
    }
}