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
    [SerializeField] private SoundManager SoundManager;
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
    private TutorialManager TutorialManager;
    private NotificationManager NotificationManager;
    private UIManager UIManager;
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
        TutorialManager = FindObjectOfType<TutorialManager>();
        NotificationManager = FindObjectOfType<NotificationManager>();
        UIManager = FindObjectOfType<UIManager>();
        SoundManager = FindObjectOfType<SoundManager>();
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
    public void ChangeScene()
    {
        _sceneTransition.ChangeScene(sceneName);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            SoundManager.InitialSound();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                SavePosition();
            }
            if (SceneManager.GetActiveScene().name == "Escena_Compra")
            {
                if(TutorialManager.GetTutorialPhase() >= 14)
                {
                    _sceneTransition.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    UIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }
            if (SceneManager.GetActiveScene().name == "Escena_Venta")
            {
                if (TutorialManager.GetTutorialPhase() >= 25)
                {
                    _sceneTransition.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    UIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }

            if (_sceneTransition != null)
            {
                if (sceneName == "Escena_Compra")
                {
                    if (TutorialManager.GetTutorialPhase() >= 9)
                    {
                        _sceneTransition.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        UIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                }
                else if (sceneName == "Escena_Venta")
                {
                    if (TutorialManager.GetTutorialPhase() >= 16)
                    {
                        _sceneTransition.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        UIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                }
                else if (sceneName == "Escena_Banco")
                {
                    if (TutorialManager.GetTutorialPhase() >= 25)
                    {
                        _sceneTransition.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        UIManager.ShowNotification("Termina el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                }
                else if (sceneName == "Escena_Mejora")
                {
                    if (TutorialManager.GetTutorialPhase() >= 25)
                    {
                        _sceneTransition.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        UIManager.ShowNotification("Termina el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                }
                

            }
            else
            {
                Debug.LogError("SceneTransition no está asignado en el Inspector.");
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.HideNotification("NoTutorial");
        }
    }

    private void SavePosition()
    {
        InventoryManager.ModifyPlayerPosition(FindObjectOfType<PlayerMovement>().transform.position);
        Debug.Log("SavePosition" + InventoryManager.GetPlayerPosition());
        GameManager.Instance.SaveTime();
    }
    #endregion

} // class CambiarEscena 
// namespace
