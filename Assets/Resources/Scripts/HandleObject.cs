using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HandleObject : MonoBehaviour
{
    GameObject gObject, lategObject;
    bool canDisplayTexColor;
    public GameObject colorContainer, textureContainer, prime_gameObject;
    int size;

    private void Start()
    {
        canDisplayTexColor = false;
    }

    private void FixedUpdate()
    {
        size = GameObject.FindGameObjectsWithTag("Model").Length;
        foreach (var go in GameObject.FindGameObjectsWithTag("Model"))
        {
            if (go.GetComponent<Interaction>().selected && lategObject != go)
            {
                gObject =  go;
                canDisplayTexColor = true;
            }
        }

        if (canDisplayTexColor)
            DisplayColorAndTexture();
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

    void DisplayColorAndTexture()
    {
        if (gObject != lategObject && gObject.GetComponent<Interaction>().selected)
        {
            Color[] colors = { Color.green, Color.red, Color.blue };
            Texture2D[] textures = {
                Resources.Load<Texture2D>("Models/Cattleya/sources/clothingSet_04_tex"),
                Resources.Load<Texture2D>("Models/Cattleya/sources/girl_texture_01"),
                Resources.Load<Texture2D>("Models/Cattleya/sources/hair1")
            };

            //texAndColorContainer
            for (int i = 0; i < 3; i++)
            {
                if (prime_gameObject != null && GameObject.FindGameObjectsWithTag("Color"+i).Length == 0)
                {
                    var colorRawImage = Instantiate(prime_gameObject);
                    // Color
                    colorRawImage.GetComponent<RawImage>().color = colors[i];
                    colorRawImage.AddComponent<Button>().onClick.AddListener(delegate { ChangeColor(colorRawImage.GetComponent<RawImage>().color); });
                    colorRawImage.transform.parent = colorContainer.transform;

                    var textureRawImage = Instantiate(prime_gameObject);
                    // Texture
                    textureRawImage.GetComponent<RawImage>().texture = textures[i];
                    textureRawImage.AddComponent<Button>().onClick.AddListener(delegate { ChangeTexture(textureRawImage.GetComponent<RawImage>().texture); });
                    textureRawImage.transform.parent = textureContainer.transform;


                    textureRawImage.SetActive(true);
                    colorRawImage.SetActive(true);

                    colorRawImage.tag = "Color"+i;
                    textureRawImage.tag = "Texture"+i;
                } else if (prime_gameObject!=null && GameObject.FindGameObjectsWithTag("Color"+i).Length!= 0 && i < 3)
                {
                    GameObject.FindGameObjectsWithTag("Color"+i)[0].GetComponent<RawImage>().color = colors[i];
                    GameObject.FindGameObjectsWithTag("Texture"+i)[0].GetComponent<RawImage>().texture = textures[i];
                }
            }

            canDisplayTexColor = false;
            lategObject = gObject;
        }
    }

    void ChangeTexture(Texture t)
    {
        gObject.transform.Find("clothingSet_04_body").gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = 
            t;

        Debug.Log(t);
    }
    
    void ChangeColor(Color c)
    {
        gObject.transform.Find("clothingSet_04_body").gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].color =
            c;
    }
}
