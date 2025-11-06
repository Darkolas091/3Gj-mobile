using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private int columnLength, rowLength;
    [SerializeField] private float x_space, z_space;
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject[] currentGrid;

    public bool gotGrid;

    [SerializeField] private GameObject field;
    [SerializeField] private GameObject hitted;

    private Camera mainCam;
    public bool creatingField;

    void Start()
    {
        mainCam = Camera.main;

        creatingField = true;

        for (int i = 0; i < columnLength * rowLength; i++)
        {
            float xPos = x_space * (i % columnLength);
            float zPos = z_space * (i / columnLength);

            Instantiate(grass, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        if (!gotGrid)
        {
            currentGrid = GameObject.FindGameObjectsWithTag("grid");
            gotGrid = true;
        }

        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            TryRaycast(touchPos);
        }

       
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            TryRaycast(mousePos);
        }
    }

    private void TryRaycast(Vector3 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("grid"))
            {
                Debug.Log("Pogodio grid: " + hit.collider.name);

                if (creatingField)
                {
                    Instantiate(field, hit.collider.transform.position, Quaternion.identity);
                    
                }
            }
        }
    }

    public void EnablePlanting()
    {
        creatingField = true;
    }
}
