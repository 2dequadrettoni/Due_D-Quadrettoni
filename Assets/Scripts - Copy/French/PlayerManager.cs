using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public RaycastHit cameraRay;
    public bool bHitting = false;
    public Material selectedMaterial, p1Material, p2Material;
    public bool p1isMove = false, p1isActions = false, p1isWhait = false, p1isSelected = false;
    public bool p2isMove = false, p2isActions = false, p2isWhait = false, p2isSelected = false;
    public GameObject player1, player2;
    public Camera cam;
    //public Camera camera;

    // Use this for initialization
    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        //Raycast from the camera to mouse position for made a selector
        bHitting = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out cameraRay);

        if (cameraRay.collider != null && Input.GetMouseButtonDown(0))
        {
            //Select player1 
            if (cameraRay.transform.tag == "Player1")
            {
                Debug.Log("Player1 Selected");
                p1isSelected = true;
                p2isSelected = false;
                player1.GetComponent<Renderer>().material = selectedMaterial;
                player2.GetComponent<Renderer>().material = p2Material;
            }

            //Select player2
            if (cameraRay.transform.tag == "Player2")
            {
                Debug.Log("Player2 Selected");
                p2isSelected = true;
                p1isSelected = false;
                player2.GetComponent<Renderer>().material = selectedMaterial;
                player1.GetComponent<Renderer>().material = p1Material;
            }

            //Move player1 
            if (p1isSelected == true && cameraRay.transform.tag == "Plane")
            {
                p1isMove = true;
                player1.transform.position = cameraRay.transform.position + Vector3.up * 3;
            }

            //Move player2
            if (p2isSelected == true && cameraRay.transform.tag == "Plane")
            {
                p2isMove = true;
                player2.transform.position = cameraRay.transform.position + Vector3.up * 3;               
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
           
            p1isActions = false;
            p1isWhait = false;
            p1isMove = false;
           
            p2isActions = false;
            p2isWhait = false;
            p2isMove = false;

        }
        
    }

}
