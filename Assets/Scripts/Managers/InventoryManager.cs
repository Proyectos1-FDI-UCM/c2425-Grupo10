//---------------------------------------------------------
// Script para gestionar los datos del inventario
// Responsable: Julia Vera Ruiz, Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Items es un Enum que asocia la lista de items que puede tener el jugador con un entero
/// Count es el total de items que puede tener el jugador
/// De 0 - Count/2 -> Seeds (Seeds)
/// De Count/2 - Count ->  Cultivos (Crops)
/// /// Fertilizer es un item especial que va aparte
/// </summary>
public enum Items
{
    CornSeed,
    LettuceSeed,
    CarrotSeed,
    StrawberrySeed,
    Corn,
    Lettuce,
    Carrot,
    Strawberry,
    Fertilizer, // NUEVO ITEM: Abono
    Count
}

/// <summary>
/// InventoryManager es una clase sin monobehaviour que maneja los datos del inventory
/// Tiene un Enum para todos los items posibles que tenga el jugador (semillas y cultivos)
/// Un array para guardar la cantidad actual de cada tipo de item
/// Métodos: GetInventory; ModifyInventory; SetMaxCrops; SetMaxSeeds;
/// </summary>
public static class InventoryManager
{
    /// <summary>
    /// Posicion del Jugador
    /// </summary>
    private static Vector3 PlayerPosition = new Vector3 (14.14f, -9.62f, 0);

    /// <summary>
    /// Cantidad de cada Item que tiene el jugador
    /// </summary>
    private static int[] Inventory = new int[(int)Items.Count];

    /// <summary> 
    /// Capacidad Máxima Semillas
    /// </summary>
    private static int MaxSeedQuantity = 30; // Cantidad máxima de espacio disponible en el inventory para las semillas

    /// <summary>
    /// Capacidad Máxima Cultivos
    /// </summary>
    private static int MaxCropQuantity = 40; // Cantidad máxima de espacio disponible en el inventory para los cultivos

    /// <summary>
    /// Bool que se activa si el inventario está lleno para algún elemento
    /// </summary>
    private static bool _inventoryFull = false;

    /// <summary>
    /// Inventario separado para cultivos con abono
    /// Solo los cultivos (Corn, Lettuce, Carrot, Strawberry) pueden tener versión con abono
    /// </summary>
    private static int[] FertilizedCropsInventory = new int[4]; // Solo para los 4 tipos de cultivos


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos
    public static void ResetInventory()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = 0;
        }
        // NUEVO: También resetear inventario de cultivos con abono
        ResetFertilizedCropsInventory();
    }

    /// <summary>
    /// Resetea el inventario de cultivos con abono
    /// </summary>
    public static void ResetFertilizedCropsInventory()
    {
        for (int i = 0; i < 4; i++)
        {
            FertilizedCropsInventory[i] = 0;
        }
    }

    // ---- MÉTODOS SET ----
    #region Métodos Set
    /// <summary>
    /// Modifica la Posición del Jugador
    /// </summary>
    /// <param name="position"></param>
    public static void SetPlayerPosition(Vector3 position)
        {
            PlayerPosition = position;
        }
        public static void ModifyPlayerPosition(Vector3 position)
        {
            PlayerPosition = position;
        }

        /// <summary>
        /// Establece el inventario completo (Cargar Partida)
        /// </summary>
        /// <param name="inventory"></param>
        public static void SetInventory(int[] inventory)
        {
            Inventory = inventory;
        }

        /// <summary>
        /// Cambia la cantidad máxima de cultivos que puedes guardar en el inventory
        /// </summary>
        public static void SetMaxCrops(int maxCrops)
        {
            MaxCropQuantity = maxCrops;
        }

        /// <summary>
        /// Cambia la cantidad máxima de semillas que puedes guardar en el inventory
        /// </summary>
        public static void SetMaxSeeds(int maxSeeds)
        {
            MaxSeedQuantity = maxSeeds;
        }

        /// <summary>
        /// Establece el inventario de cultivos con abono (para carga de partida)
        /// </summary>
        public static void SetFertilizedCropsInventory(int[] fertilizedInventory)
        {
            for (int i = 0; i < 4 && i < fertilizedInventory.Length; i++)
            {
                FertilizedCropsInventory[i] = fertilizedInventory[i];
            }
        }

    #endregion

    // ---- MÉTODOS GET ----
    #region Métodos Get

        /// <summary>
        /// Devuelve un entero, la cantidad de dicho item que tiene el jugador
        /// </summary>
        public static int GetInventoryItem(Items item)
        {
            return Inventory[(int)item];
        }

        public static int GetInventoryItem(int item)
        {
            return Inventory[(int)item];
        }

        
        /// <summary>
        /// Guarda la Posición del Jugador
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPlayerPosition()
        {
            return PlayerPosition;
        }

        public static int[] GetInventory()
        {
            return Inventory;
        }

        ///<summary>
        /// Metodo para cambiar el bool del inventario
        /// </summary>
        public static void InventoryNotFull()
        {
            _inventoryFull = false;
        }

        ///<summary>
        ///metodo para saber si el inventario esta lleno
        /// </summary>
        public static bool GetBoolInventoryFull()
        {
            return _inventoryFull;
        }

        /// <summary>
        /// Devuelve el máximo de semillas que se pueden guardar en el inventory
        /// </summary>
        public static int GetMaxSeeds()
        {
            return MaxSeedQuantity;
        }

        /// <summary>
        /// Devuelve el máximo de cultivos que se pueden guardar en el inventory
        /// (Para mostrar slots en el inventory)
        /// </summary>
        public static int GetMaxCrop()
        {
            return MaxCropQuantity;
        }

        /// <summary>
        /// Obtiene la cantidad de cultivos normales (sin abono) de un tipo específico
        /// </summary>
        public static int GetNormalCropQuantity(Items item)
        {
            if ((int)item >= (int)Items.Corn && (int)item <= (int)Items.Strawberry)
            {
                return Inventory[(int)item];
            }
            return 0;
        }

        /// <summary>
        /// Obtiene la cantidad de cultivos con abono de un tipo específico
        /// </summary>
        public static int GetFertilizedCropQuantity(Items item)
        {
            int index = GetFertilizedCropIndex(item);
            if (index >= 0)
            {
                return FertilizedCropsInventory[index];
            }
            return 0;
        }

        /// <summary>
        /// Obtiene la cantidad total de un cultivo (normal + con abono)
        /// </summary>
        public static int GetTotalCropQuantity(Items item)
        {
            return GetNormalCropQuantity(item) + GetFertilizedCropQuantity(item);
        }

        /// <summary>
        /// Obtiene el inventario de cultivos con abono (para guardado)
        /// </summary>
        public static int[] GetFertilizedCropsInventory()
        {
            int[] fertilizedInventory = new int[4];
            for (int i = 0; i < 4; i++)
            {
                fertilizedInventory[i] = FertilizedCropsInventory[i];
            }
            return fertilizedInventory;
        }
    #endregion

    // ---- MÉTODOS INVENTARIO ----
    #region Métodos Inventario

    /// <summary>
    /// Devuelve True si se efectua la modificación
    /// Añade la cantidad (quantity) al inventory del Item (item) comprobando que los valores esten dentro de los parámetros permitidos
    /// </summary>
    public static bool BoolModifyInventory(Items item, int quantity)
    {
        // NUEVO: Manejar abono como item especial
        if (item == Items.Fertilizer)
        {
            if (Inventory[(int)item] + quantity <= MaxSeedQuantity) // El abono usa límite de semillas
            {
                Inventory[(int)item] += quantity;
                return true;
            }
            else
            {
                Debug.Log("InventarioLleno - No hay espacio para más abono");
                _inventoryFull = true;
                return false;
            }
        }
        else if ((int)item >= (int)Items.Corn && (int)item <= (int)Items.Strawberry) // Es un cultivo 
        {
            if (Inventory[(int)item] + quantity <= MaxCropQuantity)
            {
                Inventory[(int)item] += quantity;
                return true;
            }
            else
            {
                Debug.Log("InventarioLleno");
                _inventoryFull = true;
                return false;
            }
        }
        else // Es una semilla
        {
            if (Inventory[(int)item] + quantity <= MaxSeedQuantity)
            {
                Inventory[(int)item] += quantity;
                return true;
            }
            else
            {
                Debug.Log("InventarioLleno");
                _inventoryFull = true;
                return false;
            }
        }
    }

    /// <summary>
    /// Devuelve True si se efectua la modificación
    /// Resta la cantidad (quantity) al inventory del Item (item) comprobando que los valores esten dentro de los parámetros permitidos 
    /// IMPORTANTE - USAR NUMEROS POSITIVOS (ModifyInventorySubstract(Item.Corn, 5) - Resta 5 Maices)
    /// </summary>
    public static bool BoolModifyInventorySubstract(Items item, int quantity) // (Se puede restar con números negativos)
        {
            if ((int)item >= (int)Items.Count / 2) // Es un cultivo 
            {
                if (Inventory[(int)item] - quantity <= 0) Inventory[(int)item] -= quantity;
                else Debug.Log("InventarioInsuficiente");
                return true;
            }
            else // Es una semilla
            {
                if (Inventory[(int)item] - quantity <= 0) Inventory[(int)item] -= quantity;
                else Debug.Log("InventarioInsuficiente");
                return false;
            }
    }

        /// <summary>
        /// Añade la cantidad (quantity) al inventory del Item (item) comprobando que los valores esten dentro de los parámetros permitidos
        /// </summary>
        public static void ModifyInventory(Items item, int quantity)
        {
            if ((int)item >= (int)Items.Count / 2) // Es un cultivo 
            {
                if (Inventory[(int)item] + quantity <= MaxCropQuantity) Inventory[(int)item] += quantity;
                else
                {
                    Debug.Log("InventarioLleno");
                    _inventoryFull = true;
                }
                Debug.Log("Item" + item.ToString() + " Cantidad: " + quantity);
            }
            else // Es una semilla
            {
                if (Inventory[(int)item] + quantity <= MaxSeedQuantity) Inventory[(int)item] += quantity;
                else
                {
                    Debug.Log("InventarioLleno");
                    _inventoryFull = true;
                }
                Debug.Log("Item" + item.ToString() + " Cantidad: " + quantity);
            }
        }

        /// <summary>
        /// Resta la cantidad (quantity) al inventory del Item (item) comprobando que los valores esten dentro de los parámetros permitidos 
        /// IMPORTANTE - USAR NUMEROS POSITIVOS (ModifyInventorySubstract(Item.Corn, 5) - Resta 5 Maices)
        /// </summary>
        public static void ModifyInventorySubstract(Items item, int quantity) // (Se puede restar con números negativos)
        {
            if ((int)item >= (int)Items.Count / 2) // Es un cultivo 
            {
                if (Inventory[(int)item] - quantity >= 0) Inventory[(int)item] -= quantity;
                else Debug.Log("InventarioInsuficiente");
            }
            else // Es una semilla
            {
                if (Inventory[(int)item] - quantity >= 0) Inventory[(int)item] -= quantity;
                else Debug.Log("InventarioInsuficiente");
            }
        }

    /// <summary>
    /// Añade cultivos con abono al inventario
    /// </summary>
    public static bool AddFertilizedCrop(Items item, int quantity)
    {
        int index = GetFertilizedCropIndex(item);
        if (index >= 0)
        {
            // Verificar límite de inventario total
            int totalCrops = GetTotalCropQuantity(item);
            if (totalCrops + quantity <= MaxCropQuantity)
            {
                FertilizedCropsInventory[index] += quantity;
                return true;
            }
            else
            {
                Debug.Log($"No hay espacio suficiente para {quantity} {item} con abono");
                _inventoryFull = true;
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Modifica el inventario de cultivos normales (sin abono)
    /// </summary>
    public static bool ModifyNormalCropInventory(Items item, int quantity)
    {
        if (quantity > 0)
        {
            // Añadir cultivos normales
            int totalCrops = GetTotalCropQuantity(item);
            if (totalCrops + quantity <= MaxCropQuantity)
            {
                Inventory[(int)item] += quantity;
                return true;
            }
            else
            {
                _inventoryFull = true;
                return false;
            }
        }
        else
        {
            // Quitar cultivos normales
            int currentNormal = GetNormalCropQuantity(item);
            if (currentNormal >= -quantity)
            {
                Inventory[(int)item] += quantity; // quantity es negativo
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Modifica el inventario de cultivos con abono
    /// </summary>
    public static bool ModifyFertilizedCropInventory(Items item, int quantity)
    {
        int index = GetFertilizedCropIndex(item);
        if (index >= 0)
        {
            if (quantity > 0)
            {
                // Añadir cultivos con abono
                return AddFertilizedCrop(item, quantity);
            }
            else
            {
                // Quitar cultivos con abono
                int currentFertilized = GetFertilizedCropQuantity(item);
                if (currentFertilized >= -quantity)
                {
                    FertilizedCropsInventory[index] += quantity; // quantity es negativo
                    return true;
                }
            }
        }
        return false;
    }
    #endregion // métodos modificar inventario

    #endregion // métodos públicos


    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Mapea Items de cultivos a índices del array de cultivos con abono
    /// </summary>
    private static int GetFertilizedCropIndex(Items item)
    {
        switch (item)
        {
            case Items.Corn: return 0;
            case Items.Lettuce: return 1;
            case Items.Carrot: return 2;
            case Items.Strawberry: return 3;
            default: return -1; // No es un cultivo válido para abono
        }
    }

    #endregion // métodos privados
}
