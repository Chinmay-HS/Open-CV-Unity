using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenCaptureProtection : MonoBehaviour
{
    private const int WDA_NONE = 0x00000000;
    private const int WDA_MONITOR = 0x0000001;

    [DllImport("user32.dll")]
    private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    void Start()
    {
        SetWindowAffinity();
    }

    void SetWindowAffinity()
    {
        IntPtr hwnd = GetActiveWindow();
        if (hwnd != IntPtr.Zero)
        {
            bool result = SetWindowDisplayAffinity(hwnd, WDA_MONITOR);
            if (!result)
            {
                Debug.LogError("Failed to set window display affinity.");
            }
            else
            {
                Debug.Log("Window display affinity set successfully.");
            }
        }
        else
        {
            Debug.LogError("Failed to get active window handle.");
        }
    }
}