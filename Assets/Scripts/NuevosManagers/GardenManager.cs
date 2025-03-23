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
    public int MaxWaterTime;
    public int MaxGrowthTime;
    public int MaxDeathTime;
    public int State;
    public Vector3 Position;
}

/// <summary>
/// GardenManager es una clase que sirve para guardar todos los valores de cada planta
/// </summary>
public static class GardenManager
{

    /* 

     * Items - Tipo de cultivo
     * ACTIVE - activa si TRUE, sin activar si FALSE
     * WATERTIMER - La planta lleva WATERTIME tiempo sin ser regada
     * GROWTHTIMER - El tiempo desde que cambió de fase
     * MAXWATERTIME - El tiempo que debe pasar entre regados
     * MAXGROWTHTIME - El tiempo que pasa entre una fase y otra
     * MAXDEATHTIME - EL tiempo que pasa desde que se debía regar hasta que muere. Si la planta pasa más de MAXDEATHTIME sin regar muere
     * STATE - La planta tiene n estados

             * Semilla = 0
             * Fase1 = 1
             * Fase2 = 2
             * Fase3 = 3
             * MuerteFase1 = -1
             * MuerteFase2 = -2
             * MuerteFase3 = -3
     
    */


    public static int GardenMax = 6; // Se cambia con cada mejora
    public static Plant[] Garden = new Plant[GardenMax];
    public static int ActivePlants = 0;

    static void Method()
    {
        for (int i = 0; i < GardenMax; i++)
        {
            Garden[i].Active = false;
        } 

    }

    public static void Active(Vector3 position, Items item)
    {
        Garden[ActivePlants].Position = position;
        Garden[ActivePlants].Active = true;
        Garden[ActivePlants].Item = item;
        Garden[ActivePlants].State = 1;
        Garden[ActivePlants].WaterTimer = 0;
        Garden[ActivePlants].GrowthTimer = 0;

        ActivePlants++;
    }

    public static void Deactivate(Vector3 position)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != position)
        {
            i++;
        }

        if (Garden[i].Position == position)
        {
            Garden[ActivePlants].Position = Vector3.zero;
            Garden[ActivePlants].Active = false;
            Garden[ActivePlants].State = 0;
            Garden[ActivePlants].WaterTimer = 0;
            Garden[ActivePlants].GrowthTimer = 0;

            ActivePlants--;
        }


    }

} // class GardenManager 
// namespace
