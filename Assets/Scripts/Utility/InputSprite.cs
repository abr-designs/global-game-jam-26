using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite keyboardInputSprite;
    [SerializeField]
    private Sprite gamepadInputSprite;

    private Sprite m_currentSprite;

    private SpriteRenderer m_spriteRenderer;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(false);
    }

    void Update()
    {
        InputDevice lastDevice = InputSystem.GetDevice<InputDevice>();
        if (lastDevice is Gamepad)
        {
            SetSprite(true);
        }
        else
        {
            SetSprite(false);
        }
    }



    private void SetSprite(bool isGamepad)
    {
        var sprite = isGamepad ? gamepadInputSprite : keyboardInputSprite;
            
        if (m_spriteRenderer != null && sprite != m_currentSprite)
        {
            m_spriteRenderer.sprite = sprite;
            m_currentSprite = sprite;
        }
    }

}
