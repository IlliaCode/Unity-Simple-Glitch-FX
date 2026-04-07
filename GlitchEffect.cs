using UnityEngine;

/// <summary>
/// Creates a 3D visual glitch effect by procedurally spawning and destroying blocks.
/// This script works across all Unity render pipelines (Built-in, URP, HDRP).
/// </summary>
public class GlitchEffect : MonoBehaviour
{
    private float _timer; // Internal counter to track time between each spawn

    [Header("Glitch Settings")]
    [Tooltip("Delay between spawns. Lower values = more intense glitch effect.")]
    [SerializeField] private float glitchIntensity = 0.1f;

    [Tooltip("Base scale factor. If set to 0, the script will auto-detect the object's size.")]
    [SerializeField] private float size;

    [Tooltip("The radius around the object where glitch blocks can appear.")]
    [SerializeField] private float spawnRadius = 0.5f;

    [Tooltip("The 3D model (e.g., a Cube) that will appear as a glitch particle.")]
    [SerializeField] public GameObject cubePrefab;

    [Tooltip("List of colors that will be randomly assigned to the glitch blocks.")]
    [SerializeField] private Color[] glitchColors = { Color.cyan, Color.magenta, Color.yellow, Color.green, Color.black };

    void Start()
    {
        // Auto-calculate size if not manually set in the Inspector
        if (size <= 0)
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                size = rend.bounds.size.magnitude;
            }
            else
            {
                size = 1f; // Fallback to 1 if no renderer is found
            }
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > glitchIntensity)
        {
            SpawnGlitchBlock();
            _timer = 0f; // Reset the cycle
        }
    }

    private void SpawnGlitchBlock()
    {
        if (cubePrefab == null) return;

        // Calculate a random spawn point within a radius based on the 'size' variable
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;

        // Create the glitch block at the calculated position
        GameObject newBlock = Instantiate(cubePrefab, randomPos, Quaternion.identity);

        // Optional: Make blocks follow the parent object's movement
        // newBlock.transform.SetParent(this.transform);

        // Adjust block scale relative to the object's size to keep the effect proportional
        float sizeFactor = size * 0.15f;
        newBlock.transform.localScale = new Vector3(
            Random.Range(0.2f, 1.2f) * sizeFactor,
            Random.Range(0.1f, 0.5f) * sizeFactor,
            Random.Range(0.2f, 1.2f) * sizeFactor
        );

        // Apply a random color from the glitchColors array to the material
        Renderer blockRenderer = newBlock.GetComponent<Renderer>();
        if (blockRenderer != null)
        {
            // Note: blockRenderer.material.color creates a unique material instance per block
            blockRenderer.material.color = glitchColors[Random.Range(0, glitchColors.Length)];
        }

        // Destroy the block quickly to simulate high-frequency digital flickering
        Destroy(newBlock, 0.15f);
    }
}
