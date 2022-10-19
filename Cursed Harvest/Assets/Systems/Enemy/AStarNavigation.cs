using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class AStarNavigation : MonoBehaviour
{
    public Vector2 worldSize;
    public float nodeSize;
    [Range(0.0f, 10.0f)] public float gridQuality = 1.0f;
    public LayerMask nonWalkable;
    List<Node> allNodes;
    List<Node> calculatedPath;

    private void Awake()
    {
        GenerateNodes();
    }

    public List<Node> FindPath(Vector2 currentPos, Vector2 targetPos)
    {
        List<Node> path = new List<Node>();

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        Node targetNode = GetNode(targetPos);
        Node startNode = GetNode(currentPos);

        open.Add(startNode);

        int count = 0;

        while (open.Count > 0)
        {
            Node currentNode = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].fCost < currentNode.fCost || open[i].fCost == currentNode.fCost && open[i].hCost < currentNode.hCost)
                {
                    currentNode = open[i];
                }
            }
            closed.Add(currentNode);
            open.Remove(currentNode);

            if (currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }

            List<Node> neighbours = GetNeighbours(currentNode);
            foreach (Node neighbour in neighbours)
            {
                if (closed.Contains(neighbour)) { continue; }

                float movementCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (movementCost < neighbour.gCost || !open.Contains(neighbour))
                {
                    neighbour.gCost = movementCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }
            }
            count++;

            if (count > 100000) { break; }
        }
        path = RetracePath(startNode, targetNode);
        return path;
    }

    public List<Node> RetracePath(Node start, Node target)
    {
        List<Node> path = new List<Node>();

        Node node = target;
        while (node != start)
        {
            path.Add(node);
            node = node.parent;
        }
        path.Reverse();
        calculatedPath = path;
        return path;
    }

    public float GetDistance(Node nodeA, Node nodeB)
    {
        float distanceX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        float distanceY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }

    public Node GenerateRandomTarget()
    {
        Node node = allNodes[Random.Range(0, allNodes.Count)];
        return node;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> results = new List<Node>();

        for(float x = -1/gridQuality; x <= 1/gridQuality; x += (1/gridQuality))
        {
            for (float y = -1/gridQuality; y <= 1/gridQuality; y += (1/gridQuality))
            {
                if(x == 0 && y == 0) { continue; }

                Vector2 pos = new Vector2(node.position.x + x, node.position.y + y);
                Node n = GetNode(pos);
                if(n != null)
                {
                    results.Add(n);
                }
            }
        }

        return results;
    }

    public Node GetNode(Vector2 position)
    {
        Node best = allNodes[0];
        float distance = Vector2.Distance(best.position, position);
        foreach (Node n in allNodes)
        {
            float temp = Vector2.Distance(n.position, position);
            if (temp < distance)
            {
                distance = temp;
                best = n;
            }    
        }
        return best;
    }

    void GenerateNodes()
    {
        List<Node> results = new List<Node>();
        for (float y = -Mathf.RoundToInt(worldSize.y / 2f); y <= Mathf.RoundToInt(worldSize.y / 2); y += (1/gridQuality))
        {
            for (float x = -Mathf.RoundToInt(worldSize.x / 2f); x <= Mathf.RoundToInt(worldSize.x / 2); x += (1/gridQuality))
            {
                Vector2 pos = new Vector2(x, y);
                Collider2D[] col = Physics2D.OverlapCircleAll(pos, nodeSize, nonWalkable);
                if (col.Length == 0){
                    Node node = new Node(pos);
                    results.Add(node);
                }
            }
        }
        allNodes = results;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.zero, worldSize);
        if (allNodes != null)
        {
            foreach (Node n in allNodes)
            {
                Gizmos.color = Color.magenta;
                if (calculatedPath != null)
                {
                    foreach (Node d in calculatedPath)
                    {
                        if (d == n)
                        {
                            Gizmos.color = Color.yellow;
                            break;
                        }
                    }
                }
                    Gizmos.DrawWireSphere(n.position, nodeSize);
            }
        }
    }
}

[System.Serializable]
public class Node{
    public Node parent;
    public Vector2 position;
    public float fCost{
        get { return gCost + hCost; }
    }
    public float gCost;
    public float hCost;

    public Node(Vector2 position)
    {
        this.position = position;
    }
}
