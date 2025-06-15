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

    
    // Eğer arcade  için “herhangi bir tuşa basınca join” isterseniz, o tuşu kontrol edeceğiz.
    // Biz “herhangi tuş” mantığı: gamepad için herhangi bir buton.
    // Ancak Input System’de “any button” algılamak için Gamepad.current.anyKey? yok; 
    // Bunun yerine her Gamepad.all içindeki ButtonControl’ları kontrol etmek gerekir.
    // Burada örnek basit: gamepad üzerindeki dpad veya South/East tuşlarına basınca join.

    private void Start()
    {
        // Başlangıçta durumları güncelle
        UpdateUI();
        // Play butonuna listener ekle
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        // Eğer sahnedeyken önceden bir instance kalmışsa reset et:
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
        // Gamepad.all listesinde her bağlı gamepad için:
        foreach (var gp in Gamepad.all)
        {
            // Player1 join değilse, gp ile herhangi tuşa basma durumunu kontrol et
            if (!CoopGameManager.Instance.Player1Joined)
            {
                if (AnyGamepadButtonPressedThisFrame(gp))
                {
                    bool ok = CoopGameManager.Instance.AssignPlayer1Gamepad(gp);
                    if (ok)
                    {
                        Debug.Log("Player1 joined with gamepad: " + gp.displayName);
                        continue; // diğer gp ile Player2 olmasın
                    }
                }
            }
            // Player2 join değilse ve gp farklı bir cihaz ise:
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
        // Basitçe: bazı yaygın butonlara bakalım: South, East, West, North, dpad yönleri, start/select vs.
        // Daha kapsamlı için tüm butonları dönebilirsiniz.
        if (gp.buttonSouth.wasPressedThisFrame) return true;
        if (gp.buttonEast.wasPressedThisFrame) return true;
        if (gp.buttonWest.wasPressedThisFrame) return true;
        if (gp.buttonNorth.wasPressedThisFrame) return true;
        if (gp.dpad.up.wasPressedThisFrame) return true;
        if (gp.dpad.down.wasPressedThisFrame) return true;
        if (gp.dpad.left.wasPressedThisFrame) return true;
        if (gp.dpad.right.wasPressedThisFrame) return true;
        // Diğer butonlar isterseniz ekleyin: start, select, shoulder vb. Ancak arcade kısıtlaması varsa sadece istediğiniz butonlar.
        return false;
    }

    private void DetectKeyboardJoins()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Player1 için WASD: eğer join değilse ve herhangi WASD tuşuna basıldıysa join
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
        // Player2 için ok tuşları: eğer join değilse ve herhangi arrow tuşuna basıldıysa join
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
        // Player1 durumu
        if (CoopGameManager.Instance.Player1Joined)
        {
            if (CoopGameManager.Instance.player1Gamepad != null)
                player1StatusText.text = "Player 1: Joined (Gamepad)";
            else if (CoopGameManager.Instance.player1KeyboardScheme == KeyboardControlScheme.WASD)
                player1StatusText.text = "Player 1: Joined (WASD)";
        }
        else
        {
            player1StatusText.text = "Player 1: Not Joined";
        }
        // Player2 durumu
        if (CoopGameManager.Instance.Player2Joined)
        {
            if (CoopGameManager.Instance.player2Gamepad != null)
                player2StatusText.text = "Player 2: Joined (Gamepad)";
            else if (CoopGameManager.Instance.player2KeyboardScheme == KeyboardControlScheme.Arrows)
                player2StatusText.text = "Player 2: Joined (Arrows)";
        }
        else
        {
            player2StatusText.text = "Player 2: Not Joined";
        }
        // Play butonu sadece her iki oyuncu da join ise interactable olsun
        if (playButton != null)
        {
            playButton.interactable = (CoopGameManager.Instance.Player1Joined && CoopGameManager.Instance.Player2Joined);
        }
    }

    private void OnPlayClicked()
    {
        // Sahne geçişi: her iki oyuncu join ise
        if (CoopGameManager.Instance.Player1Joined && CoopGameManager.Instance.Player2Joined)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CoOpGameScene");
        }
        else
        {
            // Hata mesajı gösterebilirsiniz
            Debug.Log("Both players must join before playing.");
        }
    }
}
