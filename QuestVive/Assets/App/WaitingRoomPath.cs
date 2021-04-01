using System.Collections.Generic;
using UnityEngine;


public struct TwoInt
{
    public int a;
    public int b;
}


public struct WalkingPath
{
    public Vector2 start;
    public Vector2 end;
    public int direction; // (0, 1, 2, 3): (left, right, down, up)
    public int startDirection;
    public int lastDirection;
    public int seq;
    public int lastTunnelType;
    public int startTunnelType;

}

public class WaitingRoomPath : MonoBehaviour
{
    public List<Vector2> InitialTunnelSart; 
    public List<Vector2> InitialTunnelEnd;
    public List<int> InitialTunnelStartType;
    public List<int> InitialStartDirection;

    public List<GameObject> TunnelPrefabs; // (0, 1, 2): (straight, left, right)
    public Transform WalkingPathOrigin;
    public GameObject TunnelCollider;
    public GameObject LasDoorCollider;
    public GameObject ScifiDoor;
    public GameObject WallPlane;

    public float XMax = 4;
    public List<float> XMin;
    public float YMin = 0;
    public List<float> YMax;
    List<Vector2> directionUnit;
    List<float> rotationAngle;
    
    bool haveEnterTunnel = false;
    public List<GameObject> PathObjects;
    WalkingPath lastWalkingPath;


    public void Clear()
    {
        for (int i = 0; i < WalkingPathOrigin.transform.childCount; ++i)
        {
           Destroy(WalkingPathOrigin.transform.GetChild(i).gameObject);
        }
        PathObjects.Clear();

    
    }

    void initialize()
    {
        haveEnterTunnel = false;

        // add initial path
        WalkingPath initialPath = new WalkingPath();
        initialPath.start = InitialTunnelSart[GeneralManager.instance.UserID - 2];
        initialPath.end = InitialTunnelEnd[GeneralManager.instance.UserID - 2];
        initialPath.seq = 0;
        for (int i = 0; i < directionUnit.Count; ++i)
        {
            if (Vector2.Dot(directionUnit[i], initialPath.end - initialPath.start) > 0.5f)
            {
                initialPath.direction = i;
                break;
            }
        }

        TwoInt result = checkEndTunelType(initialPath);
        initialPath.lastTunnelType = result.a;
        initialPath.lastDirection = result.b;
        initialPath.startTunnelType = InitialTunnelStartType[GeneralManager.instance.UserID - 2];
        initialPath.startDirection = InitialStartDirection[GeneralManager.instance.UserID - 2];

        GeneratePath(initialPath);
        lastWalkingPath = initialPath;
        DetermineAndGenerate();
            
    }

    int checkStartTunnelType(int lastDirection, int currentDirection)
    {

  

        Vector3 rhs = new Vector3(directionUnit[lastDirection].x, 0, directionUnit[lastDirection].y);
        Vector3 lhs = new Vector3(directionUnit[currentDirection].x, 0, directionUnit[currentDirection].y);

        float dotProduct = Vector3.Dot(Vector3.up, Vector3.Normalize(Vector3.Cross(lhs, rhs)));
        int tunnelType = 0;

        if (dotProduct > .5f)
        {
            tunnelType = 1; // left
        }
        else if (dotProduct < -.5f)
        {
            tunnelType = 2; // right
        }
        else
        {
            tunnelType = 0; // straight
        }



        return tunnelType;

    }

    TwoInt checkEndTunelType(WalkingPath newPath)
    {

        List<float> ToBoundaryDistance = new List<float>();
        // Check four directions and find the longest one
        ToBoundaryDistance.Add(newPath.end.x - XMin[(int) newPath.end.y]); // left
        ToBoundaryDistance.Add(XMax - newPath.end.x); // right
        ToBoundaryDistance.Add(newPath.end.y - YMin); // down
        ToBoundaryDistance.Add(YMax[(int)newPath.end.x] - newPath.end.y); // up

        //foreach (float distance in ToBoundaryDistance)
        //{
        //    Debug.Log($"To boundary: {distance}");
        //}

        int maxDirection = -1;
        float maxDistance = -1;
        for (int i = 0; i < 4; ++i)
        {
            if (ToBoundaryDistance[i] > maxDistance && checkNotOpposite(i, newPath.direction))
            {

                maxDistance = ToBoundaryDistance[i];
                maxDirection = i;
            }
        }


        Vector2 oldDirection = newPath.end - newPath.start;
        Vector2 newDirection = directionUnit[maxDirection];

        Vector3 lhs = new Vector3(newDirection.x, 0, newDirection.y);
        Vector3 rhs = new Vector3(oldDirection.x, 0, oldDirection.y);

        float dotProduct = Vector3.Dot(Vector3.up, Vector3.Normalize(Vector3.Cross(lhs, rhs)));
        int endTunnelType = 0;

        if (dotProduct > .5f)
        {
            endTunnelType = 1; // left
        }
        else if (dotProduct < -.5f)
        {
            endTunnelType = 2; // right
        }
        else
        {
            endTunnelType = 0; // straight
        }

        TwoInt result = new TwoInt();
        result.a = endTunnelType;
        result.b = maxDirection;

        return result;
    
    }


    bool checkNotOpposite(int currentDirection, int lastDirection)
    {
        // last left
        if (lastDirection == 0)
        {
            return currentDirection != 1;
        }
        // last right
        else if (lastDirection == 1)
        {
            return currentDirection != 0;
        }
        // last down
        else if (lastDirection == 2)
        {
            return currentDirection != 3;
        }
        // last up
        else
        {
            return currentDirection !=2;
        }
    }

    Vector2 Increment()
    {
        Vector2 increment = directionUnit[lastWalkingPath.direction];
        Quaternion quat = Quaternion.AngleAxis(rotationAngle[lastWalkingPath.lastTunnelType], Vector3.up);
        //Debug.Log($"Original last tunnel type: {lastWalkingPath.direction}");
        Vector3 inc = quat * new Vector3(increment.x, 0, increment.y);
        increment = new Vector2(inc.x, inc.z);

        // Change lastwalking path direction according to last tunnel type
        for (int i = 0; i < directionUnit.Count; ++i)
        {
            //Debug.Log(Vector2.Dot(directionUnit[i], increment));

            if (Vector2.Dot(directionUnit[i], increment) > 0.5f)
            {
                lastWalkingPath.direction = i;
                break;
            }
        }
        //Debug.Log($"Modified last tunnel type: {lastWalkingPath.direction}");


        return increment;




    }

    void DetermineAndGenerate()
    {
        Vector2 incrementVector = Increment();
        Vector2 newStart = lastWalkingPath.end + incrementVector;



        List<float> ToBoundaryDistance = new List<float>();
        // Check four directions and find the longest one
        ToBoundaryDistance.Add(newStart.x - XMin[(int)newStart.y]); // left
        ToBoundaryDistance.Add(XMax - newStart.x); // right
        ToBoundaryDistance.Add(newStart.y - YMin); // down
        ToBoundaryDistance.Add(YMax[(int)newStart.x] - newStart.y); // up

        int maxDirection = -1;
        float maxDistance = -1;
        for (int i = 0; i < 4; ++i)
        {
            if (ToBoundaryDistance[i] > maxDistance && checkNotOpposite(i, lastWalkingPath.lastDirection))
            {

                maxDistance = ToBoundaryDistance[i];
                maxDirection = i;
            }

        }
        maxDistance = Mathf.Ceil(maxDistance - 0.2f);
        float factor = Random.Range(1, (int)maxDistance);
        //float factor = (int)maxDistance;




        Vector2 newEnd = newStart + directionUnit[maxDirection] * factor;

        // Check last tunnel type
        WalkingPath newPath = new WalkingPath();
        newPath.start = newStart;
        newPath.end = newEnd;
        newPath.direction = maxDirection;
        newPath.seq = lastWalkingPath.seq + 1;
        TwoInt result = checkEndTunelType(newPath);
        newPath.lastTunnelType = result.a; // (0, 1, 2): (straight, left, right)
        newPath.lastDirection = result.b;
        newPath.startTunnelType = checkStartTunnelType(lastWalkingPath.direction,newPath.direction);



        for (int i = 0; i < directionUnit.Count; ++i)
        {
            if (Vector2.Dot(directionUnit[i], incrementVector) > 0.5f)
            {
                newPath.startDirection = i;
                break;
            }
        }

        //Debug.Log($"last direction: {lastWalkingPath.lastDirection}, max distance: {maxDistance}, max direction: {maxDirection}, factor: {factor}");


        GeneratePath(newPath);
        lastWalkingPath = newPath;
        if (PathObjects.Count > 1)
        {
            GameObject backDoor = PathObjects[PathObjects.Count - 2].transform.Find("BackDoor").gameObject;
            Destroy(backDoor);
        }

    }

    void GeneratePath(WalkingPath newPath)
    {
        // spawn the corresponding tunnel
        GameObject pathObj = new GameObject();
        PathObjects.Add(pathObj);
        pathObj.name = $"Path {newPath.seq}";
        pathObj.transform.SetParent(WalkingPathOrigin, false);
        pathObj.transform.localPosition = new Vector3(newPath.start.x, 0f, newPath.start.y);
        //Debug.Log($"end to start: {Vector2.Distance(newPath.end, newPath.start)}");

        int count = (int)Mathf.Ceil(Vector2.Distance(newPath.end, newPath.start) - 0.2f) + 1;
        //Debug.Log($"count: {count}");


        float rotationAngle = 0f;
        if (newPath.direction == 0)
        {
            rotationAngle = 270;
        }
        else if (newPath.direction == 1)
        {
            rotationAngle = 90;
        }
        else if (newPath.direction == 2)
        {
            rotationAngle = 180;
        }


        for (int i = 0; i < count; ++i)
        {
            if (i == count - 1 )
            {
                // place door at end
                GameObject door = Instantiate(ScifiDoor);
                door.name = "BackDoor";
                door.transform.RotateAround(door.transform.position, door.transform.up, rotationAngle);
                door.transform.Translate(Vector3.forward * i);
                door.transform.SetParent(pathObj.transform, false);
                door.GetComponent<TunnelDoorBehavior>().isBackDoor = true;
                Vector3 from = new Vector3(directionUnit[newPath.direction].x, 0, directionUnit[newPath.direction].y);
                Vector3 to = new Vector3(directionUnit[newPath.lastDirection].x, 0, directionUnit[newPath.lastDirection].y);
                door.transform.localRotation *= Quaternion.FromToRotation(from, to);
                door.transform.Translate(Vector3.forward * .47f);

                if (haveEnterTunnel)
                {
                    GameObject tunnelCollider = Instantiate(LasDoorCollider);
                    tunnelCollider.transform.SetParent(door.transform, false);
                    tunnelCollider.transform.Translate(Vector3.forward * .25f);

                }
                else
                {
                    GameObject wallPlane = Instantiate(WallPlane);
                    //Debug.Log(wallPlane.transform.position);
                    //Debug.Log(wallPlane.transform.rotation);

                    wallPlane.transform.SetParent(door.transform, false);
                    wallPlane.transform.Translate(-Vector3.right * .03f);
                }

                if (i == 0)
                {
                    int type = newPath.startTunnelType;

                    float angle = 0;
                    if (newPath.startDirection == 0)
                    {
                        angle = 270;
                    }
                    else if (newPath.startDirection == 1)
                    {
                        angle = 90;
                    }
                    else if (newPath.startDirection == 2)
                    {
                        angle = 180;
                    }



                    GameObject tunnelCollider = Instantiate(TunnelCollider);
                    tunnelCollider.name = newPath.seq.ToString();
                    tunnelCollider.transform.RotateAround(tunnelCollider.transform.position, tunnelCollider.transform.up, angle);
                    tunnelCollider.transform.Translate(Vector3.forward * .25f);

                    tunnelCollider.transform.SetParent(pathObj.transform, false);


                    // place door
                    GameObject frontDoor = Instantiate(ScifiDoor);
                    frontDoor.name = "FrontDoor";
                    frontDoor.transform.RotateAround(frontDoor.transform.position, frontDoor.transform.up, angle);
                    frontDoor.transform.Translate(-Vector3.forward * .5f);
                    frontDoor.transform.SetParent(pathObj.transform, false);

                    GameObject tunnel = Instantiate(TunnelPrefabs[type]);
                    //Debug.Log($"Start tunnel type {type}");
                    // rotate the tunnel according to the direction
                    tunnel.transform.RotateAround(tunnel.transform.position, tunnel.transform.up, angle);
                    tunnel.transform.Translate(Vector3.forward * i);
                    tunnel.transform.SetParent(pathObj.transform, false);
                }

                else 
                {
                    //Debug.Log($"last tunnel type: {newPath.lastTunnelType}");
                    GameObject tunnel = Instantiate(TunnelPrefabs[newPath.lastTunnelType]);
                    // rotate the tunnel according to the direction
                    tunnel.transform.RotateAround(tunnel.transform.position, tunnel.transform.up, rotationAngle);
                    tunnel.transform.Translate(Vector3.forward * i);
                    tunnel.transform.SetParent(pathObj.transform, false);

                }

            }
            else
            {
                if (i == 0)
                {
                    int type = newPath.startTunnelType;

                    float angle = 0;
                    if (newPath.startDirection == 0)
                    {
                        angle = 270;
                    }
                    else if (newPath.startDirection == 1)
                    {
                        angle = 90;
                    }
                    else if (newPath.startDirection == 2)
                    {
                        angle = 180;
                    }



                    GameObject tunnelCollider = Instantiate(TunnelCollider);
                    tunnelCollider.name = newPath.seq.ToString();
                    tunnelCollider.transform.RotateAround(tunnelCollider.transform.position, tunnelCollider.transform.up, angle);
                    tunnelCollider.transform.Translate(Vector3.forward * .25f);

                    tunnelCollider.transform.SetParent(pathObj.transform, false);


                    // place door
                    GameObject frontDoor = Instantiate(ScifiDoor);
                    frontDoor.name = "FrontDoor";
                    frontDoor.transform.RotateAround(frontDoor.transform.position, frontDoor.transform.up, angle);
                    frontDoor.transform.Translate(-Vector3.forward * .5f);
                    frontDoor.transform.SetParent(pathObj.transform, false);

                    GameObject tunnel = Instantiate(TunnelPrefabs[type]);
                    //Debug.Log($"Start tunnel type {type}");
                    // rotate the tunnel according to the direction
                    tunnel.transform.RotateAround(tunnel.transform.position, tunnel.transform.up, angle);
                    tunnel.transform.Translate(Vector3.forward * i);
                    tunnel.transform.SetParent(pathObj.transform, false);
                }



                else 
                {
                    GameObject tunnel = Instantiate(TunnelPrefabs[0]);
                    // rotate the tunnel according to the direction
                    tunnel.transform.RotateAround(tunnel.transform.position, transform.up, rotationAngle);
                    tunnel.transform.Translate(Vector3.forward * i);
                    tunnel.transform.SetParent(pathObj.transform, false);
                }


            }
        }


    }


    void DestroyFirstPath()
    {
        if (lastWalkingPath.seq < 2)
        {
            return;
        }
        GameObject lastone = PathObjects[0];
        PathObjects.RemoveAt(0);
        Destroy(lastone);
        //if (PathObjects.Count == 0)
        //{
        //    GeneralManager.instance.OnLeaveLastTunnel();
        //}
    }

    void EnterCollider()
    {
        if (!haveEnterTunnel)
        { 
            GeneralManager.instance.propManager.OtherPlayer.SetActive(true);
            haveEnterTunnel = true;
        }
        DestroyFirstPath();
        if (GeneralManager.instance.StartWalking && !GeneralManager.instance.DeviceReady)
        { 
            DetermineAndGenerate();
        }
    }

    void LeaveTunnel()
    {
        Debug.Log($"Path object count: {PathObjects.Count}");

        if (PathObjects.Count < 2)
        {
            Debug.Log("On leave last tunnel");
            DestroyFirstPath();
            GeneralManager.instance.OnLeaveLastTunnel();
        }
    }
    private void Awake()
    {
        rotationAngle = new List<float> { 0, -90, 90 };
        YMax = new List<float> { 1, 2, 2, 2 };
        XMin = new List<float> { 0, 0, 1 };

        directionUnit = new List<Vector2>();
        directionUnit.Add(new Vector2(-1, 0));
        directionUnit.Add(new Vector2(1, 0));
        directionUnit.Add(new Vector2(0, -1));
        directionUnit.Add(new Vector2(0, 1));
        PathObjects = new List<GameObject>();

    }

    // Start is called before the first frame update
    void Start()
    {

        GeneralManager.instance.OnEnterTunnelCollider += EnterCollider;
        GeneralManager.instance.OnLeaveTunnel += LeaveTunnel;
        GeneralManager.instance.OnInitialize += initialize;
    }

}
