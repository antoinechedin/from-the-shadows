using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode goLeft = KeyCode.Q;
    public KeyCode goRight = KeyCode.D;

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        transform.Translate(hor * new Vector3(1, 0, 0));
    }
}
