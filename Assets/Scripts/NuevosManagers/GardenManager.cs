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
     
    */
}

public struct CropData
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
/// GardenManager es una clase que sirve para guardar todos los valores de cada planta
/// </summary>
public static class GardenManager
{
    public static int GardenMax = 6; // Se cambia con cada mejora
    public static Plant[] Garden = new Plant[GardenMax];
    
    public static int ActivePlants = 0;

    public static CropData[] Data = new CropData[(int)Items.Count / 2];


    static void Method()
    {
        for (int i = 0; i < GardenMax; i++)
        {
            Garden[i].Active = false;
        }

        #region CropData


        Data[(int)Items.Corn].MaxWaterTime = 0;
        Data[(int)Items.Corn].MaxGrowthTime = 0;
        Data[(int)Items.Corn].MaxDeathTime = 0;

        Data[(int)Items.Letuce].MaxWaterTime = 0;
        Data[(int)Items.Letuce].MaxGrowthTime = 0;
        Data[(int)Items.Letuce].MaxDeathTime = 0;

        Data[(int)Items.Carrot].MaxWaterTime = 0;
        Data[(int)Items.Carrot].MaxGrowthTime = 0;
        Data[(int)Items.Carrot].MaxDeathTime = 0;

        Data[(int)Items.Strawberry].MaxWaterTime = 0;
        Data[(int)Items.Strawberry].MaxGrowthTime = 0;
        Data[(int)Items.Strawberry].MaxDeathTime = 0;

        #endregion

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
            Garden[i].Position = Vector3.zero;
            Garden[i].Active = false;
            Garden[i].State = 0;
            Garden[i].WaterTimer = 0;
            Garden[i].GrowthTimer = 0;

            ActivePlants--;
        }
    }

    public static void Water(Vector3 position)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != position)
        {
            i++;
        }

        if (Garden[i].Position == position)
        {
            Garden[i].WaterTimer = Data[(int)Garden[i].Item].MaxWaterTime;

        }
    }

    public static void Grown(Vector3 position)
    {
        int i = 0;
        while (i < ActivePlants && Garden[i].Position != position)
        {
            i++;
        }

        if (Garden[i].Position == position)
        {
            Garden[i].GrowthTimer = Data[(int)Garden[i].Item].MaxGrowthTime;

        }
    }

} // class GardenManager 
// namespace
