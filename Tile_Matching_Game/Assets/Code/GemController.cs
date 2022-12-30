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
        gems[x].Add(gem);
    }


    private void FindDups()
    {
        GameObject last = null;
        GameObject current = null;
        int streak = 0;
        List<List<GameObject>> found = new List<List<GameObject>>();
        List<GameObject> list = new List<GameObject>();
        // Vertical
        for(int i = 0; i < gems.Count; i++)
        {
            last = gems[i][0];
            last.GetComponent<Tile>().x = i;
            last.GetComponent<Tile>().y = 0;
            current = null;
            list.Clear();
            streak = 0;
            for(int j = 0; j < gems[i].Count; j++)
            {
                if (current != null) last = current;
                current = gems[i][j];
                current.GetComponent<Tile>().x = i;
                current.GetComponent<Tile>().y = j;
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

        // Horizontal
        for(int i = 0; i < gems[0].Count; i++)
        {
            last = gems[0][i];
            current = null;
            list.Clear();
            streak = 0;
            for(int j = 0; j < gems[i].Count; j++)
            {
                if (current != null) last = current;
                current = gems[j][i];
                if(last.GetComponent<Tile>().Type == current.GetComponent<Tile>().Type)
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
                

                gems[found[i][j].GetComponent<Tile>().x][found[i][j].GetComponent<Tile>().y].name = "deleted";
                Destroy(gems[found[i][j].GetComponent<Tile>().x][found[i][j].GetComponent<Tile>().y]);
                Debug.Log("DESTROYED");
            }
        }


        int indexi = 0;

        while(indexi < gems.Count)
        {
            int indexj = 0;
            while(indexj < gems[indexi].Count)
            {
                if (gems[indexi][indexj].name == "deleted")
                {
                    gems[indexi].RemoveAt(indexj);
                    continue;
                }
                indexj++;
            }
            indexi++;
        }
        
        found.Clear();
        list.Clear();

        
    }



    //Refill the board
    private void Refill()
    {
        for(int i = 0; i < width; i++)
        {
            while (gems[i].Count < height)
            {
                InstantiateGem(i, gems[i].Count);
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
                if (active == false)
                {
                    //Select first
                    selected = hit.collider.gameObject;
                    active = true;
                    Debug.Log("first selected");

                } else
                {
                    //Select second and swap
                    Debug.Log(selected.GetComponent<Tile>().Type);
                    Debug.Log(hit.collider.gameObject.GetComponent<Tile>().Type);
                    GameObject holder = selected.gameObject;
                    int type1 = hit.collider.gameObject.GetComponent<Tile>().Type;
                    int type2 = holder.gameObject.GetComponent<Tile>().Type;

                    selected.gameObject.GetComponent<SpriteRenderer>().sprite = gemPrefab[type1].GetComponent<SpriteRenderer>().sprite;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = gemPrefab[type2].GetComponent<SpriteRenderer>().sprite;
                    selected.gameObject.GetComponent<Tile>().x = hit.collider.gameObject.GetComponent<Tile>().x;
                    selected.gameObject.GetComponent<Tile>().y = hit.collider.gameObject.GetComponent<Tile>().y;
                    hit.collider.GetComponent<Tile>().x = holder.GetComponent<Tile>().x;
                    hit.collider.GetComponent<Tile>().y = holder.GetComponent<Tile>().y;


                    selected.gameObject.GetComponent<Tile>().Type = type1;
                    hit.collider.GetComponent<Tile>().Type = type2;
                    
                    active = false;

                    Debug.Log("Swapped");

                    /*
                    FindDups();
                    Refill();
                    */

                }



            }

        }
        if(Input.GetMouseButtonDown(1))
        {
            FindDups();
            Refill();
        }
        
    }
}
