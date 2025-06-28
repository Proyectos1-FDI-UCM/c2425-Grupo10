//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Botas : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary>
    ///Objeto que hace referencia a la interfaz de compra de las botas
    ///<summary>
    [SerializeField] public GameObject interfazBotas;

    ///<summary>
    ///Referencia a la descripcion de las botas
    ///<summary>
    [SerializeField] public TextMeshProUGUI descripcionBotas;

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
    private bool _isBotasSold;
    private int costBotas = 800;
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
        interfazBotas.SetActive(false);
        _isBotasSold = false;
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
        if (interfazActiva && !_isBotasSold)
        {
            if (moneyManager.GetMoneyCount() >= costBotas)
            {
                _isBotasSold = true;
                textos.SetActive(false);
                soldText.SetActive(true);
                PlayerMovement.ChangeBotasSold();
                moneyManager.DeductMoney(costBotas);
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
            if (PlayerMovement.Botas()) 
            { 
                soldText.SetActive(true);
            }
            else
            {
                Debug.Log("Collision enter");
                interfazBotas.SetActive(true);
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
            interfazBotas.SetActive(false);
            soldText.SetActive(false);
            interfazActiva = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    

    #endregion   

} // class Botas 
// namespace
