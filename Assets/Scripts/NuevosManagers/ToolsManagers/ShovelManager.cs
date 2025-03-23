//---------------------------------------------------------
// Se encarga de arrancar las malas hierbas y reactivar la maceta después de la acción.
// Alexia Pérez Santana
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


public class ShovelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    /// <summary>
    /// Referencia al selector manager
    /// </summary>
    [SerializeField] private SelectorManager SelectorManager;

    /// <summary>
    /// Referencia al animator del jugador
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    /// <summary>
    /// Referencia al movimiento del jugador
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    /// <summary>
    /// Referencia a la planta en la que se está trabajando
    /// </summary>
    [SerializeField] private PlantEvolution PlantEvolution;

    ///<summary>
    ///Referencia al InventoryUI
    /// </summary>
    [SerializeField] private UIManager UIManager;

    /// <summary>
    /// Aviso para desmalezar con E
    /// </summary>
    [SerializeField] private GameObject Press;

    /// <summary>
    /// Texto del aviso para desmalezar
    /// </summary>
    [SerializeField] private TextMeshProUGUI TextPress;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Booleano para saber si estás colisionando con la planta
    /// </summary>
    private bool _isInWeedArea = false;

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
        // Verifica si se ha presionado la tecla para usar la pala.
        if (InputManager.Instance.UseShovelWasPressedThisFrame())
        {
            Weeding();
        }

        else if (_isInWeedArea && !UIManager.GetInventoryVisible())
        {
            Press.SetActive(true);
            TextPress.text = "Presiona E \npara desmalezar";

            if (InputManager.Instance.UseShovelWasPressedThisFrame())
            {
                Weeding();
            }
        }

        else if (!_isInWeedArea || UIManager.GetInventoryVisible())
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
    /// Método que activa la acción de desmalezar. Inicia la animación
    /// y procesa la eliminación de malas hierbas.
    /// </summary>
    public void Weeding()
    {
        PlayerAnimator.SetBool("Weeding", true);
        Invoke("NotWeeding", 0.8f);
        //PlayerMovement.enableMovement = false;

        if (PlantEvolution != null)
        {
            PlantEvolution.Harvest();
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método para desactivar la animación de desmalezar y reactivar 
    /// la herramienta en la mano del jugador.
    /// </summary>
    private void NotWeeding()
    {
        PlayerAnimator.SetBool("Weeding", false);
        //PlayerMovement.enableMovement = true;
    }

    #endregion

    // ---- EVENTOS----
    #region

    /// <summary>
    /// Método para detectar colisión con la planta.
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Weed"))
        {
            _isInWeedArea = true;
            PlantEvolution = collision.GetComponent<PlantEvolution>();
        }
    }

    /// <summary>
    /// Método para detectar cuando deja de colisionar con la planta.
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weed"))
        {
            _isInWeedArea = false;
            PlantEvolution = null;
        }
    }

    #endregion
} // class ShovelManager 
// namespace
