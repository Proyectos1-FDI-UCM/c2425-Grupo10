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

    ///<summary>
    /// Mejora Actual
    /// </summary>
    [SerializeField] private int UpgradeLevel;
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

    /// <summary>
    /// Probabilidad de que aparezca mala hierbas
    /// </summary>
    private int _maxProb = 5;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Awake() 
    {
        InitializeReferences();
        GameManager.InitializeGardenManager();
        UpgradeLevel = GameManager.GetGardenUpgrades();
        SetUpgrade(UpgradeLevel);
    }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
    void Update()
    {
        Timer = gameTimer.GetGameTimeInHours();

        for (int i = 0; i < GardenData.GetActivePlants(); i++)
        {
            Debug.Log(GardenData.GetPlant(i).Item);

            if (GardenData.GetPlant(i).Active)
            {
                GrowthTimer = gameTimer.GetGameTimeInHours();
                WaterTimer = gameTimer.GetGameTimeInHours();

                if(GardenData.GetPlant(i).State == 0)
                {
                    GardenData.ModifyState(i, 1);
                    GardenData.ModifyGrowthTimer(i, gameTimer.GetGameTimeInHours());
                }

                // Lógica Crecimiento
                if (gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).GrowthTimer >= GardenData.GetMaxGrowthTime((GardenData.GetPlant(i).Item)) && GardenData.GetPlant(i).State < 5 && GardenData.GetPlant(i).State > 0)
                {
                    //Debug.Log("EnteredGrowth");
                    GrowthWarning(GardenData.GetPlant(i), i);
                }

                // Lógica Aviso Cosecha y Aviso Riego / Muerte
                if (GardenData.GetPlant(i).State > 3)
                {
                    HarvestWarning(GardenData.GetPlant(i));
                }
                else if ((gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).WaterTimer) >= GardenData.GetMaxWaterTime((GardenData.GetPlant(i).Item)) && GardenData.GetPlant(i).State < 3 && GardenData.GetPlant(i).State > 0 && !GardenData.GetPlant(i).WaterWarning)
                {
                    //Debug.Log("EnteredWatering");
                    WaterWarning(GardenData.GetPlant(i));
                    GardenData.ModifyWaterWarning(i);
                }
                else if (gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).WaterTimer >= GardenData.GetMaxDeathTime((GardenData.GetPlant(i).Item)) && GardenData.GetPlant(i).State < 3 && GardenData.GetPlant(i).State > 0 && GardenData.GetPlant(i).WaterWarning)
                {
                    DeathWarning(GardenData.GetPlant(i), i);
                }


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
            GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());
            GardenData.ModifyWaterWarning(i);
            //Debug.Log("WaterTimer: " + GardenData.GetPlant(i).WaterTimer);
            //Debug.Log("GrowthTimer: " + GardenData.GetPlant(i).GrowthTimer);

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
        Plant Plant = GardenData.GetPlant(i);

        if (Plant.Position == transform.position)
        {
            Debug.Log(Plant.State);
            if (Plant.State > 3)
            {
                int r = UnityEngine.Random.Range(0, _maxProb);
                CropSpriteEditor cropSpriteEditor = transform.GetChild(0).GetComponent<CropSpriteEditor>();
                if (r == 0) 
                {
                    GardenData.ModifyState(i, (-5));
                    InventoryManager.ModifyInventory(GardenData.GetPlant(i).Item, 1);
                    cropSpriteEditor.Growing(-5);
                    cropSpriteEditor.Warning("Desactivate");
                }
                else 
                {
                    GardenData.Deactivate(transform.position);
                    InventoryManager.ModifyInventory(GardenData.GetPlant(i).Item, 1);
                    cropSpriteEditor.Destroy(); 
                }
            }
        }
    }

    /// <summary>
    /// MalasHierbas: Modifica los valores de la semilla (Elimina)
    /// </summary>
    public void Weed(Transform transform)
    {
        int i = 0;
        while (i < GardenData.GetActivePlants() && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }
        Plant Plant = GardenData.GetPlant(i);

        if (Plant.Position == transform.position)
        {
            if (Plant.State == -5)
            {
                CropSpriteEditor cropSpriteEditor = transform.GetChild(0).GetComponent<CropSpriteEditor>();
                GardenData.Deactivate(transform.position);
                cropSpriteEditor.Destroy();
            }

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
        Transform Crop = SearchPlant(plant);

        GardenData.ModifyState(ArrayIndex, plant.State + 1);
        GardenData.ModifyGrowthTimer(ArrayIndex, gameTimer.GetGameTimeInHours());

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Growing(plant.State);

        }
    }

    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void DeathWarning(Plant plant, int ArrayIndex)
    {
        Debug.Log("DeathWarning");
        GardenData.ModifyState(ArrayIndex, plant.State);
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Death");
        }
    }

    /// <summary>
    /// Método para avisar de la muerte
    /// </summary>
    public void HarvestWarning(Plant plant)
    {
        Transform Crop = SearchPlant(plant);

        if (Crop != null)
        {
            CropSpriteEditor Call = Crop.GetComponent<CropSpriteEditor>();
            Call.Warning("Harvest");
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

    ///<summary>
    ///Metodo para inicializar referencias
    /// </summary>
    private void InitializeReferences()
    {
        if (GameManager == null)
        {
            GameManager = FindObjectOfType<GameManager>();
        }
        if (gameTimer == null)
        {
            gameTimer = FindObjectOfType<Timer>();
        }
    }
    #endregion

} // class TimerManager
