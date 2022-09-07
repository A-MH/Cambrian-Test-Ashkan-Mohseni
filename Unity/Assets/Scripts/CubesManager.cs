using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public CameraController cameraController;
    List<GameObject> cubes = new();
    GameObject leftmostCube;
    GameObject rightmostCube;
    GameObject uppermostCube;
    GameObject lowermostCube;

    /// <summary>
    /// take the string data received, parse it into a SimpleJSON.Jason representation and then extract the information from the jason object to instantiate the cubes with extracted transform info
    /// </summary>
    /// <param name="dataReceived"></param>
    public void InstantiateCubes(string dataReceived)
    {
        foreach (var item in cubes)
        {
            Destroy(item);
        }
        cubes.Clear();
        var json = SimpleJSON.JSON.Parse(dataReceived);
        for (int i = 0; i < json[0].Count; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.position = new Vector3(float.Parse(json[0][i][0].Value), float.Parse(json[0][i][1].Value), float.Parse(json[0][i][2].Value));
            cube.transform.eulerAngles = new Vector3(float.Parse(json[0][i][3].Value), float.Parse(json[0][i][4].Value), float.Parse(json[0][i][5].Value));
            cube.transform.localScale = new Vector3(float.Parse(json[0][i][6].Value), float.Parse(json[0][i][7].Value), float.Parse(json[0][i][8].Value));
            cubes.Add(cube);
        }
        Vector3 centrePoint = CalculateCentrePoint();
        GetExtremities();
        cameraController.NewPosition = CalculateNewCameraPosition(centrePoint);
    }

    /// <summary>
    /// calculate the distance between uppermost and lowemost cubes, if this is more than 1.3 times horizontal gap between leftmost and right most cube. then use vertical gap to adjust camera distance from cubes, else use horizontal gap. the reason for the 1.6 constant is because normally horizontal FOV is 1.6 times more than the vertical FOV
    /// </summary>
    /// <param name="centrePoint"></param>
    private Vector3 CalculateNewCameraPosition(Vector3 centrePoint)
    {
        Vector3 newCameraPosition;
        float horizontalGap = rightmostCube.transform.position.x - leftmostCube.transform.position.x;
        float verticalGap = uppermostCube.transform.position.y - lowermostCube.transform.position.y;
        if (horizontalGap > 1.6f * verticalGap)
            newCameraPosition = centrePoint + new Vector3(0, 0, -1f) * horizontalGap + new Vector3(0,0,-5);
        else
            newCameraPosition = centrePoint + new Vector3(0, 0, -1.6f) * verticalGap + new Vector3(0, 0, -5);
        return newCameraPosition;
    }

    /// <summary>
    /// To know what the target position for camera should be, we need to get the leftmost, rightmost, uppermost and lowermost cubes
    /// </summary>
    private void GetExtremities()
    {
        leftmostCube = rightmostCube = uppermostCube = lowermostCube = null;
        foreach (var cube in cubes)
        {
            var cubePosition = cube.transform.position;
            if (leftmostCube == null || cubePosition.x < leftmostCube.transform.position.x)
                leftmostCube = cube;
            if (rightmostCube == null || cubePosition.x > rightmostCube.transform.position.x)
                rightmostCube = cube;
            if (uppermostCube == null || cubePosition.y > uppermostCube.transform.position.y)
                uppermostCube = cube;
            if (lowermostCube == null || cubePosition.y < lowermostCube.transform.position.y)
                lowermostCube = cube;
        }
    }

    /// <summary>
    /// Calculate centre point of all the cubes
    /// </summary>
    private Vector3 CalculateCentrePoint()
    {
        float totalX = 0;
        float totalY = 0;
        float totalZ= 0;
        foreach (var cube in cubes)
        {
            totalX += cube.transform.position.x;
            totalY += cube.transform.position.y;
            totalZ += cube.transform.position.z;
        }
        return new Vector3(totalX, totalY, totalZ) / cubes.Count;
    }
}
