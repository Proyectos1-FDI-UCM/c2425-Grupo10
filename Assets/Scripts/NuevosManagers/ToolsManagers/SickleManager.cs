//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SickleManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Referencia al selector manager
    /// </summary>
    [SerializeField] private SelectorManager SelectorManager;

    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] PlayerMovement PlayerMovement;

    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    ///<summary> 
    /// Prefab del detector
    /// </summary>
    [SerializeField] private GameObject SickleCollisionDetector;

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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.UseWateringCanWasPressedThisFrame())
        {
            Sicklering();
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
    /// Metodo que activa la recogida de objetos con la HOZ. Se inicia la animacion
    /// </summary>
    public void Sicklering()
    {
        PlayerAnimator.SetBool("Sicklering", true);
        Invoke("NotSicklering", 0.8f);

        PlayerMovement.enablemovement = false;
        InstanceSickleDetector();

        this.gameObject.SetActive(false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    ///<summary>
    ///Metodo para desactivar la animacion de la HOZ y reactivar la herramienta en la mano del jugador
    /// </summary>
    private void NotSicklering()
    {
        PlayerAnimator.SetBool("Sicklering", false);
        PlayerMovement.enablemovement = true;
        this.gameObject.SetActive(true);


    }

    /// <summary>
    /// Instancia el detector en la dirección en la que el jugador está mirando
    /// </summary>
    private void InstanceSickleDetector()
    {
        // Obtener la dirección en la que el jugador está mirando desde PlayerMovement
        Vector2 direccion = PlayerMovement.GetLastMoveDirection().normalized;

        // Definir la posición del detector un poco delante del jugador
        Vector2 posicionDetector = (Vector2)transform.position + direccion * 0.5f;

        // Instanciar el detector en la dirección correcta
        Instantiate(SickleCollisionDetector, posicionDetector, Quaternion.identity);
    }
    #endregion

} // class SickleManager 
// namespace
