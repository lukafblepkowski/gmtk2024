using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
	PolygonCollider2D poly;
	MeshRenderer mr;

	List<Joint> joints;

    List<Vector3> vertices;
    List<Vector2> uvs;
    List<int> triangles;

	public int polySampleRate = 1;
	public Material material { get {
			return mr.material;
	} }

    void Start()
    {
		vertices = new List<Vector3>();
		uvs = new List<Vector2>();
		triangles = new List<int>();

        mesh = new Mesh();
		mr = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;

		poly = GetComponent<PolygonCollider2D>(); 

		SetJoints(new List<Joint>());

		poly.pathCount = 1;
		poly.SetPath(0,new Vector2[] { });
    }

	public void SetJoints(List<Joint> joints) {
		this.joints = joints;

		if(joints.Count == 0) return;

		vertices.Clear();
		uvs.Clear();
		triangles.Clear();

		UpdateData();
		UpdateMesh();
		UpdatePolygon();
	}

    void UpdateData() {
		//Add vertices
		var length = joints.Count;
		for(int i = 0;i < length;i++) {

			Joint j = joints[i];

			Vector2 left = (Vector2)(Quaternion.Euler(0f,0f,-90f) * j.facing).normalized * j.width;
			Vector2 right = (Vector2)(Quaternion.Euler(0f,0f,90f) * j.facing).normalized * j.width;

			//Doubling up
			if(i != 0 && i != length - 1) {
				vertices.Add((Vector3)(j.pos + left) + (Vector3.forward * j.z));
				vertices.Add((Vector3)(j.pos + right) + (Vector3.forward * j.z));
				uvs.Add(new Vector2(0,j.uvprev));
				uvs.Add(new Vector2(1,j.uvprev));
			}

			vertices.Add((Vector3)(j.pos + left) + (Vector3.forward * j.z));
			vertices.Add((Vector3)(j.pos + right) + (Vector3.forward * j.z));
			uvs.Add(new Vector2(0,j.uv));
			uvs.Add(new Vector2(1,j.uv));
		}

		for(int i = 0;i < length - 1;i++) {
			triangles.Add(4 * i);
			triangles.Add(4 * i+2);
			triangles.Add(4 * i+1);
			triangles.Add(4 * i+1);
			triangles.Add(4 * i+2);
			triangles.Add(4 * i+3);
		}
	}

	void UpdateMesh() {
        mesh.Clear();

		mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
		mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
    }
	void UpdatePolygon() {
		List<Vector2> points = new List<Vector2>();

		//was 2, is now 6 for blightbulb
		int count = vertices.Count;
		for(int i = 2; i < count; i += 4 * polySampleRate) {
			points.Add(vertices[i]);
			points.Insert(0, vertices[i+1]);
		}

		points.Add(vertices[count - 2]);
		points.Insert(0,vertices[count - 1]);

		poly.SetPath(0, points.ToArray());
	}
}
