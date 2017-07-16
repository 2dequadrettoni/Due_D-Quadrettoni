using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    bool isMove = false, isActions = false, isWhait = false;
    bool isSelected;
    RaycastHit cameraRay;
    public Material selectedMaterial;
    public Camera camera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out cameraRay);

        if (cameraRay.collider != null && Input.GetMouseButtonDown(0)  && cameraRay.transform.tag == "Player")
        {
            isSelected = true;
            this.GetComponent<Renderer>().material = selectedMaterial;
        }

        else

        if (cameraRay.collider != null && isSelected == true && Input.GetMouseButtonDown(0) && cameraRay.transform.tag == "Plane")
        {
            isActions = false;
            isWhait = false;
            isMove = true;
            transform.position = cameraRay.transform.position;
        }
    }
}
