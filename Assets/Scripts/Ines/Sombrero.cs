//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Sombrero : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary>
    ///Objeto que hace referencia a la interfaz de compra del sombrero
    ///<summary>
    [SerializeField] public GameObject interfazSombrero;

    ///<summary>
    ///Referencia a la descripcione del sombrero
    ///<summary>
    [SerializeField] public TextMeshProUGUI descripcionSombrero;

    ///<summary>
    ///Referencia al texto de "Ya lo has comprado"
    ///<summary>
    [SerializeField] public GameObject soldText;

    ///<summary>
    ///Referencia a los textos de la interfaz
    ///<summary>
    [SerializeField] public GameObject textos;

    [SerializeField] public PlayerMovement PlayerMovement;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool interfazActiva = false; 
    private bool _isSombreroSold = false;
    private int costSombrero = 600;
    private MoneyManager moneyManager;

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
        interfazSombrero.SetActive(false);
        _isSombreroSold = false;
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        moneyManager = obj.GetComponent<MoneyManager>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public bool InterfazActiva()
    {
        return interfazActiva;
    }

    public void ButtonBuyPressed()
    {
        if (interfazActiva && !_isSombreroSold)
        {
            if (moneyManager.GetMoneyCount() >= costSombrero)
            {
                _isSombreroSold = true;
                textos.SetActive(false);
                soldText.gameObject.SetActive(true);
                PlayerMovement.ChangeSombreroSold();
                moneyManager.DeductMoney(costSombrero);
            }
        }
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
            if (PlayerMovement.Sombrero())
            {
                soldText.SetActive(true);
            }
            else
            { 
                Debug.Log("Collision enter");
            interfazSombrero.SetActive(true);
            interfazActiva = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coliision exit");
            interfazSombrero.SetActive(false);
            interfazActiva = false;
            soldText.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    #endregion   

} // class Sombrero 
// namespace
