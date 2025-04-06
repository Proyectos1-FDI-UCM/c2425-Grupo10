//---------------------------------------------------------
// Manager que se encarga de realizar todas las tareas de "semillas" (Plantar la semilla adecuada, inicializar la planta...)
// Julia Vera Ruiz
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


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
        _pots = new Transform[PlantingSpots.transform.childCount]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < PlantingSpots.transform.childCount; i++)
        {
            _pots[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.UsarWasPressedThisFrame() && InventoryManager.GetInventory(_seed) > 0)
        {
            Transform Pot = FindNearestPot(transform, _pots); // Busca un lugar disponible para plantar
            if (Pot != null) // Si lo encuentra, instancia el prefab de la semilla seleccionada
            {
                GameObject Plant = Instantiate(_prefab, Pot.position, Quaternion.identity);
                InventoryManager.ModifyInventorySubstract((Items)_seed, 1);
                Plant.transform.SetParent(Pot);
            }
        }
        if (UIManager.GetInventoryVisible() == false)
        {
            AmountOfSeeds.SetActive(true);
            AmountSeedsText.text = "x" + InventoryManager.GetInventory(_seed).ToString();
        }
        else 
        {
            AmountOfSeeds.SetActive(false);
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
    /// Este método se llama desde ToolsManager
    /// Dependiendo de la semilla seleccionada inicia un prefab u otro
    /// </summary>
    public void ChangeSeed(int Seed)
    {
        _seed = (Items)Seed;
        if (Seed == (int)Items.CornSeed) _prefab = PrefabSeeds0;
        else if (Seed == (int)Items.LetuceSeed) _prefab = PrefabSeeds1;
        else if (Seed == (int)Items.CarrotSeed) _prefab = PrefabSeeds2;
        else if (Seed == (int)Items.StrawberrySeed) _prefab = PrefabSeeds3;
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


    #endregion




} // class SeedsManager 
// namespace
