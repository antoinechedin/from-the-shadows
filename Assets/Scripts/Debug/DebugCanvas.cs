using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private bool active = false;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
    void Update()
    {
        if (GameManager.Instance.Debuging != active)
        {
            active = GameManager.Instance.Debuging;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
#endif
}
