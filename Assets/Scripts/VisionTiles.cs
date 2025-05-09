//---------------------------------------------------------
// Detectar colision del jugador con el tejado.
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class VisionTiles : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Referencia al tilemap que queremos bajar la opacidad
    /// </summary>
    [SerializeField] private Tilemap Tilemap;

    /// <summary>
    /// Referencia al SpriteRenderer del arbol para bajar su opacidad
    /// </summary>
    [SerializeField] private SpriteRenderer CopaArbol;

    /// <summary>
    /// Referencia al script Visibilities
    /// </summary>
    [SerializeField] private Visibility Visibility;

    /// <summary>
    /// Referencia al collider del PLayer
    /// </summary>
    [SerializeField] private Collider2D PlayerCollider;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Cantidad de opacidad para el tilemap.
    /// </summary>
    private float _transparentAlpha = 0.5f;

    /// <summary>
    /// Cantidad de jugadores dentro del collider.
    /// </summary>
    private int _playersInside = 0;

    /// <summary>
    /// Booleano para saber si el tejado es transparente o no.
    /// </summary>
    private bool _isTransparent = false;
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
        if (Tilemap == null)
        {
            Tilemap = GetComponent<Tilemap>();
            CopaArbol = GetComponent<SpriteRenderer>();
        }
    }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Metodo para detectar cuando el jugador esta dentro del collider.
    /// </summary>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playersInside = 1; // Como solo hay un jugador en el trigger, lo fijamos en 1
            PlayerCollider = other;

            if (!_isTransparent)
            {
                _isTransparent = true;
                if (Visibility != null)
                    Visibility.Visibilities(_transparentAlpha);
            }
        }
    }
    /// <summary>
    /// Metodo para detectar cuando el jugador sale del collider.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playersInside = 0;
            _isTransparent = false;
            if (Visibility != null)
                Visibility.Visibilities(1f); // Restaurar opacidad 
        }
    }
    #endregion

} // class VisionTiles 
// namespace
