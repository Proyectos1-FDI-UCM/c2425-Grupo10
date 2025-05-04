//---------------------------------------------------------
// Transicion antes del cambio de escena.
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que maneja la transición entre escenas,
/// permitiendo un efecto de desvanecimiento al cambiar de escena.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Instancia de la clase SceneTransition para asegurar que solo haya una.
    /// </summary>
    private static SceneTransition Instance;

    /// <summary>
    /// SpriteRenderer utilizado para el efecto de desvanecimiento.
    /// </summary>
    [SerializeField] private Image FadeSprite;
    [SerializeField] private GameObject FadeObject;
    [SerializeField] private Cloud Cloud;
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
    private float _fadeSpeed =2f;

    /// <summary>
    /// Booleanos para saber si se esta degradando para salir de escena o para entrar.
    /// </summary>
    private bool _fadingOut = false;
    private bool _fadingIn = true;

    /// <summary>
    /// Nombre de la escena a la que quieres cambiar.
    /// </summary>
    private string _sceneToLoad;

    [SerializeField] private string _currentSceneName;
    [SerializeField] private bool _shouldUseClouds;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa la instancia y asegura que no se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(FadeObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //FadeSprite.raycastTarget = false; // Permitir clics a través de la imagen
        //FadeSprite.color = new Color(0, 0, 0, 0);
        Cloud = FindObjectOfType<Cloud>();

    }

    /// <summary>
    /// Se llama cada frame, si el MonoBehaviour está habilitado.
    /// Maneja el efecto de desvanecimiento al cambiar de escena.
    /// </summary>
    void Update()
    {
        
        // Aumentar la opacidad (Fade Out)
        if (_fadingOut)
        {
            

            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 1, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);
            FadeSprite.raycastTarget = true;

            if (alpha >= 0.99f)
            {
                LoadScene();
                FadeSprite.color = new Color(0, 0, 0, 1);
                _fadingOut = false;
                _fadingIn = true;
            }
        }

        if (_fadingIn)
        {
            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 0, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);

            if (alpha <= 0.01f)
            {
                if (_shouldUseClouds && Cloud != null)
                    Cloud.HideClouds();

                FadeSprite.color = new Color(0, 0, 0, 0);
                FadeSprite.raycastTarget = false;
                _fadingIn = false;
            }
        }


    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Método para cambiar de escena.
    /// </summary>
    /// <param name="sceneName">Nombre de la escena a cargar.</param>
    public void ChangeScene(string sceneName)
    {
        _sceneToLoad = sceneName;
        _currentSceneName = SceneManager.GetActiveScene().name;

        // Verifica si vamos desde o hacia el menú
        _shouldUseClouds = (_currentSceneName == "Menu" || sceneName == "Menu");

        _fadingOut = true; // Comienza el fade out
        FadeSprite.raycastTarget = true; // Bloquear clics durante la transición

    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    // Coroutine para esperar que las nubes estén completamente visibles



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
