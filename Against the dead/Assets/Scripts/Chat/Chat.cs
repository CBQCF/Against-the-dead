using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Chat : NetworkBehaviour
{
    [SerializeField]
    private TMP_InputField _input;
    [SerializeField]
    private Button _send;
    private Player _player;

    void Start()
    {
        _player = GetComponent<Player>();
        _send.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (_input.text != String.Empty)
        {
        }

        _input.text = String.Empty;
    }
}
