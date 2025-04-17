using System.Collections.Generic;
using UnityEngine;

public class DragonManager : MonoBehaviour
{
    [Header("Movement")]
    public List<Transform> waypoints;
    public float moveSpeed = 3f;
    public float spacing = 0.5f;

    [Header("Body Parts")]
    public List<Transform> bodyParts = new List<Transform>(); // Head first

    private int currentWaypoint = 0;
    private List<Vector3> positionHistory = new List<Vector3>();

    private void Start()
    {
        positionHistory.Clear();
        positionHistory.Add(bodyParts[0].position); // Start with head position
    }

    private void Update()
    {
        if (bodyParts.Count == 0) return;

        MoveHead();
        MoveBodyParts();
    }

    private void MoveHead()
    {
        Transform head = bodyParts[0];
        if (currentWaypoint >= waypoints.Count) return;

        Vector3 target = waypoints[currentWaypoint].position;
        head.position = Vector3.MoveTowards(head.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(head.position, target) < 0.05f)
        {
            currentWaypoint++;
        }

        if (positionHistory.Count == 0 || Vector3.Distance(positionHistory[positionHistory.Count - 1], head.position) > 0.1f)
        {
            positionHistory.Add(head.position);
        }
    }

    private void MoveBodyParts()
    {
        for (int i = 1; i < bodyParts.Count; i++)
        {
            int index = Mathf.Clamp(positionHistory.Count - 1 - i * Mathf.RoundToInt(spacing * 10), 0, positionHistory.Count - 1);
            bodyParts[i].position = Vector3.Lerp(bodyParts[i].position, positionHistory[index], Time.deltaTime * moveSpeed);
        }
    }

    public void RemovePart(Transform part)
    {
        int index = bodyParts.IndexOf(part);
        if (index == -1) return;

        bodyParts.RemoveAt(index);
        Destroy(part.gameObject);

        if (index - 1 >= 0 && index < bodyParts.Count)
        {
            Transform front = bodyParts[index - 1];
            Transform back = bodyParts[index];

            front.position = Vector3.Lerp(front.position, back.position, 1f);
        }
    }
}
