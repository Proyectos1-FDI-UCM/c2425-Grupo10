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
    /// Referencia al notification manager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;

    /// <summary>
    /// Audio Source
    /// </summary>
    [SerializeField] private AudioSource AudioSource;

    /// <summary>
    /// Audio del Inventario
    /// </summary>
    [SerializeField] private AudioClip Inventory;

    /// <summary>
    /// Audio al usar el input ESC
    /// </summary>
    [SerializeField] private AudioClip Esc;

    /// <summary>
    /// Audio al usar el input Q
    /// </summary>
    [SerializeField] private AudioClip Q;

    /// <summary>
    /// Audio al entrar a la casa
    /// </summary>
    [SerializeField] private AudioClip HouseEnter;

    /// <summary>
    /// Audio de las interacciones
    /// </summary>
    [SerializeField] private AudioClip Interactions;

    /// <summary>
    /// Audio del Slider (Banco)
    /// </summary>
    [SerializeField] private AudioClip Sliders;

    /// <summary>
    /// Audio al usar el input E
    /// </summary>
    [SerializeField] private AudioClip E;

    /// <summary>
    /// Audio de MadameMoo (Tutorial)
    /// </summary>
    [SerializeField] private AudioClip MadameMoo;

    /// <summary>
    /// Audio de botón siguiente (Tutorial)
    /// </summary>
    [SerializeField] private AudioClip NextButton;

    /// <summary>
    /// Audio al cargar nueva partida
    /// </summary>
    [SerializeField] private AudioClip NewGame;
    

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

    /// <summary>
    /// Metodo para Activar el sonido del inventario
    /// </summary>
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

    /// <summary>
    /// Método para Activar el sonido del menú
    /// </summary>
    public void EscSound()
    {
        AudioSource.clip = Esc;
        AudioSource.Play();
    }

    /// <summary>
    /// Método que Activa el sonido de la vaca y modifica su rango
    /// </summary>
    public void MadameMooSound()
    {
        AudioSource.clip = MadameMoo;
        AudioSource.pitch = UnityEngine.Random.Range(0.5f, 2f);
        AudioSource.Play();
    }

    /// <summary>
    /// Método que Activa el sonido de botón
    /// </summary>
    public void NextButtonSound()
    {
        AudioSource.clip = NextButton;
        AudioSource.Play();
    }


    /// <summary>
    /// Método que Activa el sonido del input Q
    /// </summary>
    public void QSound()
    {
        AudioSource.clip = Q;
        AudioSource.Play();
    }

    /// <summary>
    /// Método que Activa el sonido de la casa
    /// </summary>
    public void HouseSound()
    {
        AudioSource.clip = HouseEnter;
        AudioSource.Play();
    }

    /// <summary>
    /// Método que Activa el sonido del input E
    /// </summary>
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
