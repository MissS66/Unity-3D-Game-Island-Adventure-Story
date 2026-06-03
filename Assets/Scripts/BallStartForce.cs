using UnityEngine;

public class BallStartForce : MonoBehaviour
{
    public float startForce = 300f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * startForce);
    }
}