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
	// url que vamos puxar as informacoes
	public GameObject[] gameObjects;
	// texto de aguarde (index 0) e instruções de tela (index 1)

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

					// aplicando rotação do Json
					gameObject.transform.localScale = new Vector3(clothes.scale[0], clothes.scale[1], clothes.scale[2]);
					// aplicando posicao do Json
					gameObject.transform.localPosition = new Vector3(clothes.position[0], clothes.position[1], clothes.position[2]);
					// aplicando rotacao do Json
					gameObject.transform.localRotation = Quaternion.Euler(new Vector3(clothes.rotation[0], clothes.rotation[1], clothes.rotation[2]));
					

					// instanciando o objeto para aparecer na cena
					var instantiated = Instantiate(gameObject);

					instantiated.AddComponent<Interaction>();
					instantiated.AddComponent<BoxCollider>().center = new Vector3(-0.01583651f, 0.3244349f, -0.1931399f);
					instantiated.GetComponent<BoxCollider>().size = new Vector3(1.031673f, 1.64887f, 0.6137202f);
					instantiated.AddComponent<DuplicateObject>();
				}
				Debug.Log("All set!!!");
				
				gameObjects[0].SetActive(false);
				gameObjects[1].SetActive(true);
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
