using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour {

    public Material metalMaterialRef;
    public Material bouncyMaterialRef;
    public Material glassMaterialRef;
    public Material woodMaterialRef;

    public PhysicsPlayerController.Mat material;

    public void ChangeMaterial(PhysicsPlayerController.Mat localMaterial, GameObject other)
    {
        if (other.tag == "Player")
        {
            switch (localMaterial)
            {
                case PhysicsPlayerController.Mat.METAL:
                    other.GetComponent<PhysicsPlayerController>().SetMaterial(metalMaterialRef);
                    other.GetComponent<Magnetize>().enabled = true;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)PhysicsPlayerController.Mat.METAL);
                    break;
                case PhysicsPlayerController.Mat.RUBBER:
                    other.GetComponent<PhysicsPlayerController>().SetMaterial(bouncyMaterialRef);
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)PhysicsPlayerController.Mat.RUBBER);
                    break;
                case PhysicsPlayerController.Mat.GLASS:
                    other.GetComponent<PhysicsPlayerController>().SetMaterial(glassMaterialRef);
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)PhysicsPlayerController.Mat.GLASS);
                    break;
                case PhysicsPlayerController.Mat.WOOD:
                    other.GetComponent<PhysicsPlayerController>().SetMaterial(woodMaterialRef);
                    other.GetComponent<Magnetize>().enabled = false;
                    other.GetComponent<PhysicsPlayerController>().SetState((int)PhysicsPlayerController.Mat.WOOD);
                    break;
            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        ChangeMaterial(material, other.gameObject);
    }
}
