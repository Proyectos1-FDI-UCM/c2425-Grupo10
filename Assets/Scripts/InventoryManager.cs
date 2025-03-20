using UnityEngine;

/// <summary>
/// Items es un Enum que asocia la lista de items que puede tener el jugador con un entero
/// Count es el total de items que puede tener el jugador
/// De 0 - Count/2 -> Semillas (Seeds)
/// De Count/2 - Count ->  Cultivos (Crops)
/// </summary>
public enum Items
{
    CornSeed,
    LetuceSeed,
    CarrotSeed,
    StrawberrySeed,
    Corn,
    Letuce,
    Carrot,
    Strawberry,
    Count
}

public static class InventoryManager
{
    /// <summary>
    /// Cantidad de cada Item que tiene el jugador
    /// </summary>
    public static int[] Inventory = new int[(int)Items.Count];

    /// <summary>
    /// Capacidad Máxima
    /// </summary>
    [SerializeField] static int MaxSeedQuantity = 30; // Cantidad máxima de espacio disponible en el inventario para las semillas
    [SerializeField] static int MaxCropQuantity = 40; // Cantidad máxima de espacio disponible en el inventario para los cultivos


    /// <summary>
    /// Devuelve un entero, la cantidad de dicho item que tiene el jugador
    /// </summary>
    public static int GetInventory(Items item)  
    {
        return Inventory[(int)item];
    }

    /// <summary>
    /// Añade la cantidad (quantity) al inventario del Item (item) 
    /// </summary>
    public static void ModifyInventory(Items item, int quantity) // (Se puede restar con números negativos)
    {
        if ((int)item >= (int)Items.Count / 2) // Es un cultivo 
        {
            if (Inventory[(int)item] + quantity <= MaxCropQuantity) Inventory[(int)item] += quantity;
            else Debug.Log("InventarioLleno");

        }
        else // Es una semilla
        {
            if (Inventory[(int)item] + quantity <= MaxSeedQuantity) Inventory[(int)item] += quantity;
            else Debug.Log("InventarioLleno");

        }
    }

    public static void SetMaxCrops (int maxCrops)
    {
        MaxCropQuantity = maxCrops;
    }

    public static void SetMaxSeeds(int maxSeeds)
    {
        MaxSeedQuantity = maxSeeds;
    }

}
