using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickDetector : MonoBehaviour
{
    public GameObject spawnThis1;
    public GameObject spawnThis2;
    public GameObject spawnThis3;
    public GameObject spawnThis4;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    public Camera m_firstPersonCamera;
    public void Spawn(GameObject spawnThis)
    {
        /*TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            Instantiate(spawnThis, hit.Pose.position, hit.Pose.rotation);
            Debug.Log("#########################################");
            Debug.Log(JsonUtility.ToJson(hit.Pose.position, true));
            Debug.Log("#########################################");
        }*/
        Ray raycast = m_firstPersonCamera.ScreenPointToRay(new Vector3( Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit1;
        if (Physics.Raycast(raycast, out hit1))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit1.distance, Color.yellow);
            Debug.Log("Did Hit");
            Debug.Log("#########################################");
            Debug.Log(JsonUtility.ToJson(hit1.point, true));
            Debug.Log(JsonUtility.ToJson(hit1.collider.gameObject.name, true));
            Debug.Log("#########################################");
            Instantiate(spawnThis, hit1.point, Quaternion.identity);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        /*
        Ray raycast = m_firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Instantiate(spawnThis, raycastHit.transform.position, raycastHit.transform.rotation);
            Debug.Log("#########################################");
            Debug.Log(JsonUtility.ToJson(raycastHit.transform.position, true));
            Debug.Log(JsonUtility.ToJson(raycast.origin, true));
            Debug.Log("#########################################");
        }
        else
        {
            Debug.LogError("No hit detected");
        }*/

        /*
        Ray ray = Camera.main.ScreenPointToRay();
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo)) //If collision
        {
            //Vector3 pt = hit.Pose.position;
            //Set the Y to the Y of the snakeInstance
            Instantiate(spawnThis, hitInfo.transform.position, hitInfo.transform.rotation);
            Debug.Log("#########################################");
            Debug.Log(JsonUtility.ToJson(hitInfo.transform.position, true));
            Debug.Log(JsonUtility.ToJson(ray.origin, true));
            Debug.Log("#########################################");
        }*/
    }

    public void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == button1)
        {
            //Your code for button 1
            Debug.Log("Clicked: " + button1.name);
            Spawn(spawnThis1);
        }

        else if (buttonPressed == button2)
        {
            //Your code for button 2
            Debug.Log("Clicked: " + button2.name);
            Spawn(spawnThis2);

        }

        else if (buttonPressed == button3)
        {
            //Your code for button 3
            Debug.Log("Clicked: " + button3.name);
            Spawn(spawnThis3);

        }
        else if (buttonPressed == button4)
        {
            //Your code for button 3
            Debug.Log("Clicked: " + button3.name);
            Spawn(spawnThis4);
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //Register Button Events
        button1.onClick.AddListener(() => buttonCallBack(button1));
        button2.onClick.AddListener(() => buttonCallBack(button2));
        button3.onClick.AddListener(() => buttonCallBack(button3));

    }


    // Update is called once per frame
    void Update()
    {

    }
}
