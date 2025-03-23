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

    /// <summary>
    /// Sprites de Estados de las Plantas Vivas
    /// </summary>

    [SerializeField] private Sprite PlantStage1;
    [SerializeField] private Sprite PlantStage2;
    [SerializeField] private Sprite PlantStage3;
    [SerializeField] private Sprite PlantStage4;

    /// <summary>
    /// Sprites de Estados de las Plantas Muertas
    /// </summary>

    [SerializeField] private Sprite PlantDeadStage1;
    [SerializeField] private Sprite PlantDeadStage2;
    [SerializeField] private Sprite PlantDeadStage3;
    [SerializeField] private Sprite PlantDeadStage4;

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
        GardenManager.Active(transform.position, item); // Inicializa la Planta en GardenManager
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
