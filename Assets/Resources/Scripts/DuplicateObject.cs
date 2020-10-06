using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateObject : MonoBehaviour
{
    GameObject gObject;
    
    private void FixedUpdate()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Model"))
        {
            if (go.GetComponent<Interaction>().selected)
                gObject = go;
        }
    }

    public void Duplicate()
    {
        if (gObject != null)
        {
            var gObject2 = Instantiate(this.gObject, this.gObject.transform.position + new Vector3(1, 0, 0), this.gObject.transform.rotation);
            gObject2.GetComponent<Interaction>().selected = true;
            gObject.GetComponent<Interaction>().selected = false;
        }
    }
}
