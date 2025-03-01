//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PrefabSemilla1;  // El prefab de la semilla (asegúrate de asignarlo en el Inspector)
    private Vector3 spawnPosition;

    private void Start()
    {
        // Asignamos la posición de la maceta para luego plantar en ese lugar
        spawnPosition = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Verificamos si el objeto que está en el trigger es el jugador (comprobamos la etiqueta)
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))  // Si el jugador presiona la tecla E
        {
            Plantar(spawnPosition);
        }
    }

    private void Plantar(Vector3 position)
    {
        // Verificamos que el prefab esté asignado antes de instanciarlo
        if (PrefabSemilla1 != null)
        {
            // Instanciamos la planta en la posición determinada
            GameObject planta = Instantiate(PrefabSemilla1, position, Quaternion.identity);

            // Intentamos obtener el componente PlantaEvolucion
            PlantaEvolucion plantaEvolucion = planta.GetComponent<PlantaEvolucion>();
            if (plantaEvolucion != null)
            {
                plantaEvolucion.Planta();  // Inicia la evolución de la planta
            }
            else
            {
                Debug.LogError("El prefab de la planta no tiene el script PlantaEvolucion.");
            }
        }
        else
        {
            Debug.LogError("PrefabSemilla1 no ha sido asignado en el Inspector.");
        }
    }
}
