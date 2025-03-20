//---------------------------------------------------------
// Script para gestionar la visibilidad del inventario.
// Responsable: Alexia Pérez Santana, Iria Docampo Zotes, Julia Vera Ruiz
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;

// Añadir aquí el resto de directivas using
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// La Clase InventoryCultivos se encarga de mostrar el inventario al pulsar la tecla TAB 
/// Actualiza su información en función de InventoryManager
/// </summary>
public class InventoryCultivos : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    /// <summary>
    /// Panel del inventario
    /// </summary>
    [SerializeField] private RectTransform inventoryPanel;

    /// <summary>
    /// Barra de acceso rápido
    /// </summary>
    [SerializeField] private RectTransform quickAccessBar;  

    /// <summary>
    /// Iconos del Inventario
    /// </summary>
    [SerializeField] private GameObject InventoryIcons;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Estado del inventario
    /// </summary>
    private bool _isInventoryVisible = false; 

    /// <summary>
    /// Posiciones y velocidades
    /// </summary>
    private float _quickBarBaseY;           // Posición base de la QuickAccessBar (se mantiene siempre visible)
    private float _visibleY = 300f;           // Posición Y del inventario cuando está visible
    private float _hiddenY = -300f;         // Posición Y del inventario cuando está oculto
    private float _quickBarOffset = 100f;   // Espacio entre inventario y QuickAccessBar
    private float _transitionSpeed = 10f;   // Velocidad de animación

    /// <summary>
    ///  Capacidad de cada Slot del inventario
    /// </summary>
    private int _slotsCapacity = 10;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        // Guardamos la posición inicial de la QuickAccessBar para que siempre sea visible
        _quickBarBaseY = quickAccessBar.anchoredPosition.y;
        // Inicializamos la posición del inventario en oculto
        inventoryPanel.anchoredPosition = new Vector2(inventoryPanel.anchoredPosition.x, _hiddenY);

    }

    void Update()
    {
        // La subida del inventario se puede activar tanto con el TAB como con el ESC
        if (InputManager.Instance.TabWasPressedThisFrame())
        {
            ToggleInventory();
            ActualizeInventory();
        }

        // Define la posición objetivo del inventario
        float targetInventoryY = _isInventoryVisible ? _visibleY : _hiddenY;

        // Define la posición de la QuickAccessBar
        float targetQuickBarY = _isInventoryVisible ? (_visibleY + _quickBarOffset) : _quickBarBaseY;

        // Movimiento suave del inventario
        inventoryPanel.anchoredPosition = Vector2.Lerp(
            inventoryPanel.anchoredPosition,
            new Vector2(inventoryPanel.anchoredPosition.x, targetInventoryY),
            Time.deltaTime * _transitionSpeed
        );

        // Movimiento suave de la QuickAccessBar para que suba con el inventario
        quickAccessBar.anchoredPosition = Vector2.Lerp(
            quickAccessBar.anchoredPosition,
            new Vector2(quickAccessBar.anchoredPosition.x, targetQuickBarY),
            Time.deltaTime * _transitionSpeed
        );


    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Alterna la visibilidad del inventario y mueve la QuickAccessBar con él.
    /// </summary>
    private void ToggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible;
    }

    /// <summary>
    /// Actualiza la cantidad de los items del inventario
    /// No comprueba si hay inventario suficiente para mostrar los items porque ya lo comprueba InventoryManager
    /// </summary>
    public void ActualizeInventory()
    {
        TextMeshProUGUI _unidades;

        // Muestra las semillas
        for (int i = 0; i < (int)Items.Count/2; i++)
        {
            GameObject _crops = InventoryIcons.transform.GetChild(i).gameObject;
            if (InventoryManager.Inventory[i] != 0)
            {
                _crops.SetActive(true);
                _unidades = _crops.GetComponentInChildren<TextMeshProUGUI>();
                _unidades.text = InventoryManager.Inventory[i] + "x";
            }
            else _crops.SetActive(false);
        }

        // Muestra los cultivos
        for (int i = (int)Items.Count / 2; i < (int)Items.Count; i++) 
        {
            if (InventoryManager.Inventory[i] != 0)
            {
                int actualSlot = 1; // El Slot actual que está estableciendo
                bool fullSlot = false; // Es true si el Slot es igual que la cantidad máxima por Slot
                
                while (actualSlot < 5 && !fullSlot)
                {
                    GameObject _crops = InventoryIcons.transform.GetChild(i * actualSlot).gameObject;
                    _crops.SetActive(true);
                    _unidades = _crops.GetComponentInChildren<TextMeshProUGUI>();
                    if (InventoryManager.Inventory[i] / (actualSlot * _slotsCapacity) != 0)
                    {
                        _unidades.text = _slotsCapacity + "x";
                    }
                    else if (InventoryManager.Inventory[i] - ((actualSlot - 1) * _slotsCapacity) != 0)
                    {
                        _unidades.text = InventoryManager.Inventory[i] - ((actualSlot - 1) * _slotsCapacity) + "x";
                        fullSlot = true;
                    }
                    else
                    {
                        _crops.SetActive(false);
                        fullSlot = true;
                    }
                    actualSlot++;
                }
            }
            else InventoryIcons.transform.GetChild(i).gameObject.SetActive(false);
        }
        
    }
    

        #endregion

} // class InventoryUI
