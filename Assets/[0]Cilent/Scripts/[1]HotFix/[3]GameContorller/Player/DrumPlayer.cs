using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInput playerInput;
    void Start()
    {
      playerInput =  GetComponent<PlayerInput>();
      playerInput.InputData.Register(OnInputDataChange);
    }

    private void OnInputDataChange(PlayerInputData data)
    {
        Debug.Log(data.inputIndex);
        DrumSceneManager.Instance.CheckMusicPoint(data.inputIndex);
    }

    // Update is called once per frame
   
}
