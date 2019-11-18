using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using UnityEngine.Networking;
using System;
using UnityEngine.AI;

public class ScoreboardController : MonoBehaviour
{
    public Camera firstPersonCamera;
    private Anchor anchor;
    private Anchor tableAnchor;
    private DetectedPlane detectedPlane;
    private float yOffset;
    private int score;
    public GameObject table;
    public GameObject cube;
    public GameObject topRight;
    public GameObject bottomRight;
    public GameObject topLeft;
    public GameObject bottomLeft;
    public GameObject cardScreen;
    public GameObject world;
    public GameObject gameplay;

    public GameObject bridge;

    public float deltaX;
    public float deltaZ;

    // in ScoreboardController.cs
    public void SetSelectedPlane(DetectedPlane detectedPlane)
    {
        this.detectedPlane = detectedPlane;
        //CreateAnchor();
        //CreateTable();
    }

    private void Toast(string message)
    {
        AndroidJavaClass toastClass =
                   new AndroidJavaClass("android.widget.Toast");

        object[] toastParams = new object[3];
        AndroidJavaClass unityActivity =
          new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        toastParams[0] =
                     unityActivity.GetStatic<AndroidJavaObject>
                               ("currentActivity");
        toastParams[1] = message;
        toastParams[2] = toastClass.GetStatic<int>
                               ("LENGTH_LONG");

        AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>
                                      ("makeText", toastParams);
        toastObject.Call("show");
    }

    //public void AdjustTable()
    //{
    //    table.transform.position = Vector3.zero;
    //    table.transform.rotation = Pose.identity.rotation;
    //}
    public void CreateTable()
    {
        Debug.Log("ORA");
        //table.SetActive(true);
        //Pose center = detectedPlane.CenterPose;
        if (anchor != null)
        {
            DestroyObject(anchor);
        }
        //tableAnchor = detectedPlane.CreateAnchor(center);
        //table.transform.position = center.position;
        //cube.transform.position = center.position;
        //table.transform.SetParent(tableAnchor.transform);
        List<Vector3> vertices = new List<Vector3>();
        detectedPlane.GetBoundaryPolygon(vertices);

        Vector3 max = new Vector3(0, 0, 0);
        Vector3 min = new Vector3(0, 0, 0);
        bool check = true;

        foreach (Vector3 item in vertices) // Loop through List with foreach
        {
            if(check)
            {
                max = min = item;
                check = false;
            }
            else
            {
                max.x = (max.x < item.x) ? (item.x) : (max.x);
                max.y = (max.y < item.y) ? (item.y) : (max.y);
                max.z = (max.z < item.z) ? (item.z) : (max.z);
                min.x = (min.x > item.x) ? (item.x) : (min.x);
                min.y = (min.y > item.y) ? (item.y) : (min.y);
                min.z = (min.z > item.z) ? (item.z) : (min.z);
            }
        }
        deltaX = (max.x - min.x) / (float)1.3 / 10;
        //float deltaZ = (max.z - min.z) / (float)1.3;
        deltaZ = deltaX;
        table.transform.localScale = new Vector3(deltaX, (float)0.05, deltaZ);
        //world.transform.localScale = new Vector3(deltaX, (float)0.05, deltaZ);
        //world.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //gameplay.transform.localScale = new Vector3(deltaX, (float)0.05, deltaZ);
        //gameplay.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        table.name = "ROADO ROLLA";

        Debug.Log("ORAORAORAORAORAORAORAORAORAORAORAORAORAORAORA");
        Debug.Log("center plane- " + JsonUtility.ToJson(detectedPlane.CenterPose.position, true));
        //Debug.Log("worldPose- " + JsonUtility.ToJson(worldPose.position, true));
        Debug.Log("ORAORAORAORAORAORAORAORAORAORAORAORAORAORAORA");

        //topRight.transform.position = new Vector3(deltaX/(float)2, 1f, -deltaZ/2);

        topRight.name = "topRight";
        topLeft.transform.position = new Vector3(-deltaX/2, 0, -deltaZ/2);
        topLeft.name = "topLeft";
        bottomRight.transform.position = new Vector3(-deltaX/2, 0, deltaZ/2);
        bottomRight.name = "bottomRight";
        bottomLeft.transform.position = new Vector3(deltaX/2, 0, deltaZ/2);
        bottomLeft.name = "bottomLeft";

        //table.transform.position += worldPose.position;
        //Vector3 targetPosition = new Vector3(firstPersonCamera.transform.position.x, table.transform.position.y, firstPersonCamera.transform.position.z);
        //table.transform.LookAt(targetPosition);

        table.transform.position = Vector3.zero;
        world.transform.position = Vector3.zero;
        gameplay.transform.position = Vector3.zero;
        table.transform.rotation = Pose.identity.rotation;
        world.transform.rotation = Pose.identity.rotation;
        gameplay.transform.rotation = Pose.identity.rotation;
        

        world.SetActive(true);
        gameplay.SetActive(true);

        //var rigidBoy = topRight.GetComponent<Rigidbody>();
        //rigidBoy.velocity = new Vector3(0,0, 0);

        var lol = topRight.GetComponent<NavMeshAgent>();
        var wot = lol.SetDestination(new Vector3(bridge.transform.position.x, topRight.transform.position.y , bridge.transform.position.z));
        Debug.Log(".......................................................");
        Debug.Log(bridge.transform.position);

#pragma warning disable 618
        NetworkServer.Spawn(table);
#pragma warning restore 618
    }

    //void CreateAnchor()
    //{
    //    // Create the position of the anchor by raycasting a point towards
    //    // the top of the screen.
    //    Vector2 pos = new Vector2(Screen.width * .5f, Screen.height * .90f);
    //    Ray ray = firstPersonCamera.ScreenPointToRay(pos);
    //    Vector3 anchorPosition = ray.GetPoint(5f);

    //    // Create the anchor at that point.
    //    if (anchor != null)
    //    {
    //        DestroyObject(anchor);
    //    }
    //    anchor = detectedPlane.CreateAnchor(
    //        new Pose(anchorPosition, Quaternion.identity));

    //    // Attach the scoreboard to the anchor.
    //    transform.position = anchorPosition;
    //    transform.SetParent(anchor.transform);

    //    // Record the y offset from the plane.
    //    yOffset = transform.position.y - detectedPlane.CenterPose.position.y;

    //    // Finally, enable the renderers.
    //    foreach (Renderer r in GetComponentsInChildren<Renderer>())
    //    {
    //        r.enabled = true;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        cardScreen.SetActive(false);
        world.SetActive(false);
        table.SetActive(false);
        gameplay.SetActive(false);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The tracking state must be FrameTrackingState.Tracking
        // in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        // If there is no plane, then return
        if (detectedPlane == null)
        {
            return;
        }

        // Check for the plane being subsumed.
        // If the plane has been subsumed switch attachment to the subsuming plane.
        while (detectedPlane.SubsumedBy != null)
        {
            detectedPlane = detectedPlane.SubsumedBy;
        }
        //// Make the scoreboard face the viewer.
        //transform.LookAt(firstPersonCamera.transform);

        //// Move the position to stay consistent with the plane.
        //transform.position = new Vector3(transform.position.x,
        //            detectedPlane.CenterPose.position.y + yOffset, transform.position.z);
    }


}
