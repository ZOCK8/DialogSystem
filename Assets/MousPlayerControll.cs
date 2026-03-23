using System.Collections;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousPlayerControll : MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody2d;
    public GameObject PlayerObj;
    public float sensitivity;
    public InputSystem_Actions inputActions;
    public Camera camera;
    public bool Selected;
    public Texture2D CursorSelected;
    public Texture2D CursorCanSelected;
    public float SelectDistance;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()

    {
        inputActions.Player.Look.performed += OnLookPerformed;
        inputActions.Player.Look.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLookPerformed;
        inputActions.Player.Look.Disable();
    }
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        UnityEngine.Vector2 look = context.ReadValue<UnityEngine.Vector2>();
        float mouseX = look.x * (sensitivity / 1.2f);
        float mouseY = look.y * sensitivity;
        if (Selected)
        {
            PlayerRigidbody2d.AddForceY(mouseY);
            PlayerRigidbody2d.AddForceX(mouseX);
        }
    }
    private void OnDrawGizmos()
    {
        if (PlayerObj != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerObj.transform.position, SelectDistance);
        }
        UnityEngine.Vector2 MousePosition = camera.ScreenToWorldPoint(new UnityEngine.Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0));
        Gizmos.DrawWireSphere(MousePosition, SelectDistance);
    }
    void FixedUpdate()
    {
        UnityEngine.Vector2 MousePosition = camera.ScreenToWorldPoint(new UnityEngine.Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0));
        float Distance = UnityEngine.Vector3.Distance(MousePosition, PlayerObj.transform.position);
        Debug.DrawLine(new UnityEngine.Vector3(MousePosition.x, MousePosition.y, 0), PlayerObj.transform.position, Color.red);
        if (Distance < SelectDistance)
        {
            Debug.Log("CanSelcet");
            Cursor.SetCursor(CursorCanSelected, UnityEngine.Vector2.zero, CursorMode.Auto);
        }
        if (Mouse.current.leftButton.isPressed && Distance < SelectDistance)
        {
            Cursor.SetCursor(CursorSelected, UnityEngine.Vector2.zero, CursorMode.Auto);
            Selected = true;
        }
        else if (Distance > SelectDistance)
        {
            Cursor.SetCursor(null, UnityEngine.Vector2.zero, CursorMode.Auto);
            Selected = false;
        }
    }
}
