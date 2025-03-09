//---------------------------------------------------------
// Script para gestionar la interfaz de venta en el juego.
// Responsable: Natalia
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona la interfaz de venta, permitiendo actualizar
/// el precio del botón "Vender" según el producto seleccionado.
/// </summary>
public class VentaUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary> Texto del botón "Vender". </summary>
    [SerializeField] private TextMeshProUGUI TextoBotonVender;

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Cambia el texto del botón de vender según el producto seleccionado.
    /// </summary>
    /// <param name="precio">Precio del producto seleccionado.</param>
    public void SeleccionarProducto(int precio)
    {
        TextoBotonVender.text = $"Vender por: {precio} Rc";
    }

    #endregion
} // class VentaUI
