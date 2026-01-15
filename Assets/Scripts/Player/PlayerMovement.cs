using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Reference Settings")]
    // Можно перенести эти ссылки в Awake или Start, если они всегда будут на том же объекте или у дочерних объектов
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private PlayerInputs playerInputs;

    [Header("Debug Settings")]
    [SerializeField] private bool isViewDebug = false;

    private Vector2 _movementInput;
    private bool _isMoving;
    private Vector2 _lookDirection;

    private void OnEnable()
    {
        playerInputs.OnMoveInput += HandleMoveInput;
    }

    private void OnDisable()
    {
        playerInputs.OnMoveInput -= HandleMoveInput;
    }

    void Update()
    {
        UpdateMovement();
        UpdateAnimation();
    }

    private void HandleMoveInput(Vector2 vector)
    {
        _movementInput = vector;
    }

    private void UpdateMovement()
    {
        Vector2 movement = _movementInput.normalized * moveSpeed;
        rb.linearVelocity = movement;

        _isMoving = movement.magnitude > 0;
    }

    private void UpdateAnimation()
    {
        if (bodyAnimator == null)
            return;

        bodyAnimator.SetBool("isMoving", _isMoving);
        if (_isMoving)
        {
            bodyAnimator.SetFloat("moveX", _movementInput.x);
            bodyAnimator.SetFloat("moveY", _movementInput.y);

            // Сохраняем последнее направление взгляда для idle
            bodyAnimator.SetFloat("LastMoveX", _lookDirection.x);
            bodyAnimator.SetFloat("LastMoveY", _lookDirection.y);
        }
    }
}
