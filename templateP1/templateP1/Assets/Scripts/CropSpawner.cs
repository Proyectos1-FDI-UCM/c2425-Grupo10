//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    // Prefab de la semilla (asegúrate de que esté en el Inspector)
    [SerializeField]
    private GameObject PrefabSemilla1;  // Aquí defines el prefab

    private Vector3 spawnPosition;

    private void Start()
    {
        // Asigna la posición de la maceta (u objeto que tiene este script)
        spawnPosition = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Si el jugador presiona la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Llama a la función que planta la planta en la posición determinada
            Plantar(spawnPosition);
        }
    }

    private void Plantar(Vector3 position)
    {
        // Verifica que el prefab no sea null antes de instanciarlo
        if (PrefabSemilla1 != null)
        {
            // Instancia la planta
            GameObject planta = Instantiate(PrefabSemilla1, position, Quaternion.identity);
            // Verifica si la planta tiene el script PlantaEvolucion
            PlantaEvolucion plantaEvolucion = planta.GetComponent<PlantaEvolucion>();
            if (plantaEvolucion != null)
            {
                plantaEvolucion.Planta();  // Inicia el proceso de plantación
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

