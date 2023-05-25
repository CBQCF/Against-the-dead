using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private float time;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Image icon;
    [SerializeField] public Image background;
    
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= 5)
        {
            Destroy(gameObject);
        }
    }
}
