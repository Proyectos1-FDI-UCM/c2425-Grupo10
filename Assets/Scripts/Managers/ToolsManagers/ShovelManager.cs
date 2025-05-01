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

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    /// <summary>
    /// Distancia mínima a la que debe estar el jugador del lugar disponible para plantar
    /// </summary>
    [SerializeField] private float InitialMinDistance;

    /// <summary>
    /// GardenManager, para llamar al método Watering
    /// </summary>
    [SerializeField] private GardenManager GardenManager;
    [SerializeField] private AudioClip WeedingSound;
    [SerializeField] private AudioSource AudioSource;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Booleano para saber si estás colisionando con la planta
    /// </summary>
    private bool _isInWeedArea = false;

    /// <summary>
    /// Array con el transform de todas los lugares disponibles para plantar
    /// </summary>
    private Transform[] Pots;

    ///<summary>
    ///Referencia al CropSpriteEditor
    /// </summary>
    private CropSpriteEditor cropSpriteEditor;

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
        AudioSource = GetComponent<AudioSource>();
        Pots = new Transform[GardenManager.GetGardenSize()]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < GardenManager.GetGardenSize(); i++)
        {
            Pots[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }
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
        AudioSource.clip = WeedingSound;
        AudioSource.Play();
        Invoke("NotWeeding", 0.8f);
        PlayerMovement.DisablePlayerMovement();

        Invoke("WeedingPlant", 1f); // Se realizan los cambios de recolección en la mala hierba al terminar la animación

    }

    /// <summary>
    /// Método que realiza los cambios de desmalezar
    /// (Procesa la eliminación de malas hierbas) Al terminar la animación
    /// </summary>
    public void WeedingPlant()
    {
        Transform Pot = FindNearestPot(transform, Pots);

        Debug.Log("FindNearestPot: " + Pot);
        //Debug.Log(GardenData.GetPlant(Pot.GetChild(0).transform).State);

        if (Pot != null)
        {
            Transform Plant = Pot.transform.GetChild(0);

            if (Plant != null)
            {
                GardenManager.Weed(Pot);
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

    /// <summary>
    /// Método para desactivar la animación de desmalezar y reactivar 
    /// la herramienta en la mano del jugador.
    /// </summary>
    private void NotWeeding()
    {
        PlayerAnimator.SetBool("Weeding", false);
        PlayerMovement.EnablePlayerMovement();
    }

    /// <summary>
    /// Este método se llama cuando el jugador tiene seleccionada la regadera y pulsa E
    /// Busca cual es la planta para regar más cercano al jugador
    /// </summary>
    private Transform FindNearestPot(Transform Player, Transform[] Pots)
    {
        Debug.Log("FindNearestPot");

        Transform NearestPot = null;

        foreach (Transform pot in Pots)
        {
            float MinDistance = InitialMinDistance;
            if (pot.childCount != 0) // Comprueba que hay planta en esta posición
            {
                float SqrDistance = (Player.position - pot.position).sqrMagnitude;
                if (SqrDistance < MinDistance)
                {
                    MinDistance = SqrDistance;
                    NearestPot = pot;
                }
            }
        }

        return NearestPot;
    }
    #endregion

    // ---- EVENTOS----
    #region

    /// <summary>
    /// Método para detectar colisión con la planta.
    /// </summary>
    /// <param name="collision"></param>
    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Weed"))
    //    {
    //        _isInWeedArea = true;
    //        PlantEvolution = collision.GetComponent<PlantEvolution>();
    //    }
    //}

    /// <summary>
    /// Método para detectar cuando deja de colisionar con la planta.
    /// </summary>
    /// <param name="collision"></param>
   // void OnTriggerExit2D(Collider2D collision)
   // {
    //    if (collision.CompareTag("Weed"))
    //    {
    //        _isInWeedArea = false;
    //        PlantEvolution = null;
    //    }
    //}

    #endregion
} // class ShovelManager 
// namespace
