using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab; // Prefab for walls
    [SerializeField] private int mazeWidth = 10; // Width of the maze
    [SerializeField] private int mazeHeight = 10; // Height of the maze
    [SerializeField] private float cellSize = 1f; // Size of each cell
    [SerializeField] private float mazeRegenerationInterval = 10f; // Time interval to regenerate the maze
    [SerializeField] private float animationDuration = 1f; // Duration of sinking/rising animations
    [SerializeField] private float transitionPauseDuration = 1f; // Pause duration between transitions
    [SerializeField] private Player player; // Reference to the player object

    private bool[,] currentMaze; // Current maze grid
    private bool[,] newMaze; // New maze grid to compare against
    private System.Random random = new System.Random(); // Random generator
    private Dictionary<Vector2Int, GameObject> mazeObjects = new Dictionary<Vector2Int, GameObject>(); // Tracks maze objects

    private void Start()
    {
        StartCoroutine(RegenerateMazeRoutine());
    }

    private IEnumerator RegenerateMazeRoutine()
    {
        GenerateMaze(); // Generate the initial maze
        yield return StartCoroutine(InstantiateMazeWithAnimation());
        while (true)
        {
            yield return new WaitForSeconds(mazeRegenerationInterval - animationDuration);
            GenerateMaze(); // Generate a new maze
            yield return StartCoroutine(TransitionMaze());
        }
    }

    private void GenerateMaze()
    {
        newMaze = new bool[mazeWidth * 2 + 1, mazeHeight * 2 + 1];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int startCell = new Vector2Int(1, 1); // Start from the first cell
        stack.Push(startCell);

        newMaze[startCell.x, startCell.y] = true; // Set the starting cell as a path

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                Vector2Int chosenNeighbor = neighbors[random.Next(neighbors.Count)];

                // Carve a path between current and chosen neighbor
                Vector2Int betweenCell = (current + chosenNeighbor) / 2;
                newMaze[betweenCell.x, betweenCell.y] = true;
                newMaze[chosenNeighbor.x, chosenNeighbor.y] = true;

                stack.Push(chosenNeighbor);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 2),  // Up
            new Vector2Int(0, -2), // Down
            new Vector2Int(2, 0),  // Right
            new Vector2Int(-2, 0)  // Left
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = cell + dir;

            if (IsInBounds(neighbor) && !newMaze[neighbor.x, neighbor.y])
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool IsInBounds(Vector2Int cell)
    {
        return cell.x > 0 && cell.x < mazeWidth * 2 &&
               cell.y > 0 && cell.y < mazeHeight * 2;
    }

    private IEnumerator InstantiateMazeWithAnimation()
    {
        currentMaze = newMaze;
        mazeObjects.Clear();

        Vector3 playerPosition = player.transform.position;
        Vector2Int playerCell = new Vector2Int(
            Mathf.RoundToInt(playerPosition.x / cellSize),
            Mathf.RoundToInt(playerPosition.z / cellSize)
        );

        player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(playerCell.x * cellSize, 0, playerCell.y * cellSize), 1f);
        player.SetPaused(true);

        for (int x = 0; x < mazeWidth * 2 + 1; x++)
        {
            for (int y = 0; y < mazeHeight * 2 + 1; y++)
            {
                Vector3 position = new Vector3(x * cellSize, -1, y * cellSize);
                Vector2Int cell = new Vector2Int(x, y);

                // Skip spawning anything at the player's current cell
                if (cell == playerCell)
                {
                    continue;
                }

                // Instantiate walls for blocked cells only
                if (!currentMaze[x, y])
                {
                    GameObject wallBlock = Instantiate(wallPrefab, position, Quaternion.identity);
                    mazeObjects[cell] = wallBlock;
                    StartCoroutine(RiseBlock(wallBlock, animationDuration));
                }
            }
        }

        yield return new WaitForSeconds(animationDuration);
        player.SetPaused(false);
    }

    private IEnumerator TransitionMaze()
    {
        // Determine the player's current cell based on position
        Vector3 playerPosition = player.transform.position;
        Vector2Int playerCell = new Vector2Int(
            Mathf.RoundToInt(playerPosition.x / cellSize),
            Mathf.RoundToInt(playerPosition.z / cellSize)
        );

        // Snap player's position to the center of their cell
        player.transform.position = new Vector3(playerCell.x * cellSize, 0, playerCell.y * cellSize);
        player.SetPaused(true);

        HashSet<Vector2Int> processedCells = new HashSet<Vector2Int>();

        for (int x = 0; x < mazeWidth * 2 + 1; x++)
        {
            for (int y = 0; y < mazeHeight * 2 + 1; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);

                // Skip processing the player's current cell
                if (cell == playerCell)
                {
                    processedCells.Add(cell); // Mark as processed to avoid issues
                    continue;
                }

                bool newState = newMaze[x, y];
                bool currentState = currentMaze[x, y];

                // Skip cells that haven't changed
                if (newState == currentState)
                {
                    processedCells.Add(cell);
                    continue;
                }

                // Handle transition: remove old block and add new block if needed
                if (mazeObjects.TryGetValue(cell, out GameObject block))
                {
                    StartCoroutine(SinkBlock(block, animationDuration));
                    mazeObjects.Remove(cell);
                }

                if (!newState) // Only instantiate walls for blocked cells
                {
                    Vector3 position = new Vector3(x * cellSize, -1, y * cellSize);
                    GameObject newBlock = Instantiate(wallPrefab, position, Quaternion.identity);
                    mazeObjects[cell] = newBlock;
                    StartCoroutine(RiseBlock(newBlock, animationDuration));
                }

                processedCells.Add(cell);
            }
        }

        // Ensure all cells are processed
        int expectedCellCount = (mazeWidth * 2 + 1) * (mazeHeight * 2 + 1);
        if (processedCells.Count != expectedCellCount)
        {
            Debug.LogWarning($"Processed cells ({processedCells.Count}) do not match expected count ({expectedCellCount}).");
        }

        currentMaze = newMaze; // Update reference to the new maze
        yield return new WaitForSeconds(animationDuration);
        yield return new WaitForSeconds(transitionPauseDuration);
        player.SetPaused(false);
    }

    private IEnumerator RiseBlock(GameObject block, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = block.transform.position;
        Vector3 endPos = new Vector3(startPos.x, 0, startPos.z);

        while (elapsedTime < duration)
        {
            block.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        block.transform.position = endPos;
    }

    private IEnumerator SinkBlock(GameObject block, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = block.transform.position;
        Vector3 endPos = new Vector3(startPos.x, -1, startPos.z);

        while (elapsedTime < duration)
        {
            block.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(block);
    }
}
