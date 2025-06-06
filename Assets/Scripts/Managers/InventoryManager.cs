//---------------------------------------------------------
// Script para gestionar los datos del inventario
// Responsable: Julia Vera Ruiz
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


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    public static void ResetInventory()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = 0;
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

        #endregion

        // ---- MÉTODOS INVENTARIO ----
        #region Métodos Inventario

        /// <summary>
        /// Devuelve True si se efectua la modificación
        /// Añade la cantidad (quantity) al inventory del Item (item) comprobando que los valores esten dentro de los parámetros permitidos
        /// </summary>
        public static bool BoolModifyInventory(Items item, int quantity)
        {
            if ((int)item >= (int)Items.Count / 2) // Es un cultivo 
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
                }
                return false;
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
                }
                return false;
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

            #endregion // métodos modificar inventario

    #endregion // métodos públicos
}