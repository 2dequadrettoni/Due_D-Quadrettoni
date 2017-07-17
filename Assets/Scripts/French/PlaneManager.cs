using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{

    private Camera cam;
    public Material selectedMaterial, defaultMaterial;
    Renderer[] plane;
    public RaycastHit hitInfo;
    public PlayerManager playerManager;

    // Use this for initialization
    void Start()
    {
        plane = this.GetComponentsInChildren<Renderer>();
        hitInfo = new RaycastHit();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
            
        foreach (Renderer mat in plane)
        {
           if (mat.material != defaultMaterial)
            {
                mat.material = defaultMaterial;
            }
        }


        RaycastHit hit = playerManager.cameraRay;
        if ( playerManager.bHitting && hit.collider != null && hit.transform.tag == "Plane")
        {
            hit.transform.gameObject.GetComponent<Renderer>().material = selectedMaterial;

        }

    }


}