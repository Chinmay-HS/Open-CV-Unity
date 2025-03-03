using UnityEngine;
using System.Runtime.InteropServices;

public class AndroidScreenCaptureProtection : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    [DllImport("user32.dll")]
    private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

    void Start()
    {
        SetSecureFlag();
    }

    private void SetSecureFlag()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() => 
            {
                activity.Call("getWindow").Call("setFlags", 0x2000, 0x2000); // FLAG_SECURE = 0x2000
            }));
        }
        Debug.Log("Screen capture protection enabled.");
    }
#endif
}