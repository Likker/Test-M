using UnityEngine;

public interface IJoystickController
{
    void OnTouchDown();

    void OnTouch(Vector2 direction);

    void OnTouchUp();
}
