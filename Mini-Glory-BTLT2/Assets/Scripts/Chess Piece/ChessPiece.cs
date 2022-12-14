using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    Pawn = 0,
    Rook = 1,
    Knight = 2,
    Bishop = 3
}

public enum ChessPieceTeam
{
    White = 0,
    Black = 1
}

public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;
    public int stunned;

    private Vector3 desiredPosition;
    private Vector3 desiredScale = Vector3.one;

    private void Start() 
    {
        desiredPosition = new Vector3((float)-currentX, 0.3f, (float)currentY);
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        //transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        return r;
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if (force)
        {
            this.transform.position = desiredPosition;
            Debug.Log(transform.position);
        }
    }
    // public virtual void SetScale(Vector3 scale, bool force = false)
    // {
    //     desiredScale = scale;
    //     if (force)
    //         transform.scale = desiredScale;
    // }
}
