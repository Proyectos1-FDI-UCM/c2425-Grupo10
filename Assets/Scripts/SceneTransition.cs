//---------------------------------------------------------
// Transición antes del cambio de escena.
// Javier Librada Jerez
// Roots of Life
// Proyecto 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Clase que maneja la transición entre escenas,
/// permitiendo un efecto de desvanecimiento al cambiar de escena.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (Serialized Fields)

    /// <summary>
    /// SpriteRenderer utilizado para el efecto de desvanecimiento.
    /// </summary>
    [SerializeField] private Image FadeSprite;
    [SerializeField] private GameObject FadeObject;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (Private Fields)

    /// <summary>
    /// Instancia de la clase SceneTransition para asegurar que solo haya una.
    /// </summary>
    private static SceneTransition _instance;

    /// <summary>
    /// Velocidad de degradado.
    /// </summary>
    private float _fadeSpeed = 2f;

    /// <summary>
    /// Booleanos para saber si se esta degradando para salir de escena o para entrar.
    /// </summary>
    private bool _fadingOut = false;
    private bool _fadingIn = true;

    /// <summary>
    /// Nombre de la escena a la que quieres cambiar.
    /// </summary>
    private string _sceneToLoad;

    private bool _isFinalCamera = false;

    [SerializeField] private string _currentSceneName;
    [SerializeField] private bool _shouldUseClouds;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa la instancia y asegura que no se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {
        if (_instance != null)
        {
            // Nos destruimos si ya hay una instancia existente.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Configuramos la instancia y evitamos que se destruya al cambiar de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(FadeObject);
        }
    }

    /// <summary>
    /// Se llama cada frame, si el MonoBehaviour está habilitado.
    /// Maneja el efecto de desvanecimiento al cambiar de escena.
    /// </summary>
    void Update()
    {
        // Fade Out
        if (_fadingOut)
        {
            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 1, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);
            FadeSprite.raycastTarget = true;

            if (alpha >= 0.99f)
            {
                SceneManager.LoadScene(_sceneToLoad);
                FadeSprite.color = new Color(0, 0, 0, 1);
                _fadingOut = false;
                _fadingIn = true;
            }
        }

        // Fade In
        if (_fadingIn)
        {
            float alpha = Mathf.MoveTowards(FadeSprite.color.a, 0, _fadeSpeed * Time.deltaTime);
            FadeSprite.color = new Color(0, 0, 0, alpha);

            if (alpha <= 0.01f)
            {
                if (_shouldUseClouds)
                {
                    Cloud.Instance.HideClouds();
                    _isFinalCamera = false;
                }

                FadeSprite.color = new Color(0, 0, 0, 0);
                FadeSprite.raycastTarget = false;
                _fadingIn = false;
            }
        }
    }

    /// <summary>
    /// Se llama cuando el componente se destruye.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static SceneTransition Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y falso en otro caso.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método para cambiar de escena.
    /// </summary>
    /// <param name="sceneName">Nombre de la escena a cargar.</param>
    public void ChangeScene(string sceneName)
    {
        _sceneToLoad = sceneName;
        _currentSceneName = SceneManager.GetActiveScene().name;

        // Verifica si vamos desde o hacia el menú
        _shouldUseClouds = (_currentSceneName == "Menu" || sceneName == "Menu" || _isFinalCamera == true);
        if (_shouldUseClouds)
        {
            Cloud.Instance.ShowClouds();
            Invoke("LoadScene", 1.5f);
        }
        else
        {
            LoadScene();
        }
        FadeSprite.raycastTarget = true; // Bloquear clics durante la transición
    }

    /// <summary>
    /// Método para cargar la escena.
    /// </summary>
    public void LoadScene()
    {
        _fadingOut = true; // Comienza el fade out
    }

    public void FinalCamera()
    {
        _isFinalCamera = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar aquí si agregas métodos privados adicionales.
    #endregion

} // class SceneTransition