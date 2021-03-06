﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject Splash;
    public GameObject[] gems;
    public GameObject gemPrefab;
    public GameObject[,] Allgems;
    public GameObject background;
    private GameObject undergem;
    public int rows;
    public int columns;

    // Start is called before the first frame update
    void Start()
    {

        Allgems = new GameObject[columns, rows];
        Setboard();
    }

    public void Setboard() //set the board with random gems(5x7)
    {
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Vector2 tempPosition = new Vector2(c, r);
                GameObject backgroundTile = Instantiate(gemPrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + c + "," + r + ")";

                int gemToUse = Random.Range(0, gems.Length);
                GameObject gem = Instantiate(gems[gemToUse], tempPosition, Quaternion.identity);
                gem.transform.parent = this.transform;
                gem.name = "(" + c + "," + r + ")";
                Allgems[c, r] = gem;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void MatchatPosition(int columns, int rows)
    {
        if (Allgems[columns, rows].GetComponent<Gem>().Matched)
        {
            Destroy(Allgems[columns, rows]);
            Allgems[columns, rows] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if (Allgems[c, r] != null)
                {
                    MatchatPosition(c, r);
                }
            }
        }
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        int nullCount = 0;
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if (Allgems[c, r] == null)
                { nullCount++; }
                else if (nullCount > 0)
                {
                    Allgems[c, r].GetComponent<Gem>().rows -= nullCount;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Refill());
    }

    public void NewGem()
    {
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if (Allgems[c, r] == null)
                {
                    Vector2 tempPosition = new Vector2(c, r);
                    int Newgem = Random.Range(0, gems.Length);
                    GameObject newGem = Instantiate(gems[Newgem], tempPosition, Quaternion.identity);
                    Allgems[c, r] = newGem;
                }
            }
        }
    }

    public bool isMatches()
    {

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if(Allgems[c,r] != null)
                {
                    if(Allgems[c,r].GetComponent<Gem>().Matched)
                    { return true;  }
                }
            }
        }
                return false;
    }

    private IEnumerator Refill()
    {
        NewGem();
        yield return new WaitForSeconds(0.5f);

        while(isMatches())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
    }
}
    