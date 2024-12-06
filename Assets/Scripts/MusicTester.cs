using UnityEngine;

public class MusicTester : MonoBehaviour
{
    public enum MusicLevel
    {
        Main,
        KeyboardLevel,
        GPULevel,
        PowerLevel,
        MotherboardLevel
    }

    [Header("Select Music Level")]
    public MusicLevel selectedLevel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchMusic();
        }
    }

    private void SwitchMusic()
    {
        switch (selectedLevel)
        {
            case MusicLevel.Main:
                AudioManager.Instance.ChangeLevelMusic("main");
                Debug.Log("Changed to Main music.");
                break;
            case MusicLevel.KeyboardLevel:
                AudioManager.Instance.ChangeLevelMusic("KeyboardLevel");
                Debug.Log("Changed to KeyboardLevel music.");
                break;
            case MusicLevel.GPULevel:
                AudioManager.Instance.ChangeLevelMusic("GPULevel");
                Debug.Log("Changed to GPULevel music.");
                break;
            case MusicLevel.PowerLevel:
                AudioManager.Instance.ChangeLevelMusic("PowerLevel");
                Debug.Log("Changed to PowerLevel music.");
                break;
            case MusicLevel.MotherboardLevel:
                AudioManager.Instance.ChangeLevelMusic("MotherboardLevel");
                Debug.Log("Changed to MotherboardLevel music.");
                break;
        }
    }
}