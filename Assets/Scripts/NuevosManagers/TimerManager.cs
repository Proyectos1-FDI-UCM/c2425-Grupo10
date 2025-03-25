//---------------------------------------------------------
// Es un Timer que convierte el tiempo real en tiempo de juego, maneja los cutlivos y sus procesos
// Alexia Pérez Santana
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Este script gestiona el tiempo de juego, incluyendo el crecimiento, 
/// el riego y la marchitación de los cultivos.
/// </summary>
public class TimerManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject PlantingSpots;

    // Tiempo en minutos para un día en el juego
    [SerializeField] private float _dayInGameMinutes = 6f;

    // Tiempo en segundos para una hora en el juego
    [SerializeField] private float _hourInGameSeconds = 15f;

    // Tiempo de marchitación en minutos
    [SerializeField] private float _witherTimeMinutes = 1f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    // Tiempo acumulado en el juego
    private float _timeInGame;

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
        _timeInGame = 0f;
        //_timeSinceLastWatering = 0f;

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
        // Aumentar el tiempo en el juego basado en el tiempo real
        _timeInGame += Time.deltaTime / (360f / (_dayInGameMinutes * 60f)); // 3600 segundos en 1 hora
        //_timeSinceLastWatering += Time.deltaTime / 60f; // Tiempo en minutos

        // Aquí puedes añadir la lógica para el crecimiento y marchitación de los cultivos

        for (int i = 0; i < GardenManager.ActivePlants; i++)
        {
            Plant plant = GardenManager.Garden[i]; // Obtener la planta actual

            if (plant.Active)
            {
                GardenManager.Garden[i].WaterTimer -= _timeInGame;
                GardenManager.Garden[i].GrowthTimer -= _timeInGame;

                if (plant.WaterTimer <= -GardenManager.Data[0].MaxDeathTime) DeathWarning(plant); 
                if (plant.WaterTimer <= 0) WaterWarning(plant);

                if(plant.GrowthTimer <= 0) Growth(plant);

                Debug.Log(GardenManager.Garden[i].WaterTimer);
                //Debug.Log(GardenManager.Garden[i].GrowthTimer);

            }
        }

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void Growth(Plant plant)
    {
        int State = plant.State;
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(State);

        }
    }

    /// <summary>
    /// Método para avisar del riego
    /// </summary>
    public void WaterWarning(Plant plant)
    {
       Transform Crop = SearchPlant(plant);

        if (Crop != null) 
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Water");
        }
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

        Transform Plant = PlantingSpots.transform.GetChild(plant.Child).transform.GetChild(0);
        return Plant;

    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Método para comprobar si los cultivos se han marchitado.
    /// </summary>
    private void CheckWithering()
    {
       // Lógica para marchitar cultivos
        
    }

    #endregion   

} // class TimerManager
