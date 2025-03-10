//---------------------------------------------------------
// Script para gestionar la visibilidad del inventario.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCultivos : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [SerializeField] private RectTransform inventoryPanel;  // Panel del inventario
    [SerializeField] private RectTransform quickAccessBar;  // Barra de acceso rápido
    [SerializeField] private float visibleY = 0f;           // Posición Y del inventario cuando está visible
    [SerializeField] private float hiddenY = -300f;         // Posición Y del inventario cuando está oculto
    [SerializeField] private float transitionSpeed = 10f;   // Velocidad de animación
    [SerializeField] private float quickBarOffset = 100f;   // Espacio entre inventario y QuickAccessBar


    [SerializeField] private CultivosLista cultivosLista; // El inventario del jugador
    [SerializeField] private GameObject[] casillasInventario; // Referencias a las casillas de la UI
    //[SerializeField] private GameObject[] filasCasillas;
    [SerializeField] private Image[] imagenesCultivos;  // Las imágenes de los cultivos en las casillas
    [SerializeField] private Text[] cantidadesCultivos; // Los textos de las cantidades (al final no lo implementé)

    [SerializeField] private AudioSource sonidoInventario; // (Opcional) Sonido al abrir/cerrar

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados
    private bool _isInventoryVisible = false; // Estado del inventario
    private float quickBarBaseY; // Posición base de la QuickAccessBar (se mantiene siempre visible)
    private List<(string, int, Sprite)> inventario = new List<(string, int, Sprite)>();

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        // Guardamos la posición inicial de la QuickAccessBar para que siempre sea visible
        quickBarBaseY = quickAccessBar.anchoredPosition.y;

        // Inicializamos la posición del inventario en oculto
        inventoryPanel.anchoredPosition = new Vector2(inventoryPanel.anchoredPosition.x, hiddenY);

        ActualizarInventario();
    }

    void Update()
    {
        //la subida del inventario se puede activar tanto con el TAB como con el ESC
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        // Define la posición objetivo del inventario
        float targetInventoryY = _isInventoryVisible ? visibleY : hiddenY;

        // Define la posición de la QuickAccessBar
        float targetQuickBarY = _isInventoryVisible ? (visibleY + quickBarOffset) : quickBarBaseY;

        // Movimiento suave del inventario
        inventoryPanel.anchoredPosition = Vector2.Lerp(
            inventoryPanel.anchoredPosition,
            new Vector2(inventoryPanel.anchoredPosition.x, targetInventoryY),
            Time.deltaTime * transitionSpeed
        );

        // Movimiento suave de la QuickAccessBar para que suba con el inventario
        quickAccessBar.anchoredPosition = Vector2.Lerp(
            quickAccessBar.anchoredPosition,
            new Vector2(quickAccessBar.anchoredPosition.x, targetQuickBarY),
            Time.deltaTime * transitionSpeed
        );


    }

    public void ActualizarInventario()
    {
        inventario = cultivosLista.GetCultivosDisponibles();

        for (int i = 0; i < casillasInventario.Length; i++)
        {
            if (i < inventario.Count)
            {
                var cultivo = inventario[i];
                imagenesCultivos[i].sprite = cultivo.Item3;
                cantidadesCultivos[i].text = cultivo.Item2.ToString();
                casillasInventario[i].SetActive(true);
            }
            else
            {
                casillasInventario[i].SetActive(false);
            }
        }
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

        // Reproducir sonido opcional al abrir/cerrar el inventario
        if (sonidoInventario != null)
        {
            sonidoInventario.Play();
        }

        // Solo actualizar si el inventario se está mostrando
        if (_isInventoryVisible)
        {
            ActualizarInventario();
        }
    }

    #endregion

} // class InventoryUI
