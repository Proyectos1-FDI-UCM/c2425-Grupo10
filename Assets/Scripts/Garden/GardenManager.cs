//---------------------------------------------------------
// Maneja los cutlivos y sus procesos
// Alexia Pérez Santana y Julia Vera
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Este script gestiona los cultivos, 
/// Tiene Métodos de Acción (Water, Harvest...) que dependen de herramientas
/// Métodos de avisos (WaterWarning, DeathWarning...) que dependen del Timer
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

    /// <summary>
    /// Carpeta con todas las posiciones en las que el jugador puede plantar
    /// </summary>
    [SerializeField] private GameObject[] Prefabs = new GameObject[(int)Items.Count/2];
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
        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            //Debug.Log(GardenData.GetPlant(i).Item);

            if (GardenData.GetPlant(i).Active)
            {

                if(GardenData.GetPlant(i).State == 0)
                {
                    GardenData.ModifyState(i, 1);
                    GardenData.ModifyGrowthTimer(i, gameTimer.GetGameTimeInHours());
                }

                // Lógica Crecimiento
                if (gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).GrowthTimer >= GardenData.GetMaxGrowthTime((GardenData.GetPlant(i).Item)) && GardenData.GetPlant(i).State < 5 && GardenData.GetPlant(i).State > 0)
                {
                    GrowthWarning(GardenData.GetPlant(i), i);
                }

                // Lógica Aviso Cosecha y Aviso Riego / Muerte
                 if ((gameTimer.GetGameTimeInHours() - GardenData.GetPlant(i).WaterTimer) >= GardenData.GetMaxWaterTime((GardenData.GetPlant(i).Item)) && GardenData.GetPlant(i).State < 4 && GardenData.GetPlant(i).State > 0 && !GardenData.GetPlant(i).WaterWarning)
                {
                    WaterWarning(GardenData.GetPlant(i));
                    GardenData.ModifyWaterWarning(i);
                }
                else if (GardenData.GetPlant(i).State > 4)
                {
                    HarvestWarning(GardenData.GetPlant(i));
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
        int i = 0;
        while (i < GardenSize[UpgradeLevel] && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }

        if (GardenData.GetPlant(i).Position == transform.position)
        {
            GardenData.ModifyWaterTimer(i, gameTimer.GetGameTimeInHours());
            GardenData.ModifyWaterWarning(i);
        }
    }

    /// <summary>
    /// Cultiva: Modifica los valores de cultivo de una planta por su posición (es decir la desactiva)
    /// </summary>
    public void Harvest(Transform transform)
    {
        int i = 0;
        while (i < GardenSize[UpgradeLevel] && GardenData.GetPlant(i).Position != transform.position)
        {
            i++;
        }
        Plant Plant = GardenData.GetPlant(i);

        if (Plant.Position == transform.position)
        {
            Debug.Log(Plant.State);
            if (Plant.State > 3)
            {
                int random = UnityEngine.Random.Range(0, _maxProb);
                InventoryManager.ModifyInventory(GardenData.GetPlant(i).Item, 1);
                CropSpriteEditor cropSpriteEditor = transform.GetChild(0).GetComponent<CropSpriteEditor>();
                if (random == 0) 
                {
                    GardenData.ModifyState(i, (-5));
                    cropSpriteEditor.Growing(-5);
                    cropSpriteEditor.Warning("Desactivate");
                }
                else 
                {
                    GardenData.Deactivate(transform.position);
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
        while (i < GardenSize[UpgradeLevel] && GardenData.GetPlant(i).Position != transform.position)
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

    /// <summary>
    /// Método que carga los valores de las plantas entre escenas
    /// </summary>
    public void InitChangeScene()
    {
        for (int i = 0; i < GardenSize[UpgradeLevel]; i++)
        {
            Plant plant = GardenData.GetPlant(i);
            if (plant.Active) 
            {
                GameObject Prefab = Prefabs[(int)plant.Item%((int)Items.Count/2)];
                GameObject Crop = Instantiate(Prefab, plant.Position, Quaternion.identity);
                Crop.transform.parent = PlantingSpots.transform.GetChild(plant.Child);

                CropSpriteEditor cropSpriteEditor = Crop.GetComponent<CropSpriteEditor>();

                if (cropSpriteEditor != null) 
                { 
                    cropSpriteEditor.Growing(plant.State);
                    cropSpriteEditor.Warning("Desactivate");
                }
            }
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

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
        Transform transform = null;
        if (PlantingSpots.transform.childCount >= plant.Child)
        {
            if(PlantingSpots.transform.GetChild(plant.Child).transform.childCount > 0) 
                {
                transform = PlantingSpots.transform.GetChild(plant.Child).transform.GetChild(0);
            }
        }
        return transform;

    }

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
