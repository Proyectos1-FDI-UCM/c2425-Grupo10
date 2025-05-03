//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BeachHouseCamera : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float targetZoom = 5f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float zoomSpeed = 2f;

    // Límites para que la cámara pare de hacer zoom 
    [SerializeField] float stopDistance = 0.05f; // Umbral de distancia para detener
    [SerializeField] float stopZoomThreshold = 0.05f; // Umbral de zoom para detener
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool active = false;
    private Camera cam;

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
        cam = Camera.main;

        if (PlayerPrefs.GetInt("activarCamara", 0) == 1)
        {
            active = true;
            PlayerPrefs.SetInt("activarCamara", 0); // Reset variable 
        }
    }

    void Update()
    {
        if (!active) return;

        // Movimiento
        Vector3 currentPos = transform.position;
        Vector3 desiredPos = new Vector3(targetPosition.x, targetPosition.y, currentPos.z);
        transform.position = Vector3.Lerp(currentPos, desiredPos, Time.deltaTime * moveSpeed);

        // Zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // Verificar si ya llegó al destino y al zoom deseado
        bool reachedPosition = Vector2.Distance(new Vector2(currentPos.x, currentPos.y), new Vector2(targetPosition.x, targetPosition.y)) < stopDistance;
        bool reachedZoom = Mathf.Abs(cam.orthographicSize - targetZoom) < stopZoomThreshold;

        if (reachedPosition && reachedZoom)
        {
            active = false;
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

    #endregion

} // class BeachHouseCamera 
// namespace
