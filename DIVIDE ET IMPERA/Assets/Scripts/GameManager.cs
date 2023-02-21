using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;
    public GameManager Instance { get { return _instance; } }

    void Start()
    {
        _instance = this;
    }

    void Update()
    {

    }
}
