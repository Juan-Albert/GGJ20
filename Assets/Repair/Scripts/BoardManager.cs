using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;

    //Obtenemos todos los componentes que pueden haber en el mapa
    public GameObject[] floorTiles, looseTiles, brokenTiles, wallTiles, minionsTiles;

    private Transform board;

    private List<Vector2> creablesPositions = new List<Vector2>();

    //Metodo público que inicializa el mapa
    public void SetupScene(int level)
    {
        initializeList();
        BoardSetup();
        LayoutObjectAtRandom(brokenTiles, 1, 3);

        int looseToSpawn = 4; //(int)Mathf.Log(level, 2); //Aparecen tantos enemigos como logaritmo en base 2 de level
        LayoutObjectAtRandom(looseTiles, looseToSpawn, looseToSpawn);

        int minionsToSpawn = 3;
        LayoutObjectAtRandom(minionsTiles, minionsToSpawn, minionsToSpawn);

    }

    //Contruye un mapa de tamaño: columnas+1 x filas+1
    public void BoardSetup()
    {
        board = new GameObject("Board").transform;

        for(int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstanciate = GetRandomInArray(floorTiles);

                if (x == -1 || y == -1 || x == columns || y == rows)
                {
                    toInstanciate = GetRandomInArray(wallTiles);
                }


                GameObject floor = Instantiate(toInstanciate, new Vector2(x, y), Quaternion.identity) as GameObject;
                floor.transform.SetParent(board);
                
            }
        }
    }

    //Obtiene un elemento aleatorio de un array
    GameObject GetRandomInArray(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    //Inicializa la zona donde pueden aparecer objetos
    void initializeList()
    {
        creablesPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                creablesPositions.Add(new Vector2(x, y));
            }
        }
    }

    //Devuelve un objeto una posición dentro de la lista y lo elimina para que no se repita
    Vector2 RandomPosition()
    {
        int randomIndex = Random.Range(0, creablesPositions.Count);
        Vector2 randomPosition = creablesPositions[randomIndex];
        creablesPositions.RemoveAt(randomIndex);
        return randomPosition;
    }


    //Genera un tipo de objeto entre min y max veces en la zona de objetos creables
    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector2 randomPosition = RandomPosition();
            GameObject tileChoice = GetRandomInArray(tileArray);
            GameObject tileObject = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            tileObject.transform.SetParent(board);
        }
    }
}