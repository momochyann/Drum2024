using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class PlayerInputData
{
    public int inputIndex;
}
public interface PlayerInput
{
    public BindableProperty<PlayerInputData> InputData { get; set; }
}
public class SerialPortInput : MonoBehaviour, PlayerInput
{

    // Start is called before the first frame update

    public BindableProperty<PlayerInputData> InputData { get; set; } = new BindableProperty<PlayerInputData>(new PlayerInputData());

    void Start()
    {
        SerialPortUtilityManager.Instance.OnSerialDataReceived += OnDataReceived;
    }

    private void OnDataReceived(byte[] bytes)
    {
        string str = System.Text.Encoding.UTF8.GetString(bytes);
        string value = str.Split(':')[1];
        if (int.TryParse(value, out int inputIndex))
            InputData.Value = new PlayerInputData() { inputIndex = inputIndex+1 };
    }

}
