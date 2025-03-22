//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
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
    ///Referencia al PlantaEvolucion del crop
    /// </summary>
    [SerializeField] private PlantaEvolucion PlantaEvolucion;

    ///<summary>
    ///Referencia al InventoryUI
    /// </summary>
    [SerializeField] private InventoryCultivos InventoryCultivos;

    ///<summary>
    ///Aviso recolectar con E
    /// </summary>
    [SerializeField] private GameObject Press;
    ///<summary>
    ///Texto del aviso recolectar con E
    /// </summary>
    [SerializeField] private TextMeshProUGUI TextPress;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    ///<summary>
    ///Booleano para saber si estas colisionando con el crop
    /// </summary>
    private bool _isInCropArea = false;
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

        if (InputManager.Instance.UseSickleWasPressedThisFrame())
        {

            Croping();

        }

        else if (_isInCropArea && InventoryCultivos.GetInventoryVisible() == false)
        {
            if (PlantaEvolucion.GetCropState() == 3)
            {
                Press.SetActive(true);

                TextPress.text = "Presiona E \npara recolectar";

                if (InputManager.Instance.UseSickleWasPressedThisFrame())
                {

                    Croping();

                }

            }
            

        }

        else if (!_isInCropArea || InventoryCultivos.GetInventoryVisible() == true)
        {

            Press.SetActive(false);

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
    public void Croping()
    {
        PlayerAnimator.SetBool("Sicklering", true);

        Invoke("NotCroping", 0.8f);

        PlayerMovement.enablemovement = false;

        if (PlantaEvolucion != null)
        {

            PlantaEvolucion.Croping();

        }

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
    private void NotCroping()
    {

        PlayerAnimator.SetBool("Sicklering", false);

        PlayerMovement.enablemovement = true;

    }
    #endregion

    // ---- EVENTOS----
    #region

    /// <summary>
    /// metodo para detectar colision con pozo/cultivo
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Crop"))
        {

            _isInCropArea = true;

            PlantaEvolucion = collision.GetComponent<PlantaEvolucion>();
        }

    }
    /// <summary>
    /// metodo para detectar cuando deja de colisionar con pozo/cultivo.
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Crop"))
        {

            _isInCropArea = false;

            PlantaEvolucion = null;

        }

    }
    #endregion
} // class SickleManager 
// namespace
