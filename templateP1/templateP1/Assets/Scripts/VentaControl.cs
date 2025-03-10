//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;

public class VenderProducto : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descripcionTexto; // Para el texto de la descripción

    private int cantidad = 1; // Cantidad inicial
    private int precioPorProducto = 0; // Precio del producto seleccionado
    private string nombreProducto = ""; // Nombre del producto seleccionado

    // Método para cambiar el producto a Maíz
    public void SeleccionarMaiz()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Maíz"; // Cambia el nombre del producto
        precioPorProducto = 90; // Cambia el precio de Maíz
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    // Método para cambiar el producto a Fresas
    public void SeleccionarFresas()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Fresas"; // Cambia el nombre del producto
        precioPorProducto = 40; // Cambia el precio de las Fresas
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    // Método para cambiar el producto a Zanahorias
    public void SeleccionarZanahorias()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Zanahorias"; // Cambia el nombre del producto
        precioPorProducto = 65; // Cambia el precio de Zanahorias
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    // Método para cambiar el producto a Lechuga
    public void SeleccionarLechuga()
    {
        cantidad = 1; // Resetea la cantidad a 1
        nombreProducto = "Lechuga"; // Cambia el nombre del producto
        precioPorProducto = 20; // Cambia el precio de las Lechugas
        ActualizarDescripcion(); // Actualiza la descripción con el nuevo producto y precio
    }

    // Método para aumentar la cantidad
    public void AumentarCantidad()
    {
        if (cantidad > 0) // Asegurarse de que la cantidad es mayor que 0
        {
            cantidad++; // Incrementa la cantidad
            ActualizarDescripcion(); // Actualiza la descripción
        }
    }

    // Método para disminuir la cantidad
    public void DisminuirCantidad()
    {
        if (cantidad > 1)  // No dejamos que la cantidad sea menor que 1
        {
            cantidad--; // Disminuye la cantidad
            ActualizarDescripcion(); // Actualiza la descripción
        }
    }

    // Método para actualizar el texto de la descripción
    private void ActualizarDescripcion()
    {
        if (!string.IsNullOrEmpty(nombreProducto) && precioPorProducto > 0)
        {
            descripcionTexto.text = $"{cantidad} {nombreProducto}(s) = {cantidad * precioPorProducto} rootcoins"; // Muestra la cantidad y el precio total
        }
    }
}
