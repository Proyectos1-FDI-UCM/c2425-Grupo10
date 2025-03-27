//---------------------------------------------------------
// Este Script es un borrador provisional del Manager del Huerto que maneja todas las variables de todas las plantas del huerto
// Julia Vera Ruiz
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public struct Plant
{
    public Items Item;
    public bool Active;
    public float WaterTimer;
    public float GrowthTimer;
    public int State;
    public Vector3 Position;
    public int Child;

    /* 

     * Items - Tipo de cultivo
     * ACTIVE - activa si TRUE, sin activar si FALSE
     * WATERTIMER - La planta lleva WATERTIME tiempo sin ser regada
     * GROWTHTIMER - El tiempo desde que cambió de fase
     * STATE - La planta tiene n estados

             * Semilla = 0
             * Fase1 = 1
             * Fase2 = 2
             * Fase3 = 3
             * MuerteFase1 = -1
             * MuerteFase2 = -2
             * MuerteFase3 = -3
             
     * POSITION - Posicion en el juego
     * CHILD - Que hijo es la planta (Posicion en la carpeta)
     
    */
}

/// <summary>
/// GardenManager es una clase que sirve para guardar todos los valores de cada planta
/// </summary>
public static class GardenManager
{
    private static int GardenMax = 6; // Se cambia con cada mejora
    private static Plant[] Garden = new Plant[GardenMax];
    private static int ActivePlants = 0;

    static void Main()
    {
        for (int i = 0; i < GardenMax; i++)
        {
            Garden[i].Active = false;
        }

    }

    /// <summary>
    /// Activa una planta con los valores de transform de la planta y el tipo de cultivo
    /// </summary>
    public static void Active(Transform transform, Items item)
    {
        Garden[ActivePlants].Position = transform.position;
        Garden[ActivePlants].Active = true;
        Garden[ActivePlants].Item = item;
        Garden[ActivePlants].State = 1;
        Garden[ActivePlants].Child = transform.parent.transform.GetSiblingIndex(); // Guarda el index de la planta
        Garden[ActivePlants].GrowthTimer = TimerData.GetMaxGrowthTime(item);
        Garden[ActivePlants].WaterTimer = 0;

        ActivePlants++;
    }

    /// <summary>
    /// Desactiva una planta según su posición
    /// </summary>
    public static void Deactivate(Vector3 position)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != position)
        {
            i++;
        }

        if (Garden[i].Position == position)
        {
            Garden[i].Position = Vector3.zero;
            Garden[i].Active = false;
            Garden[i].State = 0;
            Garden[i].WaterTimer = 0;
            Garden[i].GrowthTimer = 0;
            Garden[i].Child =

            ActivePlants--;
        }
    }

    /// <summary>
    /// Riega: Modifica los valores de riego de una planta por su posición
    /// </summary>
    public static void Water(Transform transform)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != transform.position)
        {
            i++;
        }

        if (Garden[i].Position == transform.position)
        {
            Garden[i].WaterTimer = TimerData.GetMaxWaterTime(GardenManager.GetPlant(i).Item);

        }
    }

    /// <summary>
    /// Cultiva: Modifica los valores de cultivo de una planta por su posición (es decir la desactiva)
    /// </summary>
    public static void Harvest(Transform transform)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != transform.position)
        {
            i++;
        }

        if (Garden[i].Position == transform.position)
        {
            Garden[i].Position = Vector3.zero;
            Garden[i].Active = false;
            Garden[i].State = 0;
            Garden[i].WaterTimer = 0;
            Garden[i].GrowthTimer = 0;
            Garden[i].Child =

            ActivePlants--;

        }

    }

    /// <summary>
    /// Crece: Modifica los valores de crecimiento de una planta por su posición 
    /// </summary>
    public static void Grown(Transform transform)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != transform.position)
        {
            i++;
        }

        if (Garden[i].Position == transform.position)
        {
            Garden[i].State++;
            Garden[i].GrowthTimer = TimerData.GetMaxGrowthTime(GardenManager.GetPlant(i).Item);
        }
    }

    /// <summary>
    /// Modifica el timer de Riego de una planta
    /// </summary>
    public static void ModifyWaterTimer(int i, float value)
    {
        Garden[i].WaterTimer += value;
    }

    /// <summary>
    /// Modifica el timer de Crecimiento de una planta
    /// </summary>
    public static void ModifyGrowthTimer(int i, float value)
    {
        Garden[i].GrowthTimer += value;
    }

    /// <summary>
    /// Devuelve el timer de Crecimiento de una planta
    /// </summary>
    public static Plant GetPlant(int i)
    {
        return Garden[i];
    }

    /// <summary>
    /// Modifica el timer de Crecimiento de una planta
    /// </summary>
    public static int GetActivePlants()
    {
        return ActivePlants;
    }

} // class GardenManager 
// namespace
