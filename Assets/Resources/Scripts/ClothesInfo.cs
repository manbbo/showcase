using System.Collections.Generic;


[System.Serializable]
public class Models
{
	// classe que recebe as propriedades do objeto "models" que está no JSon
	public string name;
	public List<int> position, rotation, scale;

	public Models(string name, 
		List<int> position, 
		List<int> rotation, 
		List<int> scale)
    {
		this.name = name;
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
    }
}

[System.Serializable]
public class ClothesInfo
{
	// classe que recebe o objeto models
	public List<Models> models;
}