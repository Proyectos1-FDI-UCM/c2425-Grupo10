//---------------------------------------------------------
// Este Script guarda los datos de todas las plantas del huerto
// Julia Vera Ruiz, Alexia Pérez Santana
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
    private static bool _isFastTimeMode = false;

    private static int _gardenMax = 36; // Se cambia con cada mejora
    private static Plant[] _garden = new Plant[_gardenMax];
    private static int _activePlants = 0;

    private static CropVariables[] CropsData =
    {
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 2f, MaxDeathTime = 1.8f }, //maiz
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 0.4f, MaxDeathTime = 0.2f },//lechuga
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 1f, MaxDeathTime = 0.8f }, //zanahoria
    new CropVariables { MaxWaterTime = 0.5f, MaxGrowthTime = 1.5f, MaxDeathTime = 1.3f } //fresa
    };

    /// <summary>
    /// Devuelve todos los datos del garden (Array de Plants)
    /// </summary>
    public static Plant[] GetGarden()
    {
        Plant[] garden = new Plant[_gardenMax];
        for (int i = 0; i < garden.Length; i++)
        {
            //Garden[i].Active = false;
            garden[i] = _garden[i];
        }
        return garden;
    }

    /// <summary>
    /// Inicianiza el Garden
    /// </summary>
    public static void SetGarden(Plant[] garden)
    {
        for (int i = 0; i < garden.Length; i++)
        {
            _garden[i] = garden[i];
        }
    }

    /// <summary>
    /// Resetea el Garden todas las plantas desactivadas
    /// </summary>
    public static void ResetGarden()
    {
        for (int i = 0; i < _garden.Length; i++)
        {
            _garden[i].Active = false;
        }
    }

    /// <summary>
    /// Al cargar partida, establece las plantas activas
    /// </summary>
    public static void SetActivePlants(int activePlants)
    {
        _activePlants = activePlants;
    }

    /// <summary>
    /// Devuelve el numero (int) de plantas activas
    /// </summary>
    public static int GetActivePlants()
    {
        return _activePlants;
    }

    static void Main()
    {
        for (int i = 0; i < _gardenMax; i++)
        {
            if (_garden[i].Active == null) _garden[i].Active = false;
        }

    }

    /// <summary>
    /// Activa una planta con los valores de transform de la planta y el tipo de cultivo
    /// </summary>
    public static void Active(Transform transform, int item)
    {
        int i = 0;
        bool PlantActive = true;
        while (i < _gardenMax && PlantActive)
        {
            if (_garden[i].Active)
            {
                i++;
            }
            else
            {
                PlantActive = false;
            }
        }
        if (!PlantActive) { 

            _garden[i].Position = transform.position;
            _garden[i].Active = true;
            _garden[i].Item = (Items)item;
            _garden[i].State = 0;
            _garden[i].WaterWarning = false;
            _garden[i].DeathWarning = false;
            _garden[i].HarvestWarning = false;
            _garden[i].Child = transform.GetSiblingIndex(); // Guarda el index de la planta
            _garden[i].GrowthTimer = 0;

            _activePlants++;
            Debug.Log($"Planta creada Array: {i} y Pot: {_garden[i].Child}, Type: {_garden[i].Item.ToString()}");
        }
    }

    /// <summary>
    /// Desactiva una planta según su posición
    /// </summary>
    public static void Deactivate(Vector3 position)
    {
        int i = 0;
        while (i < _activePlants && _garden[i].Position != position)
        {
            i++;
        }

        if (_garden[i].Position == position)
        {
            _garden[i].Position = Vector3.zero;
            _garden[i].Active = false;
            _garden[i].State = 0;
            _garden[i].WaterTimer = 0;
            _garden[i].GrowthTimer = 0;
            _garden[i].WaterWarning = false;
            _garden[i].DeathWarning = false;
            _garden[i].HarvestWarning = false;
            _garden[i].Child =

            _activePlants--;
        }
        else Debug.Log("No se elimina la planta");
    }

    /// <summary>
    /// Desactiva una planta según su idenx en el array
    /// </summary>
    public static void Deactivate(int i)
    {

            _garden[i].Position = Vector3.zero;
            _garden[i].Active = false;
            _garden[i].State = 0;
            _garden[i].WaterTimer = 0;
            _garden[i].GrowthTimer = 0;
            _garden[i].WaterWarning = false;
            _garden[i].DeathWarning = false;
            _garden[i].HarvestWarning = false;
            _garden[i].Child =

            _activePlants--;

    }

    /// <summary>
    /// Modifica el timer de Riego de una planta
    /// </summary>
    public static void ModifyWaterTimer(int i, float value)
    {
        _garden[i].WaterTimer = value;
    }

    /// <summary>
    /// Modifica el timer de Crecimiento de una planta
    /// </summary>
    public static void ModifyGrowthTimer(int i, float value)
    {
        Debug.Log($"GrowthTimer: {_garden[i].GrowthTimer}");
        _garden[i].GrowthTimer = value;
    }

    /// <summary>
    /// Modifica el estado de una planta
    /// </summary>
    public static void ModifyState(int i, int State)
    {
        _garden[i].State = State;
    }

    /// <summary>
    /// Modifica el bool de Aviso Riego
    /// </summary>
    public static void ModifyWaterWarning(int i, bool b)
    {
        _garden[i].WaterWarning = b;
        if (b == true)
        {
            _garden[i].DeathWarning = false;
            _garden[i].HarvestWarning = false;
        }
    }

    /// <summary>
    /// Modifica el bool de Aviso Muerte
    /// </summary>
    public static void ModifyDeathWarning(int i, bool b)
    {
        _garden[i].DeathWarning = b;
        if (b == true)
        {
            _garden[i].WaterWarning = false;
            _garden[i].HarvestWarning = false;
        }
    }

    /// <summary>
    /// Modifica el bool de Aviso Cosecha
    /// </summary>
    public static void ModifyHarvestWarning(int i, bool b)
    {
        _garden[i].HarvestWarning = b;
        if (b == true)
        {
            _garden[i].DeathWarning = false;
            _garden[i].WaterWarning = false;
        }
    }

    /// <summary>
    /// Devuelve la planta Plant, en la posición i del array
    /// </summary>
    public static Plant GetPlant(int i)
    {
        if (i < _garden.Length) return _garden[i];
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
        while (i < _garden.Length && !Found)
        {
            if (_garden[i].Child == Child) Found = true;
            i++;
        }
        if (Found) Plant = _garden[i];
        return _garden[i];
    }

    /// <summary>
    /// Devuelve la planta Plant, en la posición transform
    /// </summary>
    public static Plant GetPlant(Transform transform)
    {
        Plant Plant = new Plant();
        bool Found = false;
        int i = 0;
        while (i < _garden.Length && !Found)
        {
            if (_garden[i].Position == transform.position) Found = true;
           i++;
        }
        if (i < _garden.Length && Found) Plant = _garden[i];
        return Plant;
    }

    /// <summary>
    /// Devuelve el índice de una planta concreta en el array garden
    /// </summary>
    public static int IndexGarden(Plant plant)
    {
        int Index = System.Array.IndexOf(_garden, plant);
        return Index;
    }

    /// <summary>
    /// Activa / desactiva el cheat del tiempo
    /// </summary>
    /// <param name="isFastTime"></param>
    public static void SetFastTimeMode(bool isFastTime)
    {
        _isFastTimeMode = isFastTime;
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
