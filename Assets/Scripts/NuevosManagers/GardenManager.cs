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

    [SerializeField] private float GrowthTimer;
    [SerializeField] private float WaterTimer;

    [SerializeField] private float Timer;

    /// <summary>
    /// Timer que converte el tiempo de juego en tiempo real
    /// </summary>
    [SerializeField] private Timer gameTimer;

    /// <summary>
    /// Mejora 0 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel0;

    /// <summary>
    /// Mejora 1 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel1;

    /// <summary>
    /// Mejora 2 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel2;

    /// <summary>
    /// Mejora 3 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel3;

    /// <summary>
    /// Mejora 3 del huerto
    /// </summary>
    [SerializeField] private GameObject GardenLevel4;

    /// <summary>
    /// Ref al gamemanager
    /// </summary>
    [SerializeField] private GameManager GameManager;

    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    //// Tiempo desde el último riego
    //private float _timeSinceLastWatering; 

    Transform[] Plants;

    /// <summary>
    /// Array con los tamaños de huerto
    /// </summary>
    private int[] GardenSize = { 6, 12, 18, 24, 36 };

    ///<summary>
    /// Mejora Actual
    /// </summary>
    [SerializeField] private int UpgradeLevel;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Awake() 
    {
           UpgradeLevel = GameManager.GetGardenUpgrades();
           SetUpgrade(UpgradeLevel);
    }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
    void Update()
    {
        //Debug.Log("GAMETIME" + gameTimer.GetGameTimeInHours());
        Timer = gameTimer.GetGameTimeInHours();

        for (int i = 0; i < GardenData.GetActivePlants(); i++)
        {
            Debug.Log(GardenData.GetPlant(i).Item);

            if (GardenData.GetPlant(i).Active)
            {
                GrowthTimer = gameTimer.GetGameTimeInHours();
                WaterTimer = gameTimer.GetGameTimeInHours();

                if (gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).GrowthTimer >= GardenData.GetMaxGrowthTime((GardenData.GetPlant(i).Item)))
                {
                    Debug.Log("EnteredGrowth");
                    GrowthWarning(GardenData.GetPlant(i), i);
                }

                if ((gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).WaterTimer) >= GardenData.GetMaxWaterTime((GardenData.GetPlant(i).Item)))
                {
                    Debug.Log("EnteredRiego");
                    WaterWarning(GardenData.GetPlant(i));
                }


                //else if (gameTimer.GetGameTimeInMinutes() - GardenData.GetPlant(i).GrowthTimer >= GardenData.GetMaxDeathTime((GardenData.GetPlant(i).Item)))
                //{
                //    DeathWarning(GardenData.GetPlant(i));
                //}


            }
        }
        if (GameManager == null)
        {
            GameObject ObjetoGameManager = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoGameManager != null)
            {
                GameManager = ObjetoGameManager.GetComponent<GameManager>();
            }
        }
        if (gameTimer == null)
        {
            GameObject ObjetoGameManager = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoGameManager != null)
            {
                gameTimer = ObjetoGameManager.GetComponent<Timer>();
            }
        }
        if (GameManager.GetGardenUpgrades() > UpgradeLevel)
        {
            UpgradeLevel++;
            SetUpgrade(UpgradeLevel);
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
        Debug.Log("GardenManager");
        int i = 0;
        while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());//GardenData.GetMaxWaterTime(GardenData.GetPlant(i).Item));
            Debug.Log("WaterTimer: " + GardenData.GetPlant(i).WaterTimer);
            Debug.Log("GrowthTimer: " + GardenData.GetPlant(i).GrowthTimer);

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
            Debug.Log("GrowthTimer: " + GardenData.GetPlant(i).GrowthTimer);
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
            InventoryManager.ModifyInventory(GardenData.GetPlant(i).Item, 1);
            Destroy(transform);
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

        // METODO MUERTE

        //int i = 0;
        //while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != Crop.position)
        //{
        //    i++;
        //}

        //if (GardenData.GetPlant(i).Position == transform.position)
        //{
        //    GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());
        //}
    }

    /// <summary>
    /// Método para avisar del crecimiento
    /// </summary>
    public void GrowthWarning(Plant plant, int ArrayIndex)
    {
        Debug.Log("Plant Growing");
        int State = plant.State;
        Transform Crop = SearchPlant(plant);

        GardenData.ModifyState(ArrayIndex, GardenData.GetPlant(ArrayIndex).State + 1);
        GardenData.ModifyGrowthTimer(ArrayIndex, gameTimer.GetGameTimeInHours());

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(State);

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

        Transform transform = PlantingSpots.transform.GetChild(plant.Child).transform.GetChild(0);
        return transform;

    }

    public void Init()
    {
        GardenData.ModifyWaterTimer(GardenData.GetActivePlants(), gameTimer.GetGameTimeInHours());
        GardenData.ModifyGrowthTimer(GardenData.GetActivePlants(), gameTimer.GetGameTimeInHours());
    }

    public void SetUpgrade(int Level)
    {
        if (Level == 0)
        {
            GardenLevel0.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);

        }
        else if (Level == 1)
        {
            GardenLevel1.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = GardenSize[UpgradeLevel - 1]; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);

        }
        else if (Level == 2)
        {
            GardenLevel2.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = GardenSize[UpgradeLevel - 1]; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel3.SetActive(false);
            GardenLevel4.SetActive(false);


        }
        else if (Level == 3)
        {
            GardenLevel3.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = GardenSize[UpgradeLevel - 1]; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel4.SetActive(false);


        }
        else if (Level == 4)
        {
            GardenLevel4.SetActive(true);

            Plants = new Transform[PlantingSpots.transform.childCount];
            for (int i = GardenSize[UpgradeLevel-1]; i < GardenSize[UpgradeLevel]; i++)
            {
                Plants[i] = PlantingSpots.transform.GetChild(i).transform;
                Plants[i].gameObject.SetActive(true);
            }

            GardenLevel0.SetActive(false);
            GardenLevel1.SetActive(false);
            GardenLevel2.SetActive(false);
            GardenLevel3.SetActive(false);

        }
    }
    public int GetGardenSize()
    {
        return GardenSize[UpgradeLevel];
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    ///<summary>
    ///Mertodo para inicializar el huerto
    /// </summary>
    private void InitGarden()
    {
        SetUpgrade(GameManager.GetGardenUpgrades());
    }
    #endregion

} // class TimerManager
