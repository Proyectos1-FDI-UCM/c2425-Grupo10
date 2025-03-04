//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo: Julia Vera, Natalia Nita
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    // Prefab de la semilla (asegúrate de que esté en el Inspector)
    [SerializeField]
    private GameObject PrefabSemilla1;  // Aquí defines el prefab

    [SerializeField]
    private Transform Plantas; // Para que los prefabs se asignen en la carpeta correcta

    [SerializeField]
    private float TiempoRegado; // Establecer el tiempo entre regados

    [SerializeField]
    private float TiempoCrecimiento; // Establecer el tiempo de crecimiento


    private void Start()
    {

    }


    /// <summary>
    /// Planta la semilla  si se cumplen las condiciones (Herramienta Correcta, Usuario pulsa usar [E], El jugador tiene Semillas)
    /// </summary>
    private void OnTriggerStay2D()
    {
        int Semillas = LevelManager.Instance.Semillas();
        // Si el jugador presiona la tecla E, hay semillas 
        if ((InputManager.Instance.UsarWasPressedThisFrame() || InputManager.Instance.UsarIsPressed()) && LevelManager.Instance.Herramientas() == 5 && LevelManager.Instance.Semillas() > 0)
        {
            Plantar(); // Llama a la función que planta la semilla en la posición determinada
            LevelManager.Instance.Plantar(); // Llama al método que controla la cantidad de semillas
            Destroy(this.gameObject);
            
        }
        
    }

    /// <summary>
    /// Planta la semilla y se asegura que tenga el script necesario para evolucionar
    /// </summary>
    private void Plantar()
    {
        // Verifica que el prefab no sea null antes de instanciarlo
        if (PrefabSemilla1 != null)
        {
            // Instancia la planta y la guarda como Child de la carpeta Plantas
            GameObject planta = Instantiate(PrefabSemilla1, transform.position, Quaternion.identity);
            planta.transform.SetParent(Plantas);

            // Verifica si la planta tiene el script PlantaEvolucion
            PlantaEvolucion plantaEvolucion = planta.GetComponent<PlantaEvolucion>();
            if (plantaEvolucion != null)
            {
                plantaEvolucion.Planta(TiempoCrecimiento, TiempoRegado, Plantas);  
            }
            else
            {
                Debug.LogError("El prefab de la planta no tiene el script PlantaEvolucion.");
            }
        }
        else
        {
            Debug.LogError("PrefabSemilla1 no ha sido asignado en el Inspector");
        }
    }
}
