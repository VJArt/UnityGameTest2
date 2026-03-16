using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action JumpInputPressed;
    public event Action LeftMouseClick;

    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private bool _mouseVisible;

    
    void Awake()
    {
        
    }

    void Update()
    {
        
    }
}
