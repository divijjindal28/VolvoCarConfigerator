using System.Collections.Generic;
using UnityEngine;

public class ColorSequence : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Renderer target;

    [Header("Hex Colors")]
    [SerializeField] private List<string> hexColors = new List<string>();

    private Material targetMaterial;

    // Each object remembers its own current color
    private int currentColorIndex = -1;

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Target Renderer is not assigned.");
            enabled = false;
            return;
        }

        // Creates an instance of the material for this renderer
        targetMaterial = target.sharedMaterial;
    }

    private void OnEnable()
    {
        if (targetMaterial == null)
            return;

        if (hexColors == null || hexColors.Count == 0)
        {
            Debug.LogWarning("No hex colors assigned.");
            enabled = false;
            return;
        }

        // Move to next color
        currentColorIndex = (currentColorIndex + 1) % hexColors.Count;

        // Apply color
        if (ColorUtility.TryParseHtmlString(hexColors[currentColorIndex], out Color newColor))
        {
            targetMaterial.color = newColor;
        }
        else
        {
            Debug.LogWarning($"Invalid hex color: {hexColors[currentColorIndex]}");
        }

        // Optional: disable immediately.
        // If VR Builder disables this component itself,
        // you can remove this line.
        enabled = false;
    }

    private void OnDestroy()
    {
        if (targetMaterial != null)
        {
            Destroy(targetMaterial);
        }
    }
}