//---------------------------------------------------------
// Cambiar de una escena a otra
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CambiarEscena : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] public string sceneName;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    public SceneTransition _sceneTransition;
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
        _sceneTransition = FindObjectOfType<SceneTransition>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_sceneTransition == null)
        {
            GameObject transitionObject = GameObject.FindGameObjectWithTag("ObjetoTransicion");
            if (transitionObject != null)
            {
                _sceneTransition = transitionObject.GetComponent<SceneTransition>();
            }
            else
            {
                Debug.Log("No se encontró un objeto con la etiqueta 'ObjetoTransicion'.");
            }
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
    public void BackToMenu()
    {
        _sceneTransition.ChangeScene(sceneName);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                SavePosition();
            }
            if (_sceneTransition != null)
            {
                _sceneTransition.ChangeScene(sceneName);
                Debug.Log("Cambiando a escena: " + sceneName);
            }
            else
            {
                Debug.LogError("SceneTransition no está asignado en el Inspector.");
            }
        }
    }

    private void SavePosition()
    {
        InventoryManager.ModifyPlayerPosition(FindObjectOfType<PlayerMovement>().transform.position);
        Debug.Log("SavePosition" + InventoryManager.GetPlayerPosition());
    }
    #endregion

} // class CambiarEscena 
// namespace
