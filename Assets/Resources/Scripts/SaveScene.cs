using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScene : MonoBehaviour
{

    [SerializeField] private ClothesInfo clothesInfo = new ClothesInfo();

    public void SaveIntoJson()
    {
        clothesInfo.models = new List<Models>();
        foreach (var go in GameObject.FindGameObjectsWithTag("Model"))
        {
            string name = go.name;
            List<int> position = new List<int>() { (int)go.transform.position.x, (int)go.transform.position.y, (int)go.transform.position.z },
                rotation = new List<int>() { (int)go.transform.rotation.x, (int)go.transform.rotation.y, (int)go.transform.rotation.z },
                scale = new List<int>() { (int)go.transform.localScale.x, (int)go.transform.localScale.y, (int)go.transform.localScale.z };

            clothesInfo.models.Add(new Models(name, position, rotation, scale));
        }

        string clothes = JsonUtility.ToJson(clothesInfo);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/models.json", clothes);
        Debug.Log(Application.persistentDataPath);
        // Start is called before the first frame update
    }
}
