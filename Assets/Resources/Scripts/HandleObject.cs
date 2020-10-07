using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HandleObject : MonoBehaviour
{
    GameObject gObject;
    int size;

    private void FixedUpdate()
    {
        size = GameObject.FindGameObjectsWithTag("Model").Length;
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
            gObject2.name = (size >= 10) ? "modelo" + (size + 1): "modelo0" + (size + 1);
            gObject2.GetComponent<Interaction>().selected = true;
            gObject.GetComponent<Interaction>().selected = false;

            var thumb = this.GetComponent<CreateObject>().ThumbList(gObject2.transform.Find("Thumb(Clone)").gameObject, "ThumbDuplicated"+size);
            
        }
    }
}
