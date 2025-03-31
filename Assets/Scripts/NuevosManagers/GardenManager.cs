//---------------------------------------------------------
// Maneja los cutlivos y sus procesos
// Alexia Pérez Santana y Julia Vera
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Este script gestiona los cultivos, 
/// incluyendo el crecimiento, el regado, la muerte y sus avisos. 
/// </summary>
public class GardenManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    /// <summary>
    /// Timer que converte el tiempo de juego en tiempo real
    /// </summary>
    [SerializeField] private Timer gameTimer;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    //// Tiempo desde el último riego
    //private float _timeSinceLastWatering; 

    Transform[] Plants;

#endregion

// ---- MÉTODOS DE MONOBEHAVIOUR ----
#region Métodos de MonoBehaviour

/// <summary>
/// Start is called on the frame when a script is enabled just before 
/// any of the Update methods are called the first time.
/// </summary>
void Start()
    {

        Plants = new Transform[PlantingSpots.transform.childCount]; // Inicia el tamaño del array al tamaño del total de hijos de la carpeta PlantingSpots
        for (int i = 0; i < PlantingSpots.transform.childCount; i++)
        {
            Plants[i] = PlantingSpots.transform.GetChild(i).transform; // Establece en el array todos los transforms de los lugares para plantar (dentro de la carpeta PlantingSpots)
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        for (int i = 0; i <= GardenData.GetActivePlants(); i++)
        {
            if (GardenData.GetPlant(i).Active)
            {
                if ((gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).WaterTimer) == GardenData.GetMaxWaterTime((GardenData.GetPlant(i).Item)))
                {
                    Debug.Log("EnteredRiego");
                    WaterWarning(GardenData.GetPlant(i));
                }
                //else if (gameTimer.GetGameTimeInMinutes() - GardenData.GetPlant(i).GrowthTimer >= GardenData.GetMaxDeathTime((GardenData.GetPlant(i).Item)))
                //{
                //    DeathWarning(GardenData.GetPlant(i));
                //}

                if (gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).GrowthTimer == GardenData.GetMaxGrowthTime((GardenData.GetPlant(i).Item)))
                {
                    Debug.Log("EnteredGrowth");
                    GrowthWarning(GardenData.GetPlant(i));
                }
                Debug.Log(GardenData.GetPlant(i).WaterTimer);
            }
            
        }

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Riega: Modifica los valores de riego de una planta por su posición
    /// </summary>
    public void Water(Transform transform)
    {
        Debug.LogError("WaterWarning");
        int i = 0;
        while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());//GardenData.GetMaxWaterTime(GardenData.GetPlant(i).Item));
            if (GardenData.GetPlant(i).State == 1)
            {
                GardenData.ModifyGrowthTimer(i, gameTimer.GetGameTimeInHours());
            }

        }
    }

    /// <summary>
    /// Crece: Modifica los valores de crecimiento de una planta por su posición 
    /// </summary>
    public void Grown(Transform transform)
    {
        int i = 0;
        while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.ModifyState(i, GardenData.GetPlant(i).State + 1);
            GardenData.ModifyGrowthTimer(i, gameTimer.GetGameTimeInHours());
        }
    }

    /// <summary>
    /// Cultiva: Modifica los valores de cultivo de una planta por su posición (es decir la desactiva)
    /// </summary>
    public void Harvest(Transform transform)
    {
        int i = 0;
        while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.Deactivate(transform.position);
        }

    }

    /// <summary>
    /// Método para avisar del riego
    /// </summary>
    public void WaterWarning(Plant plant)
    {
        Debug.Log("WateringWarning");
       Transform Crop = SearchPlant(plant);

        if (Crop != null) 
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Water");
        }
    }

    /// <summary>
    /// Método para avisar del crecimiento
    /// </summary>
    public void GrowthWarning(Plant plant)
    {
        Debug.Log("GrowthWarning");
        int State = plant.State;
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(State);


        }
        Grown(PlantingSpots.transform.GetChild(plant.Child).transform);
    }

    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void DeathWarning(Plant plant)
    {
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Death");
        }
    }

    /// <summary>
    /// Método para buscar la posición de la planta correcta
    /// </summary>
    public Transform SearchPlant(Plant plant)
    {

        Transform transform = PlantingSpots.transform.GetChild(plant.Child).transform.GetChild(0);
        return transform;

    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion   

} // class TimerManager
