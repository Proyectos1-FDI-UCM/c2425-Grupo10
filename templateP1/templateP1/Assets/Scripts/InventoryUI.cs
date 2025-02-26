//---------------------------------------------------------
// Script para gestionar la visibilidad del inventario.
// Permite mostrarlo y ocultarlo con TAB o ESC.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que gestiona la aparición y desaparición del inventario.
/// Permite mostrar el inventario al pulsar TAB y ocultarlo al pulsar 
/// nuevamente TAB o ESC.
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private RectTransform inventoryPanel; // Panel del inventario
    [SerializeField] private float visibleY = 0f; // Posición Y cuando el inventario es visible
    [SerializeField] private float hiddenY = -300f; // Posición Y cuando el inventario está oculto
    [SerializeField] private float transitionSpeed = 5f; // Velocidad de movimiento

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Indica si el inventario está actualmente visible.
    /// </summary>
    private bool _isInventoryVisible = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }

        // Movimiento suave del inventario
        float targetY = _isInventoryVisible ? visibleY : hiddenY;
        inventoryPanel.anchoredPosition = Vector2.Lerp(inventoryPanel.anchoredPosition, new Vector2(inventoryPanel.anchoredPosition.x, targetY), Time.deltaTime * transitionSpeed);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

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

