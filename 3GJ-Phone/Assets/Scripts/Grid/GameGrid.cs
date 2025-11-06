using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("UI")]
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Text buttonText;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float raycastDistance = 2f;

    private GameObject selectedTile;
    private bool isPlanted = false;

    [Header("Highlight")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;
    private Renderer lastHighlightedRenderer;

    void Start()
    {
        mainCam = Camera.main;
        creatingField = true;

        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }

        for (int i = 0; i < columnLength * rowLength; i++)
        {
            float xPos = x_space * (i % columnLength);
            float zPos = z_space * (i / columnLength);
            Instantiate(grass, new Vector3(xPos, 0, zPos), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!gotGrid)
        {
            currentGrid = GameObject.FindGameObjectsWithTag("grid");
            gotGrid = true;
        }

        if (player != null)
        {
            CheckTileUnderPlayer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            TryRaycast(mousePos);
        }
    }

    private void CheckTileUnderPlayer()
    {
        Ray ray = new Ray(player.position + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("grid"))
            {
                if (selectedTile != hit.collider.gameObject)
                {
                    
                    if (lastHighlightedRenderer != null)
                    {
                        lastHighlightedRenderer.material.color = defaultColor;
                    }

                    selectedTile = hit.collider.gameObject;

                   
                    Renderer rend = hit.collider.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.material.color = highlightColor;
                        lastHighlightedRenderer = rend;
                    }

                    
                    if (HasPlant(selectedTile))
                    {
                        ShowButton("Water");
                        isPlanted = true;
                    }
                    else
                    {
                        ShowButton("Plant");
                        isPlanted = false;
                    }
                }
            }
        }
        else
        {
            
            if (lastHighlightedRenderer != null)
            {
                lastHighlightedRenderer.material.color = defaultColor;
                lastHighlightedRenderer = null;
            }

            if (selectedTile != null)
            {
                HideButton();
                selectedTile = null;
            }
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
                selectedTile = hit.collider.gameObject;

                if (HasPlant(selectedTile))
                {
                    ShowButton("Water");
                    isPlanted = true;
                }
                else
                {
                    ShowButton("Plant");
                    isPlanted = false;
                }
            }
        }
    }

    private void ShowButton(string text)
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(true);
            if (buttonText != null)
            {
                buttonText.text = text;
            }
        }
    }

    private void HideButton()
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
        }
    }

    private void OnActionButtonClicked()
    {
        if (selectedTile == null) return;

        if (isPlanted)
        {
            Debug.Log("Watered!");
            WaterPlant(selectedTile);
        }
        else
        {
            Destroy(selectedTile.gameObject);
            Debug.Log("Planted!");
            Instantiate(field, selectedTile.transform.position, Quaternion.identity);

            
        }

        HideButton();
        selectedTile = null;

        if (lastHighlightedRenderer != null)
        {
            lastHighlightedRenderer.material.color = defaultColor;
            lastHighlightedRenderer = null;
        }
    }

    private bool HasPlant(GameObject tile)
    {
        Collider[] colliders = Physics.OverlapSphere(tile.transform.position, 0.1f);
        foreach (Collider col in colliders)
        {
            if (col.gameObject != tile && col.CompareTag("Plant"))
            {
                return true;
            }
        }
        return false;
    }

    private void WaterPlant(GameObject tile)
    {
       
    }

    public void EnablePlanting()
    {
        creatingField = true;
    }
}
