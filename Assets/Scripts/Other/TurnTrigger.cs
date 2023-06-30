using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    [SerializeField] GameObject turnPrefab;
    [SerializeField] int gridSize = 9;

    void Awake()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Instantiate(turnPrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }

            for (int j = -1; j > -gridSize; j--)
            {
                Instantiate(turnPrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }

        for (int i = -1; i > -gridSize; i--)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Instantiate(turnPrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }

            for (int j = -1; j > -gridSize; j--)
            {
                Instantiate(turnPrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }
    }
}