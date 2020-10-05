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
            // logica do teclado e mouse
        }
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

    private void OnMouseOver()
    {
        if (Input.GetButton("Fire1")) // direito do mouse
        {
            // Movimentação
            Vector3 direction = Vector3.zero;

            if (Input.mousePosition.x < firstMousePos.x)
                direction.x = 1;
            else if (Input.mousePosition.x > firstMousePos.x)
                direction.x = -1;

            if (Input.mousePosition.y < firstMousePos.y)
                direction.z = 1;
            else if (Input.mousePosition.y > firstMousePos.y)
                direction.z = -1;

            this.transform.localPosition += direction * Time.deltaTime;
        }
        else if (Input.GetButton("Fire2")) // esquerdo do mouse
        {
            // Rotação
            Vector3 rotation = Vector3.zero;

            if (Input.mousePosition.x < firstMousePos.x)
                rotation.y -= 0.5f;
            else if (Input.mousePosition.x > firstMousePos.x)
                rotation.y += 0.5f;

            this.transform.Rotate(rotation);

            // Escala
            Vector3 scaling = Vector3.zero;

            if (Input.mousePosition.y < firstMousePos.y)
                scaling = (this.transform.localScale.x >= 1) ? new Vector3(-1, -1, -1) * Time.deltaTime : Vector3.zero;
            else if (Input.mousePosition.y > firstMousePos.y)
                scaling = (this.transform.localScale.x <= 10) ? new Vector3(1, 1, 1) * Time.deltaTime : Vector3.zero;

            this.transform.localScale += scaling;
        }
    }

}
