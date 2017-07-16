using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{

    public Camera camera;
    public Material selectedMaterial, defaultMaterial;
    Renderer[] plane;
    public RaycastHit hitInfo;

    // Use this for initialization
    void Start()
    {
        plane = this.GetComponentsInChildren<Renderer>();
        hitInfo = new RaycastHit();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo);
            
        foreach (Renderer mat in plane)
        {
           if (mat.material != defaultMaterial)
            {
                mat.material = defaultMaterial;
            }
        }

        if (hitInfo.collider != null && hitInfo.transform.tag == "Plane")
        {
            hitInfo.transform.gameObject.GetComponent<Renderer>().material = selectedMaterial;

        }

    }


}