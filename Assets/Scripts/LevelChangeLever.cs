using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelChangeLever : Interactable
{
    [SerializeField] private GameObject MainBoard;
    [SerializeField] private GameObject KeyboardLevel;
    [SerializeField] private GameObject PowerSupplyLevel;
    [SerializeField] private GameObject GPULevel;
    [SerializeField] private MazeGenerator mazeGenerator;

    [SerializeField] private Player player;

    private void Start()
    {
        MainBoard.SetActive(true);
        KeyboardLevel.SetActive(false);
        PowerSupplyLevel.SetActive(false);
        GPULevel.SetActive(false);
    }



    public override void Interact(Player player)
    {
        // going down
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
            //Debug.Log("Switching to  ");
            PowerSupplyLevel.SetActive(false);
            GPULevel.SetActive(true);
        }
        else if (GPULevel.activeSelf)
        {
            GPULevel.SetActive(false);
            MainBoard.SetActive(true);
        }
    }

    public override void InteractAlternate(Player player)
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


}
