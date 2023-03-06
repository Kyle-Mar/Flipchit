using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCodeInput: MonoBehaviour{
    public void UpdateGameCode()
    {
        GameCode.Code = GetComponent<TMP_InputField>().text;
        Debug.Log(GameCode.Code);
    }
}
public static class GameCode
{
    public static string Code = "";
}
