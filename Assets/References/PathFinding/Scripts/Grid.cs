using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public bool bShowPlane;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public float TraslateX, TraslateY;
	public float NodedistanceX, NodedistanceY;



	Node[,] grid;
	Plane pPlane;

	public List<Node> path;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y);
		CreateGrid();

		// Create a plane
		GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
		p.transform.position = this.transform.position;
		p.transform.localScale = new Vector3( gridWorldSize.x, 0.01f, gridWorldSize.y );

		Material m = new Material(Shader.Find("Standard"));
		m.color = Color.blue;
		p.GetComponent<MeshRenderer>().material = m;
		p.GetComponent<MeshRenderer>().enabled = bShowPlane;
		p.transform.SetParent( transform );

		path = new List<Node>();

	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() {

		grid = new Node[gridSizeX,gridSizeY];

		UpdateGrid();

	}

	public void UpdateGrid() {

		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {

				Vector3 worldPoint = worldBottomLeft +
					// Offset
					( Vector3.forward	* TraslateX * x ) + ( Vector3.right	* TraslateY * y ) +
					// Node distance
					( Vector3.right	* NodedistanceX * x ) + ( Vector3.forward	* NodedistanceY * y ) +
					// Horizzontal
					Vector3.right	* ( x * nodeDiameter + nodeRadius ) + 
					// Vertical
					Vector3.forward * ( y * nodeDiameter + nodeRadius );

				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				if ( grid [x,y]  == null ) {
					grid [x,y] = new Node( walkable, worldPoint, nodeRadius, x, y );
				}
				else grid [x,y].walkable = walkable;

				grid [x,y].worldPosition = worldPoint;
			}

		}

	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {

		float fDistance = float.MaxValue;
		Node pBestNode = null;

		if ( grid != null ) {
			foreach (Node n in grid) {
				float fCurrentDistance = Vector3.Distance( n.worldPosition, worldPosition );
				if ( fCurrentDistance < fDistance ) {
					fDistance = fCurrentDistance;
					pBestNode = n;
				}
			}
		}
		return pBestNode;

		/*


		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
		*/
	}

	
	void OnDrawGizmos() {

//		UpdateGrid();

		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
	
}