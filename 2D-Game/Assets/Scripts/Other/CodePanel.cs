using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    [SerializeField]
    Text codeText;
    [SerializeField]
    GameObject codelock;
    private bool correctCode = false;
    string code = "";
    private void Start()
    {
        codelock.SetActive(false);
        codeText.text = code;
    }
    private void Update()
    {
        if (code == "1234")
        {
            codeText.color = Color.green;
            //StartCoroutine(CheckCode(true));
            correctCode = true;
        }
        else if (code.Length == 4 && code != "1234")
        {
            codeText.color = Color.red;
            //StartCoroutine(CheckCode(false));
        }
    }

    /*private IEnumerator CheckCode(bool check)
    {
        if (check)
        {
            yield return new WaitForSeconds(2f);
            codelock.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            code = "";
        }
        codeText.color = Color.white;
    }*/
    public bool GetLockStatus()
    {
        return correctCode;
    }
    public void Add(string add)
    {
        code += add;
    }

}
