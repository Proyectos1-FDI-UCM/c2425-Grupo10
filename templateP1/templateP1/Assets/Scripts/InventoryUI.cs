//---------------------------------------------------------
// Script para gestionar la visibilidad del inventario.
// Permite mostrarlo y ocultarlo con TAB o ESC.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private RectTransform inventoryPanel; // Panel del inventario completo
    [SerializeField] private RectTransform quickAccessBar; // Barra de acceso rápido (siempre visible)
    [SerializeField] private float visibleY = 0f; // Posición cuando el inventario es visible
    [SerializeField] private float hiddenY = -300f; // Posición cuando el inventario está oculto
    [SerializeField] private float quickBarVisibleY = -250f; // Posición cuando la barra está siempre visible
    [SerializeField] private float transitionSpeed = 10f; // Velocidad de animación

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _isInventoryVisible = false; // Estado del inventario

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Inicializa la UI dejando la barra visible pero el inventario oculto.
    /// </summary>
    void Start()
    {
        // Inicializa posiciones
        inventoryPanel.anchoredPosition = new Vector2(inventoryPanel.anchoredPosition.x, hiddenY);
        quickAccessBar.anchoredPosition = new Vector2(quickAccessBar.anchoredPosition.x, quickBarVisibleY);
    }

    /// <summary>
    /// Detecta la pulsación de teclas para mostrar u ocultar el inventario.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.EscWasPressedThisFrame() || InputManager.Instance.TabWasPressedThisFrame())
        {
            ToggleInventory();
        }

        // Define posiciones objetivo
        float targetInventoryY = _isInventoryVisible ? visibleY : hiddenY;
        float targetQuickBarY = _isInventoryVisible ? visibleY : quickBarVisibleY;

        // Aplica interpolación suave (Lerp) para animación
        inventoryPanel.anchoredPosition = Vector2.Lerp(inventoryPanel.anchoredPosition,
            new Vector2(inventoryPanel.anchoredPosition.x, targetInventoryY),
            Time.deltaTime * transitionSpeed);

        quickAccessBar.anchoredPosition = Vector2.Lerp(quickAccessBar.anchoredPosition,
            new Vector2(quickAccessBar.anchoredPosition.x, targetQuickBarY),
            Time.deltaTime * transitionSpeed);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Cambia la visibilidad del inventario.
    /// </summary>
    private void ToggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible;
    }

    #endregion

} // class InventoryUI
