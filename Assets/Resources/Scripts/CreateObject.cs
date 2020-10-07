using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEditor.PackageManager;

using UnityEditorInternal;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateObject : MonoBehaviour
{
	public string URL = "https://s3-sa-east-1.amazonaws.com/static-files-prod/unity3d/models.json";
	// url que vamos puxar as informacoes
	public GameObject[] gameObjects;
	// texto de aguarde (index 0) e instruções de tela (index 1)

	public GameObject prime_gameObject, parent_gameObject; // pega a Raw Image (prime) e o Content do Scroll View (parent)
	public static GameObject model, instantiated_model, thumb_cam, thumb;
	public static int size = 0;

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
					var model = LoadModel(clothes);
					var instantiated_model = InstantiateModel(model, clothes);
					var thumb_cam = CreateThumbCam(instantiated_model);
					var thumb = ThumbList(thumb_cam, "Thumbnail" + size);
					// aumentando o "tamanho" pra ter o tracking
					size++;
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
		}
		else
		{
			Debug.Log("Unfortunately, we encountered an error: " + www.error);
		}

	}

	void Start()
	{
		StartCoroutine(getData()); // faz a co-rotina para fazer load dos modelos 
	}

	private GameObject LoadModel(Models clothes)
    {
		Debug.Log("Loading models");

		var gameObject = Resources.Load<GameObject>("Models/Cattleya/sources/clothingSet_04");

		// fazendo o load dos recursos 
		Debug.Log("Model " + clothes.name);

		// aplicando rotação do Json
		gameObject.transform.localScale = new Vector3(clothes.scale[0], clothes.scale[1], clothes.scale[2]);
		// aplicando posicao do Json
		gameObject.transform.localPosition = new Vector3(clothes.position[0], clothes.position[1], clothes.position[2]);
		// aplicando rotacao do Json
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(clothes.rotation[0], clothes.rotation[1], clothes.rotation[2]));

		return gameObject;
	}


	public GameObject InstantiateModel(GameObject gameObject, Models clothes)
	{
		// instanciando o objeto para aparecer na cena
		var instantiated = Instantiate(gameObject);

		// aplicando atributos tipo nome, layer e tag
		instantiated.name = clothes.name;
		instantiated.tag = "Model";
		instantiated.layer = 8;

		instantiated.AddComponent<Interaction>();
		instantiated.AddComponent<BoxCollider>().center = new Vector3(-0.01583651f, 0.3244349f, -0.1931399f);
		instantiated.AddComponent<HandleObject>();

		return instantiated;
	}

	public GameObject CreateThumbCam(GameObject instantiated)
	{
		// criando camera de Thumb
		var thumb = Instantiate(gameObjects[2]);
		thumb.transform.parent = instantiated.transform;
		thumb.SetActive(true);
		thumb.transform.localPosition = new Vector3(0, 0.94f, 0.51f);
		thumb.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		thumb.transform.localScale = Vector3.one;

		return thumb;
	}
	public GameObject ThumbList(GameObject thumb,  String thumbname)
	{
		// fazendo raw image
		var rawImage = Instantiate(prime_gameObject);
		rawImage.transform.localPosition = Vector3.zero;

		// criando nova textura
		var renderTexture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32);
		renderTexture.name = thumbname;
		renderTexture.Create();

		// criando novo material
		var material = new Material(Shader.Find("Unlit/Texture"));
		material.mainTexture = renderTexture;
		material.name = thumbname;

		// adicionando material e textura no raw image
		rawImage.GetComponent<RawImage>().texture = renderTexture;
		rawImage.GetComponent<RawImage>().material = material;

		// colocando parentesco na rawimage
		rawImage.transform.parent = parent_gameObject.transform;
		rawImage.transform.localScale = Vector3.one;
		rawImage.transform.localPosition = Vector3.zero;
		rawImage.SetActive(true);

		// colocando a textura na camera
		thumb.GetComponent<Camera>().targetTexture = renderTexture;

		return rawImage;
	}
}
