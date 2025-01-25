using System.Collections;
using UnityEngine;

public class LevelChangeLever : Interactable
{
    [SerializeField] private GameObject MainBoard;
    [SerializeField] private GameObject KeyboardLevel;
    [SerializeField] private GameObject PowerSupplyLevel;
    [SerializeField] private GameObject GPULevel;
    [SerializeField] private MazeGenerator mazeGenerator;

    [SerializeField] private GameObject KeyboardLevelInitState;
    [SerializeField] private GameObject PowerSupplyLevelInitState;
    [SerializeField] private GameObject GPULevelInitState;

    public GameObject keyboardHint;
    public GameObject powerSupplyHint;
    public GameObject gpuHint;

    [SerializeField] private Player player;

    [SerializeField] private GameObject leverHandle;
    private Quaternion stateOne = Quaternion.Euler(270.019775f, 0, 0);
    private Quaternion stateTwo = Quaternion.Euler(319.552032f, 180, 180);
    private Quaternion targetRotation;
    private float rotationSpeed = 2f;

    private bool isTransitioning = false;

    private void Start()
    {
        MainBoard.SetActive(true);
        KeyboardLevel.SetActive(false);
        PowerSupplyLevel.SetActive(false);
        GPULevel.SetActive(false);

        targetRotation = stateOne; // Set the initial target rotation
        AudioManager.Instance.PlayMusic(AudioManager.Instance.MotherboardLvl);
        //AudioManager.Instance.PlaySFX(AudioManager.Instance.leverMusic);
    }

    public override void Interact(Player player)
    {
        if (!isTransitioning)
        {
            GameObject currentLevel = GetCurrentLevel(); // Your method to fetch the current level
            GameObject nextLevel = GetNextLevel();       // Your method to fetch the next level

            if (currentLevel != null && nextLevel != null)
            {
                // Pass 'true' for left-to-right transitions
                StartCoroutine(LevelTransitionRoutine(currentLevel, nextLevel, true));
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!isTransitioning)
        {
            GameObject currentLevel = GetCurrentLevel(); // Your method to fetch the current level
            GameObject nextLevel = GetPreviousLevel();   // Your method to fetch the previous level

            if (currentLevel != null && nextLevel != null)
            {
                // Pass 'false' for right-to-left transitions
                StartCoroutine(LevelTransitionRoutine(currentLevel, nextLevel, false));
            }
        }
    }
    public override void ShowMessageHoverUI(string hoverUIMessage)
    {
        base.ShowMessageHoverUI(hoverUIMessage);
        UIManager.Instance.ShowHoverUI(hoverUIMessage);
    }

    public override void HideMessageHoverUI()
    {
        base.HideMessageHoverUI();
        UIManager.Instance.HideHoverUI();
    }
    private void ToggleRotationState()
    {
        targetRotation = targetRotation == stateOne ? stateTwo : stateOne;
        StartCoroutine(RotateLever());
    }

    private IEnumerator RotateLever()
    {
        while (Quaternion.Angle(leverHandle.transform.rotation, targetRotation) > 0.01f)
        {
            leverHandle.transform.rotation = Quaternion.Lerp(
                leverHandle.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }
        leverHandle.transform.rotation = targetRotation;
    }

    private IEnumerator LevelTransitionRoutine(GameObject currentLevel, GameObject nextLevel, bool toRight, float liftTime = 1f, float shiftTime = 2f, float landTime = 1f)
    {
        isTransitioning = true;
        player.SetPaused(true);
        Debug.Log("Transitioning levels...");

        Vector3 leftUpPosition = new Vector3(0f, 2f, -2f);
        Vector3 rightUpPosition = new Vector3(0f, 2f, 2f);
        Vector3 liftPosition = new Vector3(0f, 5f, 0f);
        Vector3 leftDownPosition = new Vector3(0f, 0f, -2f);
        Vector3 rightDownPosition = new Vector3(0f, 0f, 2f);
        Vector3 currentLevelStartPosition = currentLevel.transform.localPosition;

        if (currentLevelStartPosition != Vector3.zero) Debug.Log("Current level position not origin: " + currentLevelStartPosition);

        nextLevel.SetActive(true);
        if (currentLevel == KeyboardLevel)
        {
            mazeGenerator.DespawnLevel();
            mazeGenerator.gameObject.SetActive(false);
        }

        float elapsedTime = 0f;

        // lift levels with acceleration
        while (elapsedTime < liftTime)
        {
            float t = elapsedTime / liftTime;
            float acceleratedT = Mathf.Pow(t, 2); // Accelerate at the start

            if (toRight)
            {
                currentLevel.transform.localPosition = Vector3.Lerp(currentLevelStartPosition, liftPosition, acceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(rightDownPosition, rightUpPosition, acceleratedT);
            }
            else
            {
                currentLevel.transform.localPosition = Vector3.Lerp(currentLevelStartPosition, liftPosition, acceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(leftDownPosition, leftUpPosition, acceleratedT);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.whooshLvlSwitch);
        // shift levels with acceleration
        while (elapsedTime < shiftTime)
        {
            float t = elapsedTime / shiftTime;
            float acceleratedT = Mathf.Pow(t, 2); // Accelerate at the start

            if (toRight)
            {
                currentLevel.transform.localPosition = Vector3.Lerp(liftPosition, leftUpPosition, acceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(rightUpPosition, liftPosition, acceleratedT);
            }
            else
            {
                currentLevel.transform.localPosition = Vector3.Lerp(liftPosition, rightUpPosition, acceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(leftUpPosition, liftPosition, acceleratedT);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // land levels with deceleration
        while (elapsedTime < landTime)
        {
            float t = elapsedTime / landTime;
            float deceleratedT = Mathf.Pow(t, 0.5f); // Decelerate towards the end

            if (toRight)
            {
                currentLevel.transform.localPosition = Vector3.Lerp(leftUpPosition, leftDownPosition, deceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(liftPosition, Vector3.zero, deceleratedT);
            }
            else
            {
                currentLevel.transform.localPosition = Vector3.Lerp(rightUpPosition, rightDownPosition, deceleratedT);
                nextLevel.transform.localPosition = Vector3.Lerp(liftPosition, Vector3.zero, deceleratedT);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions
        if (toRight)
        {
            currentLevel.transform.localPosition = leftDownPosition;
            nextLevel.transform.localPosition = Vector3.zero;
        }
        else
        {
            currentLevel.transform.localPosition = rightDownPosition;
            nextLevel.transform.localPosition = Vector3.zero;
        }

        if (nextLevel == KeyboardLevel)
        {
            mazeGenerator.gameObject.SetActive(true);
            mazeGenerator.RespawnLevel();
        }
        else if (currentLevel == KeyboardLevel)
        {
            mazeGenerator.DespawnLevel();
            mazeGenerator.gameObject.SetActive(false);
        }

        // Deactivate previous level
        currentLevel.SetActive(false);
        player.SetPaused(false);

        Debug.Log("Transition complete. New level: " + nextLevel.name);
        isTransitioning = false;
    }

    private void SwitchToNextLevel()
    {
        if (MainBoard.activeSelf)
        {
            MainBoard.SetActive(false);
            KeyboardLevel.SetActive(true);
            mazeGenerator.gameObject.SetActive(true);
            mazeGenerator.RespawnLevel();
        }
        else if (KeyboardLevel.activeSelf)
        {
            mazeGenerator.DespawnLevel();
            mazeGenerator.gameObject.SetActive(false);
            KeyboardLevel.SetActive(false);
            PowerSupplyLevel.SetActive(true);
        }
        else if (PowerSupplyLevel.activeSelf)
        {
            PowerSupplyLevel.SetActive(false);
            GPULevel.SetActive(true);
        }
        else if (GPULevel.activeSelf)
        {
            GPULevel.SetActive(false);
            MainBoard.SetActive(true);
        }
    }

    private void SwitchToPreviousLevel()
    {
        if (MainBoard.activeSelf)
        {
            MainBoard.SetActive(false);
            GPULevel.SetActive(true);
        }
        else if (GPULevel.activeSelf)
        {
            GPULevel.SetActive(false);
            PowerSupplyLevel.SetActive(true);
        }
        else if (PowerSupplyLevel.activeSelf)
        {
            PowerSupplyLevel.SetActive(false);
            KeyboardLevel.SetActive(true);
            mazeGenerator.gameObject.SetActive(true);
            mazeGenerator.RespawnLevel();
        }
        else if (KeyboardLevel.activeSelf)
        {
            mazeGenerator.DespawnLevel();
            mazeGenerator.gameObject.SetActive(false);
            KeyboardLevel.SetActive(false);
            MainBoard.SetActive(true);
        }
    }

    public GameObject GetCurrentLevel()
    {
        if (MainBoard.activeSelf) return MainBoard;
        if (KeyboardLevel.activeSelf) return KeyboardLevel;
        if (PowerSupplyLevel.activeSelf) return PowerSupplyLevel;
        if (GPULevel.activeSelf) return GPULevel;
        if (KeyboardLevelInitState.activeSelf) return KeyboardLevelInitState;
        if (PowerSupplyLevelInitState.activeSelf) return PowerSupplyLevelInitState;
        if (GPULevelInitState.activeSelf) return GPULevelInitState;

        Debug.LogWarning("No active level found!");
        return null;
    }

    private GameObject GetNextLevel()
    {
        if (MainBoard.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.keyboardLvl);
            return keyboardHint == null ? KeyboardLevel : KeyboardLevelInitState;
        }

        if (KeyboardLevel.activeSelf || KeyboardLevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.PowerLvl);
            return powerSupplyHint == null ? PowerSupplyLevel : PowerSupplyLevelInitState;
        }

        if (PowerSupplyLevel.activeSelf || PowerSupplyLevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.GPULvl);
            return gpuHint == null ? GPULevel : GPULevelInitState;
        }

        if (GPULevel.activeSelf || GPULevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.MotherboardLvl);
            return MainBoard;
        }

        Debug.LogWarning("No valid next level found!");
        return null;
    }

    private GameObject GetPreviousLevel()
    {
        if (MainBoard.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.GPULvl);
            return gpuHint == null ? GPULevel : GPULevelInitState;
        }

        if (GPULevel.activeSelf || GPULevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.PowerLvl);
            return powerSupplyHint == null ? PowerSupplyLevel : PowerSupplyLevelInitState;
        }

        if (PowerSupplyLevel.activeSelf || PowerSupplyLevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.keyboardLvl);
            return keyboardHint == null ? KeyboardLevel : KeyboardLevelInitState;
        }

        if (KeyboardLevel.activeSelf || KeyboardLevelInitState.activeSelf)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.MotherboardLvl);
            return MainBoard;
        }

        Debug.LogWarning("No valid previous level found!");
        return null;
    }
}
