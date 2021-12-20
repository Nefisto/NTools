using UnityEngine;

public static class MyDebug
{
    public static string Log (object message)
    {
        Debug.Log(message.ToString());
        return message.ToString();
    }
    
    public static string Log (string message)
    {
        Debug.Log(message);
        return message;
    }
}