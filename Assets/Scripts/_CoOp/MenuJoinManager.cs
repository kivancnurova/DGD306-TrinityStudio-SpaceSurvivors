using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MenuJoinManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text player1StatusText;
    public TMP_Text player2StatusText;

    public Button playButton;


    private void Start()
    {
        UpdateUI();
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        if (CoopGameManager.Instance != null)
        {
            CoopGameManager.Instance.UnassignPlayer1();
            CoopGameManager.Instance.UnassignPlayer2();
        }
    }

    private void Update()
    {
        DetectGamepadJoins();
        DetectKeyboardJoins();
        UpdateUI();
    }

    private void DetectGamepadJoins()
    {
        foreach (var gp in Gamepad.all)
        {
            if (!CoopGameManager.Instance.Player1Joined)
            {
                if (AnyGamepadButtonPressedThisFrame(gp))
                {
                    bool ok = CoopGameManager.Instance.AssignPlayer1Gamepad(gp);
                    if (ok)
                    {
                        Debug.Log("Player1 joined with gamepad: " + gp.displayName);
                        continue;
                    }
                }
            }
            if (CoopGameManager.Instance.Player1Joined && !CoopGameManager.Instance.Player2Joined)
            {
                if (gp != CoopGameManager.Instance.player1Gamepad)
                {
                    if (AnyGamepadButtonPressedThisFrame(gp))
                    {
                        bool ok = CoopGameManager.Instance.AssignPlayer2Gamepad(gp);
                        if (ok)
                        {
                            Debug.Log("Player2 joined with gamepad: " + gp.displayName);
                        }
                    }
                }
            }
        }
    }

    private bool AnyGamepadButtonPressedThisFrame(Gamepad gp)
    {
        if (gp.buttonSouth.wasPressedThisFrame) return true;
        if (gp.buttonEast.wasPressedThisFrame) return true;
        if (gp.buttonWest.wasPressedThisFrame) return true;
        if (gp.buttonNorth.wasPressedThisFrame) return true;
        if (gp.dpad.up.wasPressedThisFrame) return true;
        if (gp.dpad.down.wasPressedThisFrame) return true;
        if (gp.dpad.left.wasPressedThisFrame) return true;
        if (gp.dpad.right.wasPressedThisFrame) return true;
        return false;
    }

    private void DetectKeyboardJoins()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (!CoopGameManager.Instance.Player1Joined)
        {
            if (kb.wKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame
                || kb.sKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame)
            {
                bool ok = CoopGameManager.Instance.AssignPlayer1KeyboardWASD();
                if (ok)
                    Debug.Log("Player1 joined with Keyboard WASD");
            }
        }
        if (CoopGameManager.Instance.Player1Joined && !CoopGameManager.Instance.Player2Joined)
        {
            if (kb.upArrowKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame
                || kb.leftArrowKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame)
            {
                bool ok = CoopGameManager.Instance.AssignPlayer2KeyboardArrows();
                if (ok)
                    Debug.Log("Player2 joined with Keyboard Arrows");
            }
        }
    }

    private void UpdateUI()
    {
        if (CoopGameManager.Instance.Player1Joined)
        {
            if (CoopGameManager.Instance.player1Gamepad != null)
            {
                player1StatusText.text = "Player 1: Joined (Gamepad)";
                player1StatusText.color = Color.green;
            }
            else if (CoopGameManager.Instance.player1KeyboardScheme == KeyboardControlScheme.WASD)
            {
                player1StatusText.text = "Player 1: Joined (WASD)";
                player1StatusText.color = Color.green;
            }
        }
        else
        {
            player1StatusText.text = "Player 1: Not Joined";
            player1StatusText.color = Color.red;
        }
        if (CoopGameManager.Instance.Player2Joined)
        {
            if (CoopGameManager.Instance.player2Gamepad != null)
            {
                player2StatusText.text = "Player 2: Joined (Gamepad)";
                player2StatusText.color = Color.green;
            }
            else if (CoopGameManager.Instance.player2KeyboardScheme == KeyboardControlScheme.Arrows)
            {
                player2StatusText.text = "Player 2: Joined (Arrows)";
                player2StatusText.color = Color.green;
            }
        }
            else
            {
                player2StatusText.text = "Player 2: Not Joined";
                player2StatusText.color = Color.red;
            }
        if (playButton != null)
        {
            playButton.interactable = (CoopGameManager.Instance.Player1Joined && CoopGameManager.Instance.Player2Joined);
        }
    }

    private void OnPlayClicked()
    {
        if (CoopGameManager.Instance.Player1Joined && CoopGameManager.Instance.Player2Joined)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CoOpGameScene");
        }
        else
        {
            Debug.Log("Both players must join before playing.");
        }
    }
}
