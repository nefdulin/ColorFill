﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill
{
	public static class LookupTables
	{
		public static readonly Vector3[] Vertices = new Vector3[8]
		{
			new Vector3(0.0f, 0.0f, 0.0f),
			new Vector3(1.0f, 0.0f, 0.0f),
			new Vector3(1.0f, 1.0f, 0.0f),
			new Vector3(0.0f, 1.0f, 0.0f),
			new Vector3(0.0f, 0.0f, 1.0f),
			new Vector3(1.0f, 0.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(0.0f, 1.0f, 1.0f),
		};

		public static readonly int[,] Triangles = new int[6, 6]
		{
			{0, 3, 1, 1, 3, 2}, // Back Face
			{5, 6, 4, 4, 6, 7}, // Front Face
			{3, 7, 2, 2, 7, 6}, // Top Face
			{1, 5, 0, 0, 5, 4}, // Bottom Face
			{4, 7, 0, 0, 7, 3}, // Left Face
			{1, 2, 5, 5, 2, 6} // Right Face
		};

		public static readonly Vector3[] NeighborCubeChecks = new Vector3[6] {

            new Vector3(0.0f, 0.0f, -1.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, -1.0f, 0.0f),
            new Vector3(-1.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f)

        };


		public static readonly Vector2[] UVs = new Vector2[6]
		{
			new Vector2 (0.0f, 0.0f),
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 0.0f),
			new Vector2 (1.0f, 0.0f),
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 1.0f)
		};
	}

}