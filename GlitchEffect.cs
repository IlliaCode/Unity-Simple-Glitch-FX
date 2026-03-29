using UnityEngine;

/// <summary>
/// Creates a visual 3D glitch effect by spawning procedural objects.
/// </summary>
public class GlitchEffect : MonoBehaviour
{
    private float _timer; // Internal pulse tracker

    [Header("Glitch Settings")]
    [Tooltip("How often a new glitch block appears (in seconds)")]
    [SerializeField] private float glitchIntensity = 0.1f;

    [Tooltip("The prefab used as a glitch particle (e.g., a simple cube)")]
    [SerializeField] public GameObject cubePrefab;

    [Tooltip("Array of colors that will be randomly assigned to glitch blocks")]
    [SerializeField] private Color[] glitchColors = { Color.cyan, Color.magenta, Color.yellow, Color.green, Color.black };

    void Update()
    {
        // Accumulate time since the last frame
        _timer += Time.deltaTime;

        // Check if it's time to spawn a new glitch element
        if (_timer > glitchIntensity)
        {
            SpawnGlitchBlock();
            _timer = 0f; // Reset timer for the next cycle
        }
    }

    private void SpawnGlitchBlock()
    {
        if (cubePrefab == null) return;

        // Generate a random spawn position within a small sphere around the object
        Vector3 randomPos = transform.position + Random.insideUnitSphere * 0.5f;

        // Instantiate the glitch block
        GameObject newBlock = Instantiate(cubePrefab, randomPos, Quaternion.identity);

        // Randomize scale to create a "digital noise" look
        newBlock.transform.localScale = new Vector3(
            Random.Range(0.2f, 1.2f),
            Random.Range(0.1f, 0.5f),
            Random.Range(0.2f, 1.2f)
        );

        // Access the renderer to apply a random glitch color
        Renderer blockRenderer = newBlock.GetComponent<Renderer>();
        if (blockRenderer != null)
        {
            // Pick a random color from the defined array
            blockRenderer.material.color = glitchColors[Random.Range(0, glitchColors.Length)];
        }

        // Destroy the block quickly to simulate flickering and optimize memory
        Destroy(newBlock, 0.15f);
    }
}