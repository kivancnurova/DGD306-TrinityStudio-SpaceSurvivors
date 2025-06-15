using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CoopSceneInitializer : MonoBehaviour
{
    public CoopPC player1;
    public CoopPC player2;

    private void Start()
    {
        var gm = CoopGameManager.Instance;
        if (gm == null) return;

        player1.Initialize(gm.player1Gamepad, gm.player1KeyboardScheme);
        player2.Initialize(gm.player2Gamepad, gm.player2KeyboardScheme);
    }
}