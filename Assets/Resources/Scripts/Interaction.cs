using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Interaction : MonoBehaviour
{
    public bool selected; // ele fala se o objeto pode ser selecionado para interação ou não
    Vector2 firstMousePos; // pega a primeira posição do mouse quando entra no objeto

    private void Awake()
    {
        selected = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (selected)
        {
            movement();
            rotate();
            resize();
        }
    }

    private void movement()
    {
        // Movimentação
        var direction = Vector3.zero;
        direction.x -= Input.GetAxis("Horizontal");
        direction.y += Input.GetAxis("UpDown");
        direction.z -= Input.GetAxis("Vertical");

        this.transform.Translate(direction * Time.deltaTime);
    }

    private void rotate()
    {
        // Rotação
        var rotation = Vector3.zero;
        rotation.y -= Input.GetAxis("Rotate");

        this.transform.Rotate(rotation * Time.deltaTime * 20);
    }

    private void resize()
    {
        // Escala
        this.transform.localScale += new Vector3(Input.GetAxis("Rescale") * Time.deltaTime,
                Input.GetAxis("Rescale") * Time.deltaTime,
                Input.GetAxis("Rescale") * Time.deltaTime) * 20;
    }

    private void OnMouseDown()
    {
        // ele verifica se o mouse passou pelo objeto. Se sim, ele faz as coisas
        selected = true;
        firstMousePos = Input.mousePosition;
        Debug.Log("Selected: " + selected);
        
        var gos = GameObject.FindGameObjectsWithTag("Model"); 
        if (gos.Length > 1)
        {
            foreach (var go in gos)
            {
                if (go != this.gameObject)
                    go.GetComponent<Interaction>().selected = false;
            }
        }
    }
}
