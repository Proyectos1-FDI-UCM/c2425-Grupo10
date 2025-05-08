//---------------------------------------------------------
// Se encarga de detectar la entrada del jugador y manejar la recolección.
// Javier Librada
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que gestiona la acción de recolectar cultivos con la hoz.
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

    ///<summary>
    ///Bool que activa/desactiva las características para la build
    /// </summary>
    [SerializeField] private bool Build = true;

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
    ///Referencia al InventoryUI
    /// </summary>
    [SerializeField] private UIManager UIManager;

    ///<summary>
    ///Aviso recolectar con E
    /// </summary>
    [SerializeField] private GameObject Press;

    ///<summary>
    ///Texto del aviso recolectar con E
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

    ///<summary>
    ///ref al tutorial manager 
    /// </summary>
    [SerializeField] private TutorialManager TutorialManager;

    [SerializeField] private AudioClip HarvestSound;

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

    /// <summary>
    /// Array con el transform de todas los lugares disponibles para plantar
    /// </summary>
    private Transform[] _pots;

    ///<summary>
    ///Referencia al CropSpriteEditor
    /// </summary>
    private CropSpriteEditor _cropSpriteEditor;

    ///<summary>
    ///Planta para recolectar
    /// </summary>
    private Plant _plant;

    private AudioSource _audioSource;
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
        _audioSource = GetComponent<AudioSource>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        _pots = new Transform[GardenManager.GetGardenSize()]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < GardenManager.GetGardenSize(); i++)
        {
            _pots[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _plant = CheckCollison();

        // Verifica si se ha presionado la tecla para usar la hoz.
        if (InputManager.Instance.UseSickleWasPressedThisFrame())
        {

            if (TutorialManager.GetTutorialPhase() >= 19)
            {
                Harvest();
            }
            else
            {

                UIManager.ShowNotification("Avanza en el tutorial\npara usar", "NoCounter", 4, "Tool");
                Invoke("NoWarning", 1f);

            }
        }

        if (_isInCropArea && UIManager.GetInventoryVisible() == false)
        {
            if (_plant.State >= 5)
            {
                Press.SetActive(true);
                TextPress.text = "Presiona E \npara recolectar";
            }
        }


        else if (!_isInCropArea || UIManager.GetInventoryVisible() == true)
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
    public void Harvest()
    {
        PlayerMovement.DisablePlayerMovement();
        PlayerAnimator.SetBool("Harvesting", true);

        _audioSource.clip = HarvestSound;
        _audioSource.Play();

        Invoke("NotHarvest", 0.8f);

        // Lo moveremos a después del mensaje (si funciona)
        Invoke("HarvestPlant", 1f); // Se realizan los cambios de recolección en la planta al terminar la animación

    }

    /// <summary>
    /// Método que realiza los cambios de cultivar
    /// (Procesa la eliminación de las plantas) Al terminar la animación
    /// </summary>
    public void HarvestPlant()
    {
        Transform Pot = FindNearestPot(transform, _pots);
  
        Debug.Log("FindNearestPot: " + Pot);
        //Debug.Log(GardenData.GetPlant(Pot.GetChild(0).transform).State);

        if (Pot != null)
        {
            GardenManager.Harvest(Pot);
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
    private void NotHarvest()
    {

        PlayerAnimator.SetBool("Harvesting", false);

        PlayerMovement.EnablePlayerMovement();

    }

    /// <summary>
    /// Este método se llama cuando el jugador tiene seleccionada la azada y pulsa E
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


    ///<summary>
    ///metodo para ocultar la notificacion
    /// </summary>
    private void NoWarning()
    {
        UIManager.HideNotification("Tool");
    }

    /// <summary>
    /// Método para detectar las colisiones con los cultivos
    /// </summary>
    private Plant CheckCollison()
    {
        Plant plant = new Plant();
        Transform pot = FindNearestPot(transform, _pots);
        if (pot != null && pot.GetChild(0) != null) 
        {
            Transform crop = pot.GetChild(0);
            plant = GardenData.GetPlant(crop);
            if (plant.State >= 5)
                _isInCropArea = true;
            //if (plant.State == 4) 
        }
        else _isInCropArea = false;
        return plant;
    }

    /// <summary>
    /// metodo para detectar colision con pozo/cultivo
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.GetComponent<CropSpriteEditor>())
        {
            _isInCropArea = true;
            _cropSpriteEditor = collision.GetComponent<CropSpriteEditor>();
        }

    }

    /// <summary>
    /// metodo para detectar cuando deja de colisionar con pozo/cultivo.
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.GetComponent<CropSpriteEditor>())
        {

            _isInCropArea = false;

            _cropSpriteEditor = null;

        }

    }
    #endregion
} // class SickleManager 
// namespace
