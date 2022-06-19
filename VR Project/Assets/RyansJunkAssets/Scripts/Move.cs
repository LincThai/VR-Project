using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    [Tooltip("Goal gets overwritten")]
    public GameObject goal;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
    }
}
