using UnityEngine;

public static class Dbg
{
    public static void Log(string message)
    {
        Debug.Log(message);
    }
    public static void Log(string message, Color col)
    {
        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(col)}>{message}</color>");
    }
    public static void Log<T>(T message)
    {
        Dbg.Log(message.ToString());
    }
    public static void Log<T>(T message, Color col)
    {
        Dbg.Log(message.ToString(), col);
    }
}