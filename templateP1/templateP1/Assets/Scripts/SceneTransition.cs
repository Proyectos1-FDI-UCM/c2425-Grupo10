//---------------------------------------------------------
// Transicion antes del cambio de escena.
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
public class SceneTransition : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private SceneTransition Instance;
    [SerializeField] private SpriteRenderer FadeSprite;
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
    /// Velocidad de degradado.
    /// </summary>
    private float _fadeSpeed =5f;

    /// <summary>
    /// Booleanos para saber si se esta degradando para salir de escena o para entrar.
    /// </summary>
    private bool _fadingOut = false;
    private bool _fadingIn = true;

    /// <summary>
    /// Nombre de la escena a la que quieres cambiar.
    /// </summary>
    private string _sceneToLoad;
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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_fadingIn)
        {
            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 0, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);

            if (alpha <= 0.01f)
            {
                FadeSprite.color = new Color(0, 0, 0, 0);
                _fadingIn = false;
            }
        }

        if (_fadingOut)
        {
            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 1, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);

            if (alpha >= 0.99f)
            {
                FadeSprite.color = new Color(0, 0, 0, 1);
                _fadingOut = false;
                SceneManager.LoadScene(_sceneToLoad);
                _fadingIn = true; // Iniciar el Fade In en la nueva escena
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

    /// <summary>
    /// Metodo para cambiar de escena.
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        _sceneToLoad = sceneName;
        _fadingOut = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class SceneTransition 
// namespace
