using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.PackageManager;

using UnityEngine;
using UnityEngine.Networking;

public class CreateObject : MonoBehaviour
{
	public string URL = "https://s3-sa-east-1.amazonaws.com/static-files-prod/unity3d/models.json";
	private ClothesInfo clothes;
	// url que vamos puxar as informacoes

	IEnumerator getData()
    {
		Debug.Log("Processing data...\n\n\n");
		UnityWebRequest www = UnityWebRequest.Get(URL);
		
		yield return www.SendWebRequest();
		Debug.Log("Done.");
		if (www.error == null)
        {
			try
			{
				Debug.Log("Loading information...");
				byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(www.downloadHandler.text);
				// transformando os dados em Bytes
				string jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes, 3, jsonBytes.Length - 3);
				// transformando os bytes em String removendo os 3 bytes extras

				var clothesInfo = JsonUtility.FromJson<ClothesInfo>(jsonString);
				// transformando os dados do JSON


				foreach (var clothes in clothesInfo.models)
				{
					Debug.Log("Loading models");

					var gameObject = Resources.Load<GameObject>("Models/Cattleya/sources/clothingSet_04");

					// aplicando atributos tipo nome e tag
					gameObject.name = clothes.name;
					gameObject.tag = "Model";

					// fazendo o load dos recursos 
					Debug.Log("Model " + clothes.name);
					gameObject.transform.localScale = new Vector3(clothes.scale[0], clothes.scale[1], clothes.scale[2]);
					// aplicando rotação do Json
					gameObject.transform.localPosition = new Vector3(clothes.position[0], clothes.position[1], clothes.position[2]);
					// aplicando posicao do Json
					gameObject.transform.localRotation = Quaternion.Euler(new Vector3(clothes.rotation[0], clothes.rotation[1], clothes.rotation[2]));
					// aplicando rotacao do Json

					gameObject.AddComponent<Interaction>();

					Instantiate(gameObject);
					// instanciando o objeto para aparecer na cena
				}
				Debug.Log("All set!!!");
			}
			catch (Exception e)
			{
				Debug.Log("Couldn't find any model");
				Debug.LogException(e);
			}
	} else
        {
			Debug.Log("Unfortunately, we encountered an error: " + www.error);
        }
    }

    void Start()
	{
		StartCoroutine(getData()); // faz a co-rotina para fazer load dos modelos
	}

}
