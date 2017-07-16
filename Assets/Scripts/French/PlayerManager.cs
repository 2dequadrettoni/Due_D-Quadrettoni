using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    RaycastHit cameraRay;
    public Material selectedMaterial, p1Material, p2Material;
    bool p1isMove, p1isActions, p1isWhait, p1isSelected;
    bool p2isMove, p2isActions, p2isWhait, p2isSelected;
    public GameObject player1, player2;
    //public Camera camera;

    // Use this for initialization
    void Start()
    {
        p1isMove = GetComponent<Player1>().isMove;
        p1isActions = GetComponent<Player1>().isActions;
        p1isWhait = GetComponent<Player1>().isWhait;
        p1isSelected = GetComponent<Player1>().isSelected;
        p2isMove = GetComponent<Player2>().isMove;
        p2isActions = GetComponent<Player2>().isActions;
        p2isWhait = GetComponent<Player2>().isWhait;
        p2isSelected = GetComponent<Player2>().isSelected;
    }

    // Update is called once per frame
    void Update()
    {
        
        Physics.Raycast(gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out cameraRay);

        if (cameraRay.collider != null && Input.GetMouseButtonDown(0)  && cameraRay.transform.tag == "Player1")
        {
            p1isSelected = true;
            p2isSelected = false;
            player1.GetComponent<Renderer>().material = selectedMaterial;
            player2.GetComponent<Renderer>().material = p2Material;
        }

        //else

        if (cameraRay.collider != null && Input.GetMouseButtonDown(0) && cameraRay.transform.tag == "Player2")
        {
            p2isSelected = true;
            p1isSelected = false;
            player2.GetComponent<Renderer>().material = selectedMaterial;
            player1.GetComponent<Renderer>().material = p1Material;
        }

        //else

        if (cameraRay.collider != null && p1isSelected == true && Input.GetMouseButtonDown(0) && cameraRay.transform.tag == "Plane")
        {
            player1.transform.position = cameraRay.transform.position;          
        }

        if (cameraRay.collider != null && p2isSelected == true && Input.GetMouseButtonDown(0) && cameraRay.transform.tag == "Plane")
        {
            player2.transform.position = cameraRay.transform.position;
        }
    }
}
