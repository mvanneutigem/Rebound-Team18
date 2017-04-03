using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour {

    public Material metalMaterialRef;
    public Material bouncyMaterialRef;
    public Material glassMaterialRef;
    public Material woodMaterialRef;
    public enum Mat
    {
        METAL,
        RUBBER,
        GLASS,
        WOOD
    }
    public Mat material;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (material)
            {
                case Mat.METAL:
                    other.GetComponent<Renderer>().material = metalMaterialRef;
                    other.GetComponent<Magnetize>().enabled = true;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)Mat.METAL);
                    break;
                case Mat.RUBBER:
                    other.GetComponent<Renderer>().material = bouncyMaterialRef;
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)Mat.RUBBER);
                    break;
                case Mat.GLASS:
                    other.GetComponent<Renderer>().material = glassMaterialRef;
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)Mat.GLASS);
                    break;
                case Mat.WOOD:
                    other.GetComponent<Renderer>().material = woodMaterialRef;
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)Mat.WOOD);
                    break;
            }
            
        }
    }
}
