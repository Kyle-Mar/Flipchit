using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCodeInput: MonoBehaviour{
    public void UpdateGameCode()
    {
        // The gamecode is updated after the text input field is submitted.
        GameCode.Code = GetComponent<TMP_InputField>().text;
        Debug.Log(GameCode.Code);
    }
}
// A static class so the gamecode can be easily accessed.
public static class GameCode
{
    public static string Code = "";
}
