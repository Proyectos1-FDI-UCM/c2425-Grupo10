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
public class Setas : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Tiempo que tarda una seta en volver a aparecer
    /// </summary>
    [SerializeField] private float respawnTime = 120f;

    /// <summary>
    /// Herramienta Hands
    /// </summary>
    [SerializeField] private GameObject Hands;

    /// <summary>
    /// Texto que indica la cantidad de setas recogidas
    /// </summary>
    //[SerializeField] private TextMeshProUGUI CountText;

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
    /// Booleano para saber si la seta es accesible
    /// </summary>
    private bool isAvailable = true;

    /// <summary>
    /// Refencian al sprite de la seta
    /// </summary>
    private Renderer mushroomRenderer;

    /// <summary>
    /// Refencia al collider de la seta
    /// </summary>
    private Collider2D mushroomCollider;

    private TextMeshProUGUI CountText;

    private int costSetas=10;


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
        mushroomRenderer = GetComponent<Renderer>();
        mushroomCollider = GetComponent<Collider2D>();
        CountText = GameObject.Find("CountText").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        int cantidadSetas = InventoryManager.GetMushroomCount();
        CountText.text = "x" + cantidadSetas.ToString();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void TryCollect(SelectorManager selectorManager)
    {
        if (!isAvailable) return;
        if (selectorManager._currentTool == selectorManager.GlovesTool)
        {
            if (InputManager.Instance.UseHandsIsPressed())
            {
                InventoryManager.AddSeta();
                StartCoroutine(HideAndRespawn());
            }
        }
    }

    public void SellButtonPressed()
    {

    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private System.Collections.IEnumerator HideAndRespawn()
    {
        isAvailable = false;
        mushroomRenderer.enabled = false;
        mushroomCollider.enabled = false;
        Debug.Log("La seta ha desaparecido");

        yield return new WaitForSeconds(respawnTime);

        isAvailable = true;
        mushroomRenderer.enabled = true;
        mushroomCollider.enabled = true;
        Debug.Log("La seta vuelto a aparecer");

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject toolManagerObj = GameObject.FindGameObjectWithTag("ToolManager");
            SelectorManager selectorManager = toolManagerObj.GetComponent<SelectorManager>();
            TryCollect(selectorManager);
        }
    }

    #endregion   

} // class Setas 
// namespace
