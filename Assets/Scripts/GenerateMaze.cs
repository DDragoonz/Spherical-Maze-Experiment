using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour {

    [Tooltip("Radius of sphere")]
    public float radius;
    // Longitude |||
    [Tooltip("how many wall generated verticaly? recomended value = radius * 6")]
    public int Longitude;
    // Latitude ---
    [Tooltip("how many wall generated horizontaly? recomended value = radius * 3")]
    public int Latitude;
    [Tooltip("how many horizontal wall on pole deleted? useful to make pole spacious")]
    public int LatitudeCut;
    [Tooltip("how many shortcut is generated? will randomly delete wall")]
    public int Shortcut;
    public GameObject WallPrefab;
    [Tooltip("set true to force wall to expand width")]
    public bool ScaleWidth;

    private MazeWall[,] nodes;

    private int sizeX, sizeY;
    
    

    // Use this for initialization
    void Start() 
    {
        Longitude = (int)(Longitude / 2f) * 2; // longitude must be even
        Latitude = Latitude % 2 == 1 ? Latitude : Latitude + 1;// Latitude must be odd

        sizeX = Longitude;
        sizeY = Latitude - LatitudeCut * 2;
        

        nodes = new MazeWall[sizeX, sizeY];


        generateWall(); // first step is generating full wall on spherical surface
        

        if (ScaleWidth) rescaleWall(); // then rescaling wall if needed


        nodes[0, 0].SetAsPassage(); // set first nodes as passage
        
        visit(0, 0); // begin recursive backtracking algorithm

        generatePassage(); // begin creating passage by disabling wall marked as passage

        

        generateShortcut(); // begin disabling some wall randomly
        
    }

    private void generateWall()
    {
        #region 
        //generating node on sphere surface source : http://wiki.unity3d.com/index.php/ProceduralPrimitives#C.23_-_Sphere

        float _pi = Mathf.PI;
        float _2pi = _pi * 2f;
        for (int lat = LatitudeCut, y = 0; lat < Latitude - LatitudeCut; lat++, y++)
        {
            float a1 = _pi * (float)(lat + 1) / (Latitude + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0, x = 0; lon < Longitude; lon++, x++)
            {
                float a2 = _2pi * (float)(lon == Longitude ? 0 : lon) / Longitude;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                
                Vector3 pos = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;

                #endregion

                GameObject wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                
                nodes[x, y] = wall.GetComponent<MazeWall>();
                nodes[x, y].Initialize();
                
                

            }
            
        }
    }

   

    private void rescaleWall() // rescale wall width by measuring width between wall, only work if wall width is default to 1 unit
    {
        for (int y = 0; y < sizeY; y++)
        {
            float distanceX = Vector3.Distance(nodes[0, y].transform.position, nodes[1, y].transform.position);
            for (int x = 0; x < sizeX; x++)
            {
                Vector3 newScale = nodes[x, y].transform.localScale;
                newScale.x = distanceX;
                nodes[x, y].transform.localScale = newScale;
            }
        }
    }

    private void generatePassage()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (nodes[x, y].isPassage()) nodes[x, y].Disable();
            }
            
        }
    }

    private void generateShortcut()
    {
        for (int i = 0; i < Shortcut;)
        {
            int randX = Random.Range(1, sizeX-1);
            int randY = Random.Range(1, sizeY-1);
            if (!nodes[randX, randY].isPassage() )
            {
                if(
                    (nodes[randX+1, randY].isPassage() && nodes[randX - 1, randY].isPassage())||
                    (nodes[randX, randY+1].isPassage() && nodes[randX, randY-1].isPassage())
                    )
                {
                    nodes[randX, randY].Disable();
                    i++;
                }
                
            }
        }
    }


    private void visit(int x, int y) // recursive method
    {
        
        List<int> indexQueue = new List<int>(4);

        while (indexQueue.Count < indexQueue.Capacity) //generate random number as next traversing direction
        {

            int n = Random.Range(0, 4);
            if (!indexQueue.Contains(n))
                indexQueue.Add(n);
        }

        
        foreach (int i in indexQueue)
        {
            int newX = 0, newY = 0; // interpret i to direction, by providing new x and y coordinate
            switch (i) {
                case 0:
                    newX = x;
                    newY = y + 2;
                    if (newY > (sizeY)) continue; 
                    break;
                case 1:
                    newX = x;
                    newY = y - 2;
                    if (newY < 0)continue;
                    break;
                case 2:
                    newX = x + 2;
                    newY = y;
                    if (newX > (sizeX - 2)) newX = 0;
                    break;
                case 3:
                    newX = x - 2;
                    newY = y;
                    if (newX < 0)newX = (sizeX - 2);
                    break;
            }

            
            if (nodes[newX, newY].isPassage())
            {
                continue;
            }
            else
            {
                nodes[newX, newY].SetAsPassage();
                if (i == 0)nodes[newX, newY - 1].SetAsPassage();
                else if (i == 1)nodes[newX, newY + 1].SetAsPassage();
                else if (i == 2)
                {
                    int newestX = newX - 1 < 0 ? sizeX - 1 : newX - 1;
                    nodes[newestX, newY].SetAsPassage();
                }
                else if (i == 3) nodes[newX + 1, newY].SetAsPassage();
                
                visit(newX, newY); // recursively visit next neighbour
            }
        }

    }
}
