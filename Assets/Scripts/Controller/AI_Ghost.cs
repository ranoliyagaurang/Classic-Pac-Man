using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AI_Ghost : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Vector3 destination;
    [SerializeField] float radius;

    private void OnEnable()
    {
        GameManager.StateChanged += GameManager_StateChanged;
    }

    private void GameManager_StateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Running:
                agent.enabled = true;
                FindDestination();
                break;

            case GameState.GameOver:
                agent.enabled = false;
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= GameManager_StateChanged;
    }

    private void Update()
    {
        if (!agent.enabled)
            return;

        if (Vector3.Distance(transform.position, destination) < 0.25f)
        {
            FindDestination();
        }
    }

    void FindDestination()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        colliders = colliders.ToList().FindAll(x => x.CompareTag("Turn")).ToArray();

        Dictionary<Collider, float> keyValues = new();
        for (int i = 0; i < colliders.Length; i++)
        {
            keyValues.Add(colliders[i], Vector3.Distance(transform.position, colliders[i].transform.position));
        }

        var items = from pair in keyValues
                    orderby pair.Value descending
                    select pair;

        for(int i = 0; i < items.Count(); i++)
        {
            destination = items.ElementAt(Random.Range(0, items.Count() - 1)).Key.transform.position;

            NavMeshPath navMeshPath = new();

            if (agent.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                destination.y = 0.5f;
                break;
            }
            else
            {
                Debug.LogWarning("Not Reachable");
            }
        }

        agent.SetDestination(destination);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("AI_Ghost"))
        {
            FindDestination();
        }
    }
}