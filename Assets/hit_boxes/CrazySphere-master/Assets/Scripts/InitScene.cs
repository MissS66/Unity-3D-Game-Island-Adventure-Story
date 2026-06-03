using UnityEngine;
using UnityEngine.EventSystems;
public class InitScene : MonoBehaviour
{
    private GameObject goPlane;

    [Header("贴图")]
    public Texture cubeTexture;
    public Texture sphereTexture;

    [Header("发射设置")]
    public float shootForce = 45f;
    public float bulletMass = 3f;
    public float cubeMass = 0.5f;

    void Start()
    {
        goPlane = GameObject.Find("Plane");

        CreateCubes();
    }

    void CreateCubes()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject goCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                goCube.transform.position = new Vector3(i - 1, j, -1);

                Rigidbody cubeRb = goCube.AddComponent<Rigidbody>();
                cubeRb.mass = cubeMass;

                goCube.AddComponent<AutoDestroy>();

                if (cubeTexture != null)
                {
                    Renderer r = goCube.GetComponent<Renderer>();
                    r.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    r.material.mainTexture = cubeTexture;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (Camera.main == null)
            {
                Debug.LogError("场景中没有 MainCamera 标签的摄像机！");
                return;
            }

            GameObject goBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            goBullet.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            goBullet.transform.localScale = Vector3.one * 0.6f;

            Rigidbody bulletRb = goBullet.AddComponent<Rigidbody>();
            bulletRb.mass = bulletMass;
            bulletRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            goBullet.AddComponent<AutoDestroy>();

            if (sphereTexture != null)
            {
                Renderer r = goBullet.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                r.material.mainTexture = sphereTexture;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bulletRb.AddForce(ray.direction.normalized * shootForce, ForceMode.Impulse);

            if (goPlane != null)
            {
                AudioSource audio = goPlane.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("欢迎试玩 CrazySphere（疯狂击箱子）游戏");
    }
}