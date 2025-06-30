//---------------------------------------------------------
// Manager que se encarga de realizar todas las tareas de "semillas" (Plantar la semilla adecuada, inicializar la planta...)
// Julia Vera Ruiz
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;


/// <summary>
/// SeedsManager se encarga de plantar la semilla adecuada 
/// Se planta si el jugador esta a una distancia mínima de los lugares específicos para plantar
/// Planta la semilla que el jugador tiene seleccionada
/// </summary>
public class SeedsManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    ///<summary> 
    ///Referencia al SelectorManager
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    /// <summary>
    ///Prefab de la Semilla 0
    /// </summary>
    [SerializeField] private GameObject PrefabSeeds0;

    /// <summary>
    /// Prefab de la Semilla 1
    /// </summary>
    [SerializeField] private GameObject PrefabSeeds1;

    /// <summary>
    /// Prefab de la Semilla 2
    /// </summary>
    [SerializeField] private GameObject PrefabSeeds2;

    /// <summary>
    /// Prefab de la Semilla 3
    /// </summary>
    [SerializeField] private GameObject PrefabSeeds3;

    /// <summary>
    /// Distancia mínima a la que debe estar el jugador del lugar disponible para plantar
    /// </summary>
    [SerializeField] private float InitialMinDistance;

    /// <summary>
    /// Ref al gardenManager
    /// </summary>
    [SerializeField] private GardenManager gardenManager;

    ///<summary>
    ///gameobject mensaje cantidad semillas
    ///</summary>
    [SerializeField] private GameObject AmountOfSeeds;

    ///<summary>
    ///Texto cantidad de semillas
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountSeedsText;

    ///<summary>
    ///Ref al ui manager
    /// </summary>
    [SerializeField] private UIManager UIManager;

    ///<summary>
    ///Audio de plantar
    /// </summary>
    [SerializeField] private AudioSource PlantAudio;


    // NUEVOS CAMPOS:
    /// <summary>
    /// Referencia al FertilizerManager
    /// </summary>
    [SerializeField] private FertilizerManager FertilizerManager;

    /// <summary>
    /// Texto cantidad de abono
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountFertilizerText;

   


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
    /// Array con el transform de todas los lugares disponibles para plantar
    /// </summary>
    private Transform[] _pots;

    /// <summary>
    /// Prefab de la semilla seleccionada
    /// </summary>
    private GameObject _prefab;

    /// <summary>
    /// Semilla actual
    /// </summary>
    private Items _seed;

    ///<summary>
    ///Nombre de la semilla para mostrar en el mensaje
    /// </summary>
    private string _nameSeed;

    /// <summary>
    /// Tutorial Manager
    /// </summary>
    private TutorialManager _tutorialManager;

    /// <summary>
    /// Si está en modo abono
    /// </summary>
    private bool _isFertilizerMode = false;

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
        // Inicializar array de posiciones para plantar
        _pots = new Transform[PlantingSpots.transform.childCount];
        for (int i = 0; i < PlantingSpots.transform.childCount; i++)
        {
            _pots[i] = PlantingSpots.transform.GetChild(i).transform;
        }

        // Buscar referencias si no están asignadas
        _tutorialManager = FindObjectOfType<TutorialManager>();

        if (PlantAudio == null)
        {
            PlantAudio = GetComponent<AudioSource>();
        }

        if (FertilizerManager == null)
        {
            FertilizerManager = FindObjectOfType<FertilizerManager>();
        }

        // Inicializar con lechuga por defecto
        ChangeSeed(1); // Lechuga como primera semilla por defecto

        Debug.Log("SeedsManager inicializado correctamente");
    }

    /// <summary>
    /// Update (MODIFICADO para incluir abono)
    /// </summary>
    void Update()
    {
        // Detectar input para usar (tecla E)
        if (InputManager.Instance.UsarWasPressedThisFrame())
        {
            if (_isFertilizerMode)
            {
                // MODO ABONO: Aplicar abono a plantas cercanas
                if (InventoryManager.GetInventoryItem(Items.Fertilizer) > 0)
                {
                    if (FertilizerManager != null)
                    {
                        FertilizerManager.TryApplyFertilizer();
                    }
                    else
                    {
                        Debug.LogError("FertilizerManager no está asignado");
                    }
                }
                else
                {
                    Debug.Log("No tienes abono en el inventario");
                }
            }
            else
            {
                // MODO SEMILLAS: Plantar semilla (lógica existente)
                if (InventoryManager.GetInventoryItem(_seed) > 0)
                {
                    Transform Pot = FindNearestPot(transform, _pots);
                    if (Pot != null)
                    {
                        // Tutorial check
                        if (_tutorialManager.GetTutorialPhase() == 14)
                        {
                            _tutorialManager.CheckBox(0);
                            _tutorialManager.Invoke("NextDialogue", 0.6f);
                        }

                        // Crear la planta
                        GameObject Plant = Instantiate(_prefab, Pot.position, Quaternion.identity);
                        Plant.transform.SetParent(Pot);

                        // Consumir semilla del inventario
                        InventoryManager.ModifyInventorySubstract(_seed, 1);

                        // Activar en GardenData
                        GardenData.Active(Pot.transform, (int)_seed + ((int)Items.Count / 2));

                        // Configurar warnings y efectos
                        CropSpriteEditor crop = Plant.GetComponent<CropSpriteEditor>();
                        if (crop != null)
                        {
                            crop.Warning("Water");
                        }

                        // Animación y sonido
                        PlayerAnimator.SetBool("Planting", true);
                        if (PlantAudio != null)
                        {
                            PlantAudio.Play();
                        }

                        Invoke("NotPlanting", 0.4f);
                        PlayerMovement.DisablePlayerMovement();

                        Debug.Log($"Plantada: {_nameSeed} en posición {Pot.position}");
                    }
                    else
                    {
                        Debug.Log("No hay lugares disponibles para plantar cerca");
                    }
                }
                else
                {
                    Debug.Log($"No tienes semillas de {_nameSeed} en el inventario");
                }
            }
        }

        // Actualizar UI de cantidad
        UpdateUI();
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
    /// Cambia al modo abono
    /// </summary>
    public void ChangeToFertilizer()
    {
        _isFertilizerMode = true;

        // Actualizar texto para mostrar cantidad de abono
        if (AmountSeedsText != null)
        {
            AmountSeedsText.text = "x" + InventoryManager.GetInventoryItem(Items.Fertilizer).ToString();
        }

        Debug.Log("Modo abono activado");
    }

    /// <summary>
    /// Cambia a una semilla específica
    /// </summary>
    /// <param name="Seed">Índice de la semilla (0=Maíz, 1=Lechuga, 2=Zanahoria, 3=Fresa)</param>
    public void ChangeSeed(int Seed)
    {
        _isFertilizerMode = false; // IMPORTANTE: Desactivar modo abono al cambiar a semilla

        // Mapear índice del selector a Items enum correcto
        switch (Seed)
        {
            case 0:
                _seed = Items.CornSeed;
                _prefab = PrefabSeeds0;
                _nameSeed = "Maíz";
                break;
            case 1:
                _seed = Items.LettuceSeed;
                _prefab = PrefabSeeds1;
                _nameSeed = "Lechuga";
                break;
            case 2:
                _seed = Items.CarrotSeed;
                _prefab = PrefabSeeds2;
                _nameSeed = "Zanahoria";
                break;
            case 3:
                _seed = Items.StrawberrySeed;
                _prefab = PrefabSeeds3;
                _nameSeed = "Fresa";
                break;
            default:
                Debug.LogError("Índice de semilla inválido: " + Seed);
                _seed = Items.LettuceSeed;
                _prefab = PrefabSeeds1;
                _nameSeed = "Lechuga";
                break;
        }

        // Actualizar texto de cantidad en UI
        if (AmountSeedsText != null)
        {
            AmountSeedsText.text = "x" + InventoryManager.GetInventoryItem(_seed).ToString();
        }

        Debug.Log($"Semilla cambiada a: {_nameSeed}");
    }


    /// <summary>
    /// Verifica si está en modo abono
    /// </summary>
    /// <returns>True si está en modo abono</returns>
    public bool IsFertilizerMode()
    {
        return _isFertilizerMode;
    }

    /// <summary>
    /// Obtiene la semilla actual seleccionada
    /// </summary>
    /// <returns>Items enum de la semilla actual</returns>
    public Items GetCurrentSeed()
    {
        return _seed;
    }

    /// <summary>
    /// Obtiene el nombre de la semilla actual
    /// </summary>
    /// <returns>Nombre legible de la semilla actual</returns>
    public string GetCurrentSeedName()
    {
        return _nameSeed;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    /// <summary>
    /// Este método se llama cuando el jugador tiene seleccionadas las semillas y pulsa E (usar)
    /// Busca cual es el lugar disponible para plantar más cercano al jugador
    /// </summary>
    private Transform FindNearestPot(Transform Player, Transform[] Pots)
    {
        Transform NearestPot = null;

        foreach (Transform pot in Pots)
        {
            float MinDistance = InitialMinDistance;
            if (pot.gameObject.activeInHierarchy && pot.childCount == 0) // Comprueba que no hay ninguna planta en esta posición
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

    /// <summary>
    /// Metodo para desactivar la animacion de plantar
    /// </summary>
    private void NotPlanting()
    {

        PlayerAnimator.SetBool("Planting", false);

        this.gameObject.SetActive(true);

        PlayerMovement.EnablePlayerMovement();

    }

    /// <summary>
    /// Actualiza la interfaz de usuario
    /// </summary>
    private void UpdateUI()
    {
        if (UIManager != null && !UIManager.GetInventoryVisible())
        {
            if (AmountOfSeeds != null)
            {
                AmountOfSeeds.SetActive(true);
            }

            if (AmountSeedsText != null)
            {
                if (_isFertilizerMode)
                {
                    AmountSeedsText.text = "x" + InventoryManager.GetInventoryItem(Items.Fertilizer).ToString();
                }
                else
                {
                    AmountSeedsText.text = "x" + InventoryManager.GetInventoryItem(_seed).ToString();
                }
            }
        }
        else
        {
            if (AmountOfSeeds != null)
            {
                AmountOfSeeds.SetActive(false);
            }
        }
    }


    #endregion

} // class SeedsManager 
// namespace
