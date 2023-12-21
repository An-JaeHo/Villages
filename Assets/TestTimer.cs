using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : MonoBehaviour
{
    public TMP_Text m_TextMeshPro;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //Debug.Log(time);
        m_TextMeshPro.text = time.ToString();
    }
}
