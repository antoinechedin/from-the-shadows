using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject shadowPlayer;
    public GameObject lightPlayer;
    
    private void Update()
    {
        //TESTS
        if (Input.GetButton("Attack"))
        {
            Debug.Log("1");
            shadowPlayer.transform.Translate(1 * new Vector3(1, 0, 0) * Time.deltaTime);
        }
        if (Input.GetButton("Joy1_Attack"))
        {
            Debug.Log("2");
            lightPlayer.transform.Translate(1 * new Vector3(1, 0, 0) * Time.deltaTime);
        }
    }
}
