//---------------------------------------------------------
// Este Script guarda los datos de todas las plantas del huerto
// Julia Vera Ruiz
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using

[Serializable]
public struct Plant
{
    public Items Item;
    public bool Active;
    public float WaterTimer;
    public float GrowthTimer;
    public bool WaterWarning;
    public bool DeathWarning;
    public bool HarvestWarning;
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
             * Fase4 = 4
             * MuerteFase1 = -1
             * MuerteFase2 = -2
             * MuerteFase3 = -3
             * MuerteFase3 = -4
             * Mala Hierba = -5
             
     * POSITION - Posicion en el juego
     * CHILD - Que hijo es la planta (Posicion en la carpeta)
     
    */
}

public struct CropVariables
{
    public float MaxWaterTime;
    public float MaxGrowthTime;
    public float MaxDeathTime;

    /*
     
     * MAXWATERTIME - El tiempo que debe pasar entre regados
     * MAXGROWTHTIME - El tiempo que pasa entre una fase y otra
     * MAXDEATHTIME - El tiempo que pasa desde que se debía regar hasta que muere. Si la planta pasa más de MAXDEATHTIME sin regar muere
     
     */
}


/// <summary>
/// GardenData es una clase que sirve para guardar todos los valores de cada planta
/// </summary>
public static class GardenData
{

    private static int GardenMax = 36; // Se cambia con cada mejora
    private static Plant[] Garden = new Plant[GardenMax];
    private static int ActivePlants = 0;

    private static CropVariables[] CropsData =
    {
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 1.8f, MaxDeathTime = 2.3f }, //maiz
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 0.3f, MaxDeathTime = 0.8f },//lechuga
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 0.8f, MaxDeathTime = 1f }, //zanahoria
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 1.3f, MaxDeathTime = 1.5f } //fresa
    };

    public static Plant[] GetGarden()
    {
        Plant[] garden = new Plant[GardenMax];
        for (int i = 0; i < garden.Length; i++)
        {
            //Garden[i].Active = false;
            garden[i] = Garden[i];
        }
        return garden;
    }
    public static void SetGarden(Plant[] garden)
    {
        for (int i = 0; i < garden.Length; i++)
        {
            Garden[i] = garden[i];
        }
    }

    public static void ResetGarden()
    {
        for (int i = 0; i < Garden.Length; i++)
        {
            Garden[i].Active = false;
        }
    }

    public static void SetActivePlants(int activePlants)
    {
        ActivePlants = activePlants;
    }

    /// <summary>
    /// Devuelve el numero (int) de plantas activas
    /// </summary>
    public static int GetActivePlants()
    {
        return ActivePlants;
    }

    static void Main()
    {
        for (int i = 0; i < GardenMax; i++)
        {
            if (Garden[i].Active == null) Garden[i].Active = false;
        }

    }

    /// <summary>
    /// Activa una planta con los valores de transform de la planta y el tipo de cultivo
    /// </summary>
    public static void Active(Transform transform, Items item)
    {
        int i = 0;
        bool PlantActive = true;
        while (i < GardenMax && PlantActive)
        {
            if (Garden[i].Active)
            {
                i++;
            }
            else
            {
                PlantActive = false;
            }
        }
        if (!PlantActive) { 

            Garden[i].Position = transform.position;
            Garden[i].Active = true;
            Garden[i].Item = item;
            Garden[i].State = 0;
            Garden[i].WaterWarning = false;
            Garden[i].DeathWarning = false;
            Garden[i].HarvestWarning = false;
            Garden[i].Child = transform.parent.transform.GetSiblingIndex(); // Guarda el index de la planta
            Garden[i].GrowthTimer = 0;

            ActivePlants++;
            Debug.Log($"Planta creada Array: {i} y Pot: {Garden[i].Child}, ");
        }
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
            Garden[i].WaterWarning = false;
            Garden[i].DeathWarning = false;
            Garden[i].HarvestWarning = false;
            Garden[i].Child =

            ActivePlants--;
        }
        else Debug.Log("No se elimina la planta");
    }

    /// <summary>
    /// Desactiva una planta según su idenx en el array
    /// </summary>
    public static void Deactivate(int i)
    {

            Garden[i].Position = Vector3.zero;
            Garden[i].Active = false;
            Garden[i].State = 0;
            Garden[i].WaterTimer = 0;
            Garden[i].GrowthTimer = 0;
            Garden[i].WaterWarning = false;
            Garden[i].DeathWarning = false;
            Garden[i].HarvestWarning = false;
            Garden[i].Child =

            ActivePlants--;

    }

    /// <summary>
    /// Modifica el timer de Riego de una planta
    /// </summary>
    public static void ModifyWaterTimer(int i, float value)
    {
        Garden[i].WaterTimer = value;
    }

    /// <summary>
    /// Modifica el timer de Crecimiento de una planta
    /// </summary>
    public static void ModifyGrowthTimer(int i, float value)
    {
        Debug.Log($"GrowthTimer: {Garden[i].GrowthTimer}");
        Garden[i].GrowthTimer = value;
    }

    /// <summary>
    /// Modifica el estado de una planta
    /// </summary>
    public static void ModifyState(int i, int State)
    {
        Garden[i].State = State;
    }

    /// <summary>
    /// Modifica el bool de Aviso Riego
    /// </summary>
    public static void ModifyWaterWarning(int i, bool b)
    {
        Garden[i].WaterWarning = b;
        if (b = true)
        {
            Garden[i].DeathWarning = false;
            Garden[i].HarvestWarning = false;
        }
    }

    /// <summary>
    /// Modifica el bool de Aviso Muerte
    /// </summary>
    public static void ModifyDeathWarning(int i, bool b)
    {
        Garden[i].DeathWarning = b;
        if (b = true)
        {
            Garden[i].WaterWarning = false;
            Garden[i].HarvestWarning = false;
        }
    }

    /// <summary>
    /// Modifica el bool de Aviso Cosecha
    /// </summary>
    public static void ModifyHarvestWarning(int i, bool b)
    {
        Garden[i].HarvestWarning = b;
        if (b = true)
        {
            Garden[i].DeathWarning = false;
            Garden[i].WaterWarning = false;
        }
    }

    /// <summary>
    /// Devuelve la planta Plant, en la posición i del array
    /// </summary>
    public static Plant GetPlant(int i)
    {
        if (i < Garden.Length) return Garden[i];
        else return new Plant();
    }

    /// <summary>
    /// Devuelve la planta Plant, por el child
    /// </summary>
    public static Plant GetPlantChild(int Child)
    {
        Plant Plant = new Plant();
        bool Found = false;
        int i = 0;
        while (i < Garden.Length && !Found)
        {
            if (Garden[i].Child == Child) Found = true;
            i++;
        }
        if (Found) Plant = Garden[i];
        return Garden[i];
    }

    /// <summary>
    /// Devuelve la planta Plant, en la posición transform
    /// </summary>
    public static Plant GetPlant(Transform transform)
    {
        Plant Plant = new Plant();
        bool Found = false;
        int i = 0;
        while (i < Garden.Length && !Found)
        {
            if (Garden[i].Position == transform.position) Found = true;
           i++;
        }
        if (i < Garden.Length && Found) Plant = Garden[i];
        return Plant;
    }

    // ---------------- ITEM DATA ------------------

    /// <summary>
    /// Método que devuelve el tiempo de riego para un Item
    /// </summary>
    /// <param name="item"></param>
    public static float GetMaxWaterTime(Items item)
    {
        return CropsData[(int)item / ((int)Items.Count / 2)].MaxWaterTime;
    }

    /// <summary>
    /// Método que devuelve el tiempo de muerte para un Item
    /// </summary>
    /// <param name="item"></param>
    public static float GetMaxDeathTime(Items item)
    {
        return CropsData[(int)item / ((int)Items.Count/2)].MaxDeathTime;
    }

    /// <summary>
    /// Método que devuelve el tiempo de crecimiento para un Item
    /// </summary>
    /// <param name="item"></param>
    public static float GetMaxGrowthTime(Items item)
    {
        return CropsData[(int)item / ((int)Items.Count / 2)].MaxGrowthTime;
    }


} // class GardenManager 
// namespace
