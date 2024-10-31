using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class PointShoot : MonoBehaviour
{
    private bool change_pos = false;
    private Vector3 _startPos = new Vector3(-0.45f, 0.05f, 0f);
    private Vector3 _currentPos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentPos = _startPos;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _currentPos = new Vector3(0.5f, 0.05f, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _currentPos = _startPos;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _currentPos = new Vector3(0.5f, 0.05f, 0f);
        }
    }
}
