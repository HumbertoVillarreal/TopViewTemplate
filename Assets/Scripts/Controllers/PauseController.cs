using UnityEngine;

public class PauseController : MonoBehaviour
{

    public static bool IsGamePaused { get; private set; } = false;
    public static bool IsMenuOpen { get; private set; } = false;
    public static bool IsDialogOpen { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
    }

    public static void SetMenuOpen(bool pause)
    {
        IsMenuOpen = pause;
    }

    public static void SetDialogOpen(bool pause)
    {
        IsDialogOpen = pause;
    }

}
