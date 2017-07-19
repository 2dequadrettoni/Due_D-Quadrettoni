using UnityEngine;
using System.Collections.Generic;


public class Pathfinding : MonoBehaviour {
	
	private	Grid		grid;
	private	List<Node>	pCurrentPath;

	void Awake() {
		grid = GetComponent<Grid>();
		pCurrentPath = new List<Node>();

	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		pCurrentPath = grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}


	void CalculatePath( Vector3 startPos, Vector3 targetPos ) {

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				pCurrentPath.Clear();
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					else {
						//openSet.UpdateItem(neighbour);
					}
				}
			}
		}

	}

	public bool FindPath( Vector3 vStartPosition, Vector3 vEndPosition, out List<Node> pNodeList ) {

		pNodeList = null;

		CalculatePath( vStartPosition, vEndPosition );

		pNodeList = new List<Node>( pCurrentPath );

		return ( pCurrentPath.Count > 0 );
	}

	public void UpdateGrid() {

		grid.UpdateGrid();

	}







}
