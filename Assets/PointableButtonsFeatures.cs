using UnityEngine;

public class PointableButtonsFeatures : MonoBehaviour
{
    public GameObject ExteriorGameObject;
    public GameObject InteriorGameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ExteriorButtonCharacteristic() {
        ExteriorGameObject.SetActive(!ExteriorGameObject.activeSelf);
        InteriorGameObject.SetActive(false);
    }

    public void InteriorButtonCharacteristic()
    {
        InteriorGameObject.SetActive(!InteriorGameObject.activeSelf);
        ExteriorGameObject.SetActive(false);
    }
}
