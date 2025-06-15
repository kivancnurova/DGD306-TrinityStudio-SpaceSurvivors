using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyboardControlScheme
{
    None,
    WASD,
    Arrows
}

public enum PlayerIndex { One = 1, Two = 2 }

public partial class CoopGameManager : MonoBehaviour
{
    public static CoopGameManager Instance { get; private set; }

    [HideInInspector]
    public Gamepad player1Gamepad;
    [HideInInspector]
    public Gamepad player2Gamepad;

    [HideInInspector]
    public KeyboardControlScheme player1KeyboardScheme = KeyboardControlScheme.None;
    [HideInInspector]
    public KeyboardControlScheme player2KeyboardScheme = KeyboardControlScheme.None;

    public bool Player1Joined => (player1Gamepad != null) || (player1KeyboardScheme != KeyboardControlScheme.None);
    public bool Player2Joined => (player2Gamepad != null) || (player2KeyboardScheme != KeyboardControlScheme.None);

    public InputDevice GetDevice(PlayerIndex p)
        => p == PlayerIndex.One ? (InputDevice)player1Gamepad : player2Gamepad;

    public KeyboardControlScheme GetScheme(PlayerIndex p)
        => p == PlayerIndex.One ? player1KeyboardScheme : player2KeyboardScheme;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public bool AssignPlayer1Gamepad(Gamepad gp)
    {
        if (Player1Joined) return false;
        player1Gamepad = gp;
        return true;
    }

    public bool AssignPlayer2Gamepad(Gamepad gp)
    {
        if (Player2Joined) return false;
        player2Gamepad = gp;
        return true;
    }

    public bool AssignPlayer1KeyboardWASD()
    {
        if (Player1Joined) return false;
        player1KeyboardScheme = KeyboardControlScheme.WASD;
        return true;
    }

    public bool AssignPlayer2KeyboardArrows()
    {
        if (Player2Joined) return false;
        player2KeyboardScheme = KeyboardControlScheme.Arrows;
        return true;
    }

    public void UnassignPlayer1()
    {
        player1Gamepad = null;
        player1KeyboardScheme = KeyboardControlScheme.None;
    }
    public void UnassignPlayer2()
    {
        player2Gamepad = null;
        player2KeyboardScheme = KeyboardControlScheme.None;
    }
}
