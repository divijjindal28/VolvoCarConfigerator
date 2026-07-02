using Meta.XR.MRUtilityKit;
using UnityEngine;

public class AddPositionBalls : MonoBehaviour
{

    [SerializeField] private OVRInput.Button tapeActionButton;

    public GameObject positionBalls;
    public GameObject PositionBallsParents;
    public GameObject RemotePosition;
    void Start()
    {
    }

    void Update()
    {
        if (PositionBallsParents.transform.childCount <= 2)
        {
            if (OVRInput.GetDown(tapeActionButton))
            {
                SpawnCalibrationPoint();
            }

        }
    }

    private void SpawnCalibrationPoint()
    {
        GameObject ball = Instantiate(
            positionBalls,
            RemotePosition.transform.position,
            Quaternion.identity
        );

        ball.transform.SetParent(PositionBallsParents.transform);

        
    }

}
