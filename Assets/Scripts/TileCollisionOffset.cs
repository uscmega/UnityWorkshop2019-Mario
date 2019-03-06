using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCollisionOffset : MonoBehaviour
{
    public float yOffset = 0.25f;
    public float duration = 0.5f;
    private Tilemap m_Tilemap;
    private 

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var cellPosition = m_Tilemap.WorldToCell(other.rigidbody.position);
            m_Tilemap.GetTile(cellPosition);
        }
    }

    // private IEnumerator offsetTileCoroutine(ITilemap tilemap, Vector3Int tilePos, TileBase tile)
    // {
    //     float startTime = Time.time;
    //     TileData tileData;
    //     // while ()
    //     // {
    //     //     // use the below statement to wait for a frame
    //     //     yield return null;
    //     // }
    // }
}
