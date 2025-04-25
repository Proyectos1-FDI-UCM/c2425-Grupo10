//---------------------------------------------------------
// Controla el sonido del juego
// Julia Vera Ruiz
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Se encarga de gestionar y reproducir los efectos de sonido
/// </summary>
public class SoundManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Ref al notification manager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;

    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip Inventory;
    [SerializeField] private AudioClip Esc;
    [SerializeField] private AudioClip Q;
    [SerializeField] private AudioClip HouseEnter;
    [SerializeField] private AudioClip Interactions;
    [SerializeField] private AudioClip Sliders;
    [SerializeField] private AudioClip E;
    [SerializeField] private AudioClip MadameMoo;
    [SerializeField] private AudioClip NextButton;
    [SerializeField] private AudioClip NewGame;
    

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
    void Awake()
    {
        InitializeReferences();
        NotificationManager.InitializeSoundManager();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.TabWasPressedThisFrame()) TabSound();
        if (InputManager.Instance.ExitWasPressedThisFrame()) EscSound();
        if (InputManager.Instance.SalirWasPressedThisFrame()) QSound();
        if (InputManager.Instance.UsarWasPressedThisFrame()) ESound();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void TabSound()
    { 
        AudioSource.clip = Inventory;
        AudioSource.Play();
    }

    /// <summary>
    /// Metodo para Activar el sonido de inicio de partida
    /// </summary>
    public void InitialSound()
    {
        AudioSource.clip = NewGame;
        AudioSource.Play();
    }
    public void EscSound()
    {
        AudioSource.clip = Esc;
        AudioSource.Play();
    }
    public void MadameMooSound()
    {
        AudioSource.clip = MadameMoo;
        AudioSource.pitch = UnityEngine.Random.Range(0.5f, 2f);
        AudioSource.Play();
    }

    public void NextButtonSound()
    {
        AudioSource.clip = NextButton;
        AudioSource.Play();
    }

    public void QSound()
    {
        AudioSource.clip = Q;
        AudioSource.Play();
    }

    public void HouseSound()
    {
        AudioSource.clip = HouseEnter;
        AudioSource.Play();
    }

    public void ESound()
    {
        AudioSource.clip = E;
        AudioSource.Play();
    }



    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    
    /// <summary>
    /// Metodo para inicializar referencias del script
    /// </summary>
    private void InitializeReferences()
    {
        NotificationManager = FindObjectOfType<NotificationManager>();

    }
    #endregion

} // class SoundManager 
// namespace
