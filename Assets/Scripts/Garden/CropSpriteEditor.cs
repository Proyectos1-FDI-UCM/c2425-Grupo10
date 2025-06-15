//---------------------------------------------------------
// Este Script se encarga de inicializar las plantas y modificar los sprites de los cultivos 
// Julia Vera Ruiz
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// CropSpriteEditor, inicializa las plantas 
/// Modifica los sprites en función del TimerManager
/// </summary>
public class CropSpriteEditor : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Timer gameTimer;
    /// <summary>
    /// Sprites de Estados de las Plantas Vivas
    /// </summary>
    [SerializeField] private Sprite[] Sprites = new Sprite[5];

    /// <summary>
    /// Sprites de Estados de las Plantas Muertas
    /// </summary>
    [SerializeField] private Sprite[] DeadSprites = new Sprite[5];
    //[SerializeField] private Sprite PlantStageWeeds;

    /// <summary>
    /// Sprites de Estados de las Plantas Muertas
    /// </summary>

    [SerializeField] private Sprite WarningWater;
    [SerializeField] private Sprite WarningDeath;
    [SerializeField] private Sprite WarningCollect;

    [SerializeField] private Items item;

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
    /// Sprite Renderer de los avisos
    /// </summary>
    /// 
    private SpriteRenderer _warning;

    /// <summary>
    /// Sprites de Estados de las Plantas
    /// </summary>
    /// 
    private SpriteRenderer _spriteRenderer;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = true;
        _warning = transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
        _warning.enabled = true;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _warning = transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
        if (!GardenData.GetPlant(transform).Active)
        {
            //GardenData.Active(transform, item); // Inicializa la Planta en GardenManager

            //Warning("Water");
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
    /// Cambia los avisos 
    /// </summary>
    public void Warning(string Type)
    {
        if (Type == "Desactivate")
        {
            _warning.enabled = false;
        }
        if (Type == "Water")
        {
            _warning.enabled = true;
            _warning.sprite = WarningWater;
        }
        if (Type == "Death")
        {
            _warning.enabled = true;
            _warning.sprite = WarningDeath;
        }
        if (Type == "Harvest")
        {
            _warning.enabled = true;
            _warning.sprite = WarningCollect;
        }
    }

    /// <summary>
    /// Modifica los valores de crecimiento
    /// </summary>
    public void Growing(int state)
    {
        _spriteRenderer.enabled = true;
        if (state == 0) 
        {
            _spriteRenderer.sprite = Sprites[0];
        }
        else if (state == 1)
        {
            _spriteRenderer.sprite = Sprites[1];
        }
        else if (state == 2)
        {
            _spriteRenderer.sprite = Sprites[2];
        }
        else if (state == 3)
        {
            _spriteRenderer.sprite = Sprites[3];
        }
        else if (state == 4)
        {
            int numero = UnityEngine.Random.Range(0, 1);
            if (numero == 0)
            {
                GardenData.ChangeItem(item);
                _spriteRenderer.sprite = Sprites[5];
            }
            else
                _spriteRenderer.sprite = Sprites[4];
        }
        else if (state == 5)
        {
            
            _spriteRenderer.sprite = Sprites[4];
        }
        else if (state == -1)
        {
            _spriteRenderer.sprite = DeadSprites[1];
        }
        else if (state == -2)
        {
            _spriteRenderer.sprite = DeadSprites[2];
        }
        else if (state == -3)
        {
            _spriteRenderer.sprite = DeadSprites[3];
        }
        else if (state == -4)
        {
            _spriteRenderer.sprite = DeadSprites[3];
        }
        else if (state == -5)
        {
            _spriteRenderer.sprite = DeadSprites[4];
        }
        else if (state == -6)
        {
            _spriteRenderer.sprite = DeadSprites[0];
        }
        //Grown(transform); // Modifica el timer de crecimiento
        // Debug.Log("SpriteChanged" + state);
    }

    public int GetGrowthState()
    {
        return GardenData.GetPlantChild(transform.GetSiblingIndex()).State;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CropSpriteEditor 
// namespace
