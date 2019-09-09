using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject GhostPrefab;
    public List<Ghost> Ghosts;
    public float myGhostGhostCounter;

    // Start is called before the first frame update
    void Start()
    {
        Ghosts = new List<Ghost>();
        for(int g = 0; g < 1; g++)
        {
            Ghosts.Add(GameObject.Instantiate(GhostPrefab).GetComponent<Ghost>());
            Ghosts[g].Respawn(MapManager.Get().ghostStartPos);
            Ghosts[g].transform.SetParent(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int g = 0; g < Ghosts.Count; g++)
        {
            Ghosts[g].OnUpdate(MapManager.Get(), GameManager.Get().GetPlayer());
        }
    }
    void HandleEnemyCollisionWithPlayer(Ghost g)
    {
        if(!g.isVulnerable)
        {
            GameManager.Get().PlayerDestroyed();
            g.Respawn(MapManager.Get().ghostStartPos);
        }
        else
        {
            GameManager.Get().GhostDestroyed();
            g.Respawn(MapManager.Get().ghostStartPos);
            g.alive = false;
        }
    }
}
