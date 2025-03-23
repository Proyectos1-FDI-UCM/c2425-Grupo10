//---------------------------------------------------------
// Cambiar la opacidad del tejado.
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que maneja la visibilidad de los objetos en el juego,
/// permitiendo cambiar la opacidad del tejado y otros elementos.
/// </summary>
public class Visibility : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Referencia al Tilemap que se desea modificar.
    /// </summary>
    [SerializeField] private Tilemap Tilemap;

    /// <summary>
    /// Referencia al SpriteRenderer del árbol que se desea modificar.
    /// </summary>
    [SerializeField] private SpriteRenderer Tree;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
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
        Tilemap = GetComponent<Tilemap>();
        Tree = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Cambia la visibilidad del Tilemap y del árbol ajustando su opacidad.
    /// </summary>
    /// <param name="_amount">Valor de opacidad (0.0f a 1.0f).</param>
    public void Visibilities(float _amount)
    {
        if (Tilemap != null)
        {
            Color _color = Tilemap.color;
            _color.a = _amount;
            Tilemap.color = _color;
        }
        if (Tree != null)
        {
            Color _color = Tree.color;
            _color.a = _amount;
            Tree.color = _color;
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

} // class Visibilities 
// namespace
