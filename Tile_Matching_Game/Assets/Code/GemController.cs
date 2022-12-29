using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] List<GameObject> gemPrefab;
    private List<List<GameObject>> gems = new List<List<GameObject>>();

    public Vector3 FirstObj;
    public Vector3 SecondObj;

    private int _id;

    public bool active = false;
    public GameObject selected;
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
        System.Random rnd = new System.Random();
        int randomType = rnd.Next(0, 3);
        var gem = Instantiate(gemPrefab[randomType], new Vector2(widthPos + x, spawnHeight + heightPos + y), Quaternion.identity);
        gem.transform.SetParent(gameObject.transform.Find("Gems"));
        gem.name = "Gem " + x + " " + y;
        gem.GetComponent<Tile>().x = x;
        gem.GetComponent<Tile>().y = y;
        gem.GetComponent<Tile>().id = _id++;
        gems[x].Add(gem);
    }


    private void FindDups()
    {
        GameObject last = null;
        GameObject current = null;
        int streak = 0;
        List<List<GameObject>> found = new List<List< GameObject>>();
        List<GameObject> list = new List<GameObject>();
        for(int i = 0; i < gems.Count; i++)
        {
            last = gems[i][0];
            current = null;
            list.Clear();
            streak = 0;
            for(int j = 0; j < gems[i].Count; j++)
            {
                if (current != null) last = current;
                current = gems[i][j];
                if (last.GetComponent<Tile>().Type == current.GetComponent<Tile>().Type) 
                {
                    streak++;
                } else
                {
                    if(streak >= 3)
                    {
                        found.Add(new List<GameObject>(list));
                    }
                    streak = 1;
                    list.Clear();
                }
                list.Add(current);
            }
            if (streak >= 3) found.Add(new List<GameObject>(list));


        }

        for(int i = 0; i < found.Count; i++)
        {
            for(int j = 0; j < found[i].Count; j++)

            {
                Debug.Log(gems[found[i][j].GetComponent<Tile>().x][found[i][j].GetComponent<Tile>().y].name);
                gems[found[i][j].GetComponent<Tile>().x][found[i][j].GetComponent<Tile>().y].name = "a";
                Destroy(gems[found[i][j].GetComponent<Tile>().x][found[i][j].GetComponent<Tile>().y]);
            }
        }
        
        for(int i = 0; i < gems.Count; i++){
            for(int j = 0; j < gems[i].Count; j++)
            {
                if (gems[i][j].name == "a")
                {
                    gems[i].RemoveAt(j);
                    j--;
                }
            }
        }
       

        //gems[found[i][j].GetComponent<Tile>().x].RemoveAt(found[i][j].GetComponent<Tile>().y);
        
    }



    //Refill the board
    private void Refill()
    {
        for(int i = 0; i < width; i++)
        {
            int y = 6;
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
                if(active == false)
                {
                    //Select first
                    selected = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    active = true;

                } else
                {
                    //Select second and swap
                    int holder = selected.gameObject.GetComponent<Tile>().Type;
                    selected.gameObject.GetComponent<Tile>().Type = hit.collider.gameObject.GetComponent<Tile>().Type;
                    hit.collider.GetComponent<Tile>().Type = holder;
                    FindDups();
                    Refill();


                    active = false;
                }


                //FindDups();
                //Refill();

            }
        }
        
    }
}
