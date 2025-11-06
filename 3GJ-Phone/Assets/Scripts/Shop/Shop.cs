using UnityEngine;


public class Shop : MonoBehaviour
{
    [SerializeField] private int[] id;
    [SerializeField] private string[] productName;
    [SerializeField] private float[] price;

    [SerializeField] private int numberOfProducts;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject productPrefab;


    private void Start()
    {
        
    }


    private void Update()
    {
        
    }


    public void OpenShop()
    {
        shopWindow.SetActive(true);
    }

    public void CloseShop()
    {
        shopWindow.SetActive(false);
    }
}
