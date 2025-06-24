using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 2;
    public float horizontalSpeed = 3;

    public float laneWidth = 3; // Jarak antar lajur
    private int currentLane = 0; // Lajur saat ini (-1 untuk kiri, 0 untuk tengah, 1 untuk kanan)
    private Vector3 targetPosition; // Posisi tujuan player setelah swipe

    public int minLane = -1;
    public int maxLane = 1;

    private PlayerControls playerControls;
    private Vector2 touchStartPosition;
    private float minSwipeDistance = 100; // Jarak minimum swipe agar dianggap valid (dalam pixel)


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Aktifkan Action Map "Gameplay"
        playerControls.Gameplay.Enable();

        // "Langganan" ke event. Saat aksi PrimaryContact dimulai (jari menyentuh layar), panggil fungsi StartTouch.
        playerControls.Gameplay.PrimaryContact.started += ctx => StartTouch(ctx);
        // Saat aksi PrimaryContact selesai (jari diangkat), panggil fungsi EndTouch.
        playerControls.Gameplay.PrimaryContact.canceled += ctx => EndTouch(ctx);
    }
    private void OnDisable()
    {
        // Hentikan "langganan" untuk mencegah error dan memory leak
        playerControls.Gameplay.PrimaryContact.started -= ctx => StartTouch(ctx);
        playerControls.Gameplay.PrimaryContact.canceled -= ctx => EndTouch(ctx);

        // Non-aktifkan Action Map
        playerControls.Gameplay.Disable();
    }


    private void Start()
    {
        targetPosition = transform.position;
    }

    // Fungsi yang dipanggil saat jari menyentuh layar
    private void StartTouch(InputAction.CallbackContext context)
    {
        // Baca dan simpan posisi awal sentuhan dari aksi PrimaryPosition
        touchStartPosition = playerControls.Gameplay.PrimaryPosition.ReadValue<Vector2>();
    }

    // Fungsi yang dipanggil saat jari diangkat dari layar
    private void EndTouch(InputAction.CallbackContext context)
    {
        // Baca posisi akhir sentuhan
        Vector2 touchEndPosition = playerControls.Gameplay.PrimaryPosition.ReadValue<Vector2>();

        // Hitung perbedaan/delta
        Vector2 swipeDelta = touchEndPosition - touchStartPosition;

        // Logika swipe tetap sama seperti sebelumnya
        if (swipeDelta.magnitude > minSwipeDistance)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0) MoveRight();
                else MoveLeft();
            }
        }
    }

    void Update()
    {
        // Logika pergerakan player tidak perlu diubah
        transform.position += Vector3.forward * playerSpeed * Time.deltaTime;

        Vector3 finalTargetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalTargetPosition, horizontalSpeed * Time.deltaTime);
    }

    void MoveLeft()
    {
        if (currentLane > minLane)
        {
            currentLane--;
            UpdateTargetPosition();
        }
    }

    void MoveRight()
    {
        if (currentLane < maxLane)
        {
            currentLane++;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        // Hitung posisi x target berdasarkan lajur saat ini dan lebar lajur
        float targetX = currentLane * laneWidth;

        // Atur posisi target dengan mempertahankan posisi y dan z dari transform saat ini
        // Kita hanya ingin mengubah sumbu X
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
