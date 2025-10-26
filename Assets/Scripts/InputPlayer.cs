
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    private Vector2 direction;

    public void SetPlayerInput(InputAction.CallbackContext _context)
    {
        direction = _context.ReadValue<Vector2>();
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
