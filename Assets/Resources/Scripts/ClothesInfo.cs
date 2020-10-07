using System.Collections.Generic;


[System.Serializable]
public class Models
{
	// classe que recebe as propriedades do objeto "models" que está no JSon
	public string name;
	public List<float> position, rotation, scale;
}

[System.Serializable]
public class ClothesInfo
{
	// classe que recebe o objeto models
	public List<Models> models;
}