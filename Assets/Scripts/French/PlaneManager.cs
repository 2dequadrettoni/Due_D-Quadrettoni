using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{

    public Camera camera;
    public Material selectedMaterial, defaultMaterial;
    Renderer[] plane;

    // Use this for initialization
    void Start()
    {
        plane = this.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Renderer mat in plane)
        {
            mat.material = defaultMaterial;
        }

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Plane")
        {
            // Destroy(hitInfo.transform.gameObject);
            hitInfo.transform.gameObject.GetComponent<Renderer>().material = selectedMaterial;

        }


    }

}