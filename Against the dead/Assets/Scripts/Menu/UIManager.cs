using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Canvas _canvas;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        _canvas.enabled = gameObject.scene.Equals(SceneManager.GetActiveScene());
    }
}
