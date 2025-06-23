using UnityEngine;

/// <summary>
/// Serializable class for saving and loading player state.
/// </summary>
[System.Serializable]
public class SaveData
{
    public float playerPosX;   // Player's X position
    public float playerPosY;   // Player's Y position
    public float playerTime;   // Player's elapsed time
}