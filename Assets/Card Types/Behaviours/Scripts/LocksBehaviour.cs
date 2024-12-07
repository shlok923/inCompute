using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocksBehaviour", menuName = "Card Behaviours/Locks")]

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineHelper");
                _instance = obj.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(obj); // Optional: persists across scenes
            }
            return _instance;
        }
    }

    public void StartHelperCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
public class LocksBehaviour : CardBehaviour
{
    private DialInteractor[] dialsToCorrect;
    private LevelChangeLever levelChangerInCard;
    private GameObject powerSupplyLevelInCard;

    public void SetDialsAndLevelChangerAndPSLevel(DialInteractor[] dials, LevelChangeLever levelChanger, GameObject powerSupplyLevel)
    {
        dialsToCorrect = dials;
        levelChangerInCard = levelChanger;
        powerSupplyLevelInCard = powerSupplyLevel;
    }

    public override bool canUse()
    {
        return levelChangerInCard.GetCurrentLevel() == powerSupplyLevelInCard;
    }

    public override void Activate()
    {
        CoroutineHelper.Instance.StartHelperCoroutine(ActivateSequence());  
    }

    private IEnumerator ActivateSequence()
    {

        AudioManager.Instance.PlaySFX(AudioManager.Instance.lockCardUse);
        dialsToCorrect[0].ResetToOriginalAngle();
        dialsToCorrect[1].ResetToOriginalAngle();

        yield return new WaitForSeconds(1);

        // Turn the first dial
        dialsToCorrect[0].TurnDialMultipleTimes(6);

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // Turn the second dial
        dialsToCorrect[1].TurnDialMultipleTimes(5);
    }

    public override void Regenerate()
    {
        Debug.Log("Card of Freeze of can't be regenerated!");
    }
}
