using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;
    public static Map Get() { return instance; }
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(this.gameObject);

        InitPathmap();
    }
    public GameObject SmallDotPrefab;
    public GameObject LargeDotPrefab;

    public int dotCount = 0;
    public int DotCount { get { return dotCount; } }

    public List<SmallDot> smallDots = new List<SmallDot>();
    public List<BigDot> bigDots = new List<BigDot>();
    public List<PathmapTile> tiles = new List<PathmapTile>();
    public PathmapTile playerStartPos;
    public List<Cherry> cherry = new List<Cherry>();
    public int tileSize = 22;

    public GameObject SmallDotGroup;
    public GameObject BigDotGroup;

    public GameObject debugNodeOpen;
    public GameObject debugNodeClosed;


    // Start is called before the first frame update
    void Start()
    {        
        //InitDots();
        //initBigDots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InitPathmap()
    {
        string[] lines = System.IO.File.ReadAllLines("Assets/Data/map.txt");
        for (int y = 0; y < lines.Length; y++)
        {
            char[] line = lines[y].ToCharArray();
            for (int x = 0; x < line.Length; x++)
            {
                PathmapTile tile = new PathmapTile();
                tile.posX = x;
                tile.posY = -y;                                
                tiles.Add(tile);
                tile.blocking = line[x] == 'x';
                if(line[x] == 's')
                    playerStartPos = tile;
                else if(line[x] == '.')
                {
                    SmallDot dot = GameObject.Instantiate(SmallDotPrefab).GetComponent<SmallDot>();
                    dot.transform.SetParent(SmallDotGroup.transform);
                    dot.SetPosition(new Vector2(x * tileSize , -y * tileSize));
                    dot.OnCollected += OnDotColleted;
                    smallDots.Add(dot);
                    dotCount++;
                }
                else if(line[x] == 'o')
                {
                    BigDot dot = GameObject.Instantiate(LargeDotPrefab).GetComponent<BigDot>();
                    dot.transform.SetParent(BigDotGroup.transform);
                    dot.SetPosition(new Vector2(x * tileSize, -y * tileSize));
                    dot.OnCollected += OnDotColleted;
                    bigDots.Add(dot);
                    dotCount++;
                }
            }
        }
        return true;
    }


    internal bool TileIsValid(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY && !tiles[t].blocking)
                return true;
        }
        return false;
    }

    public List<PathmapTile> GetPath(int currentTileX, int currentTileY, int targetX, int targetY)
    {
        PathmapTile fromTile = GetTile(currentTileX, currentTileY);
        PathmapTile toTile = GetTile(targetX, targetY);

        for (int t = 0; t < tiles.Count; t++)
        {
            tiles[t].visited = false;
        }

        List<PathmapTile> path = new List<PathmapTile>();
        if (Pathfind(fromTile, toTile, path))
        {
            return path;
        }
        return null;
    }

    private bool Pathfind(PathmapTile fromTile, PathmapTile toTile, List<PathmapTile> path)
    {
        fromTile.visited = true;

        if (fromTile.blocking)
            return false;
        path.Add(fromTile);
        if (fromTile == toTile)
            return true;

        List<PathmapTile> neighbours = new List<PathmapTile>();

        PathmapTile up = GetTile(fromTile.posX, fromTile.posY - 1);
        if (up != null && !up.visited && !up.blocking && !path.Contains(up))
            neighbours.Insert(0, up);

        PathmapTile down = GetTile(fromTile.posX, fromTile.posY + 1);
        if (down != null && !down.visited && !down.blocking && !path.Contains(down))
            neighbours.Insert(0, down);

        PathmapTile right = GetTile(fromTile.posX + 1, fromTile.posY);
        if (right != null && !right.visited && !right.blocking && !path.Contains(right))
            neighbours.Insert(0, right);

        PathmapTile left = GetTile(fromTile.posX - 1, fromTile.posY);
        if (left != null && !left.visited && !left.blocking && !path.Contains(left))
            neighbours.Insert(0, left);

        for(int n = 0; n < neighbours.Count; n++)
        {
            PathmapTile tile = neighbours[n];

            path.Add(tile);

            if (Pathfind(tile, toTile, path))
                return true;

            path.Remove(tile);
        }

        return false;
    }

    public PathmapTile GetTile(int tileX, int tileY)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tileX == tiles[t].posX && tileY == tiles[t].posY)
                return tiles[t];
        }

        return null;
    }

    public void OnDotColleted(Item dot)
    {
        if(dot.tag == "SmallDot")
        {
            smallDots.Remove((SmallDot)dot);            
        }
        else if(dot.tag == "BigDot")
        {
            bigDots.Remove((BigDot)dot);
        }        
        dot.OnCollected -= OnDotColleted;
        dotCount--;
        Destroy(dot.gameObject);
    }


    bool HasIntersectedCherry(Vector2 aPosition)
    {
        return true;
    }
}

