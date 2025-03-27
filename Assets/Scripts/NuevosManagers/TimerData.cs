//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public struct CropVariables
{
    public int MaxWaterTime;
    public int MaxGrowthTime;
    public int MaxDeathTime;

    /*
     
     * MAXWATERTIME - El tiempo que debe pasar entre regados
     * MAXGROWTHTIME - El tiempo que pasa entre una fase y otra
     * MAXDEATHTIME - EL tiempo que pasa desde que se debía regar hasta que muere. Si la planta pasa más de MAXDEATHTIME sin regar muere
     
     */
}

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TimerData
{
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private static CropVariables[] CropsData =
    {
    new CropVariables { MaxWaterTime = 1000, MaxGrowthTime = 1000, MaxDeathTime = 3000 },
    new CropVariables { MaxWaterTime = 1000, MaxGrowthTime = 1000, MaxDeathTime = 3000 },
    new CropVariables { MaxWaterTime = 1000, MaxGrowthTime = 1000, MaxDeathTime = 3000 },
    new CropVariables { MaxWaterTime = 1000, MaxGrowthTime = 1000, MaxDeathTime = 3000 }
    };
        

    #endregion
    
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController


    /// <summary>
    /// Método que devuelve el tiempo de riego para un Item
    /// </summary>
    /// <param name="item"></param>
    public static int GetMaxWaterTime (Items item)
    {
        return CropsData[(int)item/2].MaxWaterTime;
    }

    /// <summary>
    /// Método que devuelve el tiempo de muerte para un Item
    /// </summary>
    /// <param name="item"></param>
    public static int GetMaxDeathTime(Items item)
    {
        return CropsData[(int)item/2].MaxDeathTime;
    }

    /// <summary>
    /// Método que devuelve el tiempo de crecimiento para un Item
    /// </summary>
    /// <param name="item"></param>
    public static int GetMaxGrowthTime(Items item)
    {
        return CropsData[(int)item / 2].MaxGrowthTime;
    }
    #endregion


} // class TimerData 
// namespace
