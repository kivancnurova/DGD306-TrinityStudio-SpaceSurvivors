using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Rigidbody2D))]
public class CoopPC : MonoBehaviour
{
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private float speed = 5f;

    // Hareket sınırları
    private Rect boundsP1 = new Rect(-7f, -3.5f, 14f, 2f);   //  x y w h
    private Rect boundsP2 = new Rect(-7f,  1.5f, 14f, 2f);

    // Atanmış cihaz/scheme
    private Gamepad   pad;
    private KeyboardControlScheme keyScheme;

    private Rigidbody2D rb;

    public void Initialize(Gamepad gpad, KeyboardControlScheme scheme)
    {
        pad       = gpad;          // null olabilir
        keyScheme = scheme;        // None / WASD / Arrows
    }

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start()
    {
        // Sahneye girerken kendimizi CoopGameManager’dan otomatik doldurabiliriz
        var gm = CoopGameManager.Instance;
        if (gm != null)
        {
            pad       = gm.GetDevice(playerIndex) as Gamepad;
            keyScheme = gm.GetScheme(playerIndex);
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = ReadInput();
        Move(input);
    }

    private Vector2 ReadInput()
    {
        // ❶ Gamepad varsa doğrudan onun d-pad’ini (veya sol çubuğunu) kullan
        if (pad != null)
            return pad.dpad.ReadValue();   // arcade’de d-pad varsa yeter

        // ❷ Klavyede hangi şema atanmışsa ona göre oku
        var kb = Keyboard.current;
        if (kb == null) return Vector2.zero;

        switch (keyScheme)
        {
            case KeyboardControlScheme.WASD:
                return new Vector2(
                    Bool(kb.dKey) - Bool(kb.aKey),
                    Bool(kb.wKey) - Bool(kb.sKey));
            case KeyboardControlScheme.Arrows:
                return new Vector2(
                    Bool(kb.rightArrowKey) - Bool(kb.leftArrowKey),
                    Bool(kb.upArrowKey)    - Bool(kb.downArrowKey));
            default:
                return Vector2.zero;
        }

        static int Bool(KeyControl k) => k.isPressed ? 1 : 0;
    }

    private void Move(Vector2 dir)
    {
        Vector2 delta = dir.normalized * speed * Time.fixedDeltaTime;
        Vector2 target = rb.position + delta;

        // Kendi kutumuza sıkıştır
        Rect r = (playerIndex == PlayerIndex.One) ? boundsP1 : boundsP2;
        target.x = Mathf.Clamp(target.x, r.xMin, r.xMax);
        target.y = Mathf.Clamp(target.y, r.yMin, r.yMax);

        rb.MovePosition(target);
    }
}
