using System.Collections;
using Meta.XR.MRUtilityKit;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MRUKLabelDebugger : MonoBehaviour
{
    [SerializeField] private OVRInput.Button tapeActionButton;

    public GameObject positionBalls;
    public GameObject PositionBallsParents;
    public GameObject RemotePosition;
  

    private NavMeshSurface navMeshSurface;
    public GameObject plane;
    private GameObject planeGameObject;
    void Start()
    {
        Debug.Log("MRUKLabelDebugger Start");
        //StartCoroutine(DebugLabels());
    }



    void Update()
    {
        if (PositionBallsParents.transform.childCount <= 1)
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
        CreatePlane(ball);
        BuildNavMesh();

    }

    IEnumerator DebugLabels()
    {
        MRUKRoom room = null;
        while (room == null)
        {
            room = MRUK.Instance.GetCurrentRoom();
            yield return null;
        }

        foreach (var anchor in room.Anchors)
        {
            Debug.Log($"Anchor: {anchor.name}");
            if (anchor.name.Contains("FLOOR")) {
                GameObject floor = anchor.gameObject;
                Debug.Log("MRUKLabelDebugger FloorFound");
                //Component[] components = floor.GetComponents<Component>();

                //foreach (Component c in components)
                //{
                //    Debug.Log("FloorFound : "+ c.GetType().Name);
                //}


                //GameObject floorChild = floor.transform.GetChild(0).gameObject;
                //Debug.Log("FloorFound CHILD");
                //Component[] childcomponents = floorChild.GetComponents<Component>();

                //foreach (Component c in childcomponents)
                //{
                //    Debug.Log("FloorFound CHILD : " + c.GetType().Name);
                //}


                //GameObject floorChildChild = floorChild.transform.GetChild(0).gameObject;
                //Debug.Log("FloorFound CHILD CHILD");
                //Component[] childchildcomponents = floorChildChild.GetComponents<Component>();

                //foreach (Component c in childchildcomponents)
                //{
                //    Debug.Log("FloorFound CHILD CHILD : " + c.GetType().Name);
                //}

                //navMeshSurface = floorChildChild.GetComponent<NavMeshSurface>();
                //navMeshSurface.collectObjects = CollectObjects.All;
                //navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
                //navMeshSurface.BuildNavMesh();
                CreatePlane(floor);
                BuildNavMesh();
            }

        }
    }

    void CreatePlane(GameObject parent)
    {
        planeGameObject = Instantiate(plane,parent.transform.position,Quaternion.identity);
        //planeGameObject.transform.SetParent(parent.transform);
    }

    void BuildNavMesh()
    {
        navMeshSurface = planeGameObject.GetComponent<NavMeshSurface>();
        navMeshSurface.collectObjects = CollectObjects.All;
        navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
        navMeshSurface.BuildNavMesh();
    }
}
