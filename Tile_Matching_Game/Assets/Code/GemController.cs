using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject gemPrefab;
    private List<List<GameObject>> gems = new List<List<GameObject>>();


    public Vector3 FirstObj;
    public Vector3 SecondObj;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < width; i++)
        {
            gems.Add(new List<GameObject>());
            Debug.Log("List created");
            for(int j = 0;  j < height; j++)
            {
                InstantiateGem(i,j);
                Debug.Log("gem created");
            }
        }
    }

    private void InstantiateGem(int x, int y)
    {
        int spawnHeight = 10;
        int widthPos = - width / 2;
        int heightPos = - height / 2;
        var gem = Instantiate(gemPrefab, new Vector2(widthPos + x, spawnHeight + heightPos + y), Quaternion.identity);
        gem.name = "Gem " + x + " " + y;
        gem.GetComponent<Tile>().x = x;
        gem.GetComponent<Tile>().y = y;
    }

    private void Find()
    {

    }

    //Refill the board
    private void Refill()
    {
        for(int i = 0; i < width; i++)
        {
            int y = 10;
            while (gems[i].Count < height)
            {
                InstantiateGem(i, y);
                y++;
                
            }


        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                

            }
        }
    }
}
