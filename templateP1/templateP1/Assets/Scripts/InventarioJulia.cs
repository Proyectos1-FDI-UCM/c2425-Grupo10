//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class InventarioJulia : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private int[] inventario;
    private TextMeshProUGUI Unidades;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        inventario = GameManager.Instance.Inventario();
        //prueba();
        MostrarInventario();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) MostrarInventario();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void MostrarInventario()
    {
        for (int i = 0; i < 4; i++)
        {
             GameObject _crops = transform.GetChild(i).gameObject;
            if (inventario[i] != 0) 
            { _crops.SetActive(true);
                Unidades = _crops.GetComponentInChildren<TextMeshProUGUI>();
                Unidades.text = inventario[i] + "x";
            }
            else _crops.SetActive(false);
        }
        for (int i = 4; i < 8; i++)
        {
            if (inventario[i] != 0)
            {
                int j = 1;
                bool b = true;
                while (j < 5 && b)
                {
                    GameObject _crops = transform.GetChild(i * j).gameObject;
                    _crops.SetActive(true);
                    Unidades = _crops.GetComponentInChildren<TextMeshProUGUI>();
                    if (inventario[i] / (j * 10) != 0)
                    {
                        Unidades.text = 10 + "x";
                    }
                    else if (inventario[i] - ((j - 1) * 10) != 0)
                    {
                        Unidades.text = inventario[i] - ((j-1) * 10) + "x";
                        b = false;
                    }
                    else
                    {
                        _crops.SetActive(false);
                        b = false;
                    }
                    j++;
                }
            }
            else transform.GetChild(i).gameObject.SetActive(false);
        }


    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class InventarioJulia 
// namespace
