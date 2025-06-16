using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class MenuOpener : MonoBehaviour
{
    public InputActionReference openMenuActionRef;

    public GameObject menuPanel;
    public GameObject firstOption;

    private void OnEnable()
    {
        if (openMenuActionRef != null)
        {
            openMenuActionRef.action.performed += OnOpenMenuPerformed;
            openMenuActionRef.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (openMenuActionRef != null)
        {
            openMenuActionRef.action.performed -= OnOpenMenuPerformed;
            openMenuActionRef.action.Disable();
        }
    }

    private void OnOpenMenuPerformed(InputAction.CallbackContext context)
    {
        OpenMenu();
    }

    private void OpenMenu()
    {
        if (menuPanel != null && Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            menuPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstOption);

        }
        else
        {
            Debug.LogWarning("Menu Panel referansı atanmamış veya zaman ölçeği zaten 0!");
        }
    }

    public void ResumeGame()
    {
        if (menuPanel != null)
        {
            Time.timeScale = 1f;
            menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Menu Panel referansı atanmamış!");

        }
    }

}
