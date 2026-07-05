using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    public static PlayerMarker instance;
    void Awake()
    {
        if (instance != null) {Destroy(gameObject);return;}
        instance = this;
    }
}
