//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Julia Vera Ruiz
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

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
    /// Prefab de las Seeds (Este es el inicial, añadiré todos en los siguientes commits)
    /// </summary>
    [SerializeField] private GameObject PrefabSeeds;

    /// <summary>
    /// Distancia mínima a la que debe estar el jugador del lugar disponible para plantar
    /// </summary>
    [SerializeField] private float InitialMinDistance;

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
    private Transform[] Pots;

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
        Pots = new Transform[PlantingSpots.transform.childCount]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < PlantingSpots.transform.childCount; i++)
        {
            Pots[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.UsarWasPressedThisFrame())
        {
            Transform Pot = FindNearestPot(transform, Pots); // Busca un lugar disponible para plantar
            if (Pot != null) // Si lo encuentra, instancia el prefab de la semilla seleccionada
            {
                GameObject Plant = Instantiate(PrefabSeeds, Pot.position, Quaternion.identity);
                Plant.transform.SetParent(Pot);
            }
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
            if (pot.childCount == 0) // Comprueba que no hay ninguna planta en esta posición
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
