using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    float m_default_posX;
    float m_default_posY;
    float m_default_posZ;

    public GameObject Bishop_White;
    public GameObject Bishop_Black;
    public GameObject Knight_White;
    public GameObject Knight_Black;
    public GameObject Pawn_White;
    public GameObject Pawn_Black;
    public GameObject Rook_White;
    public GameObject Rook_Black;
    public int size_row;
    public int size_col;

    ChessPiece[,] posChess;
    ChessPiece currentDragging;
    private RaycastHit version;
    float rayLength;
    Camera currentCamera;
    ChessBoard m_chessboard;

    // Start is called before the first frame update
    void Start()
    {
        m_default_posX = 0;
        m_default_posY = 0.3f;
        m_default_posZ = 0;
        rayLength = 100;
        posChess = new ChessPiece[8,8];
        m_chessboard = FindObjectOfType<ChessBoard>();

        m_chessboard.DisplayChessBoard(size_row, size_col);
        DisplayChessDefault();
    }

    // Update is called once per frame
    void Update()
    {
       if (!currentCamera)
        {
            currentCamera = Camera.current;
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out version, rayLength))
        {
            if (version.collider.tag == "Cell" || version.collider.tag == "Bishop_White" || version.collider.tag == "Bishop_Black" || version.collider.tag == "Knight_White" || version.collider.tag == "Knight_Black" || version.collider.tag == "Pawn_White" || version.collider.tag == "Pawn_Black" || version.collider.tag == "Rook_White" || version.collider.tag == "Rook_Black")
            {
                Vector2Int index = LookupChessPos(version.transform.gameObject);
                
                if (version.collider.tag != "Cell")
                {
                    ChessPiece tmp = version.transform.gameObject.GetComponent<ChessPiece>();
                    List<Vector2Int> availableMove = tmp.GetAvailableMoves(ref posChess, size_col, size_row);
                    m_chessboard.DrawRoad(availableMove);
                }
                else
                {
                    m_chessboard.ResetRoadColor(); 
                }


                //Move chess
                if (Input.GetMouseButtonDown(0))
                {
                    if (posChess[index.x, index.y] != null)
                    {
                        //Is it our turn?
                        if (true)
                        {
                            currentDragging = posChess[index.x, index.y];
                        }
                    }
                }

                if (currentDragging != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int previousPos = new Vector2Int(currentDragging.currentX, currentDragging.currentY);
                    index = LookupChessPos(version.transform.gameObject);
                    bool validMove = MoveTo(currentDragging, index.x, index.y);
                    if (!validMove)
                    {
                        currentDragging = null;
                    }
                }
            }
            // else if (version.collider.tag == "Cell")
            // {
            //     Vector2Int index = LookupChessPos(version.transform.gameObject, 0);
            // }
            else
            {
                m_chessboard.ResetRoadColor();
            }
        }
    }

    GameObject DisplayChess(GameObject chess, float cell_x, float cell_z)
    {
        Vector3 spawnPos = new Vector3(m_default_posX - cell_x, m_default_posY, m_default_posZ + cell_z);
        //Update posChess
        posChess[(int)cell_x, (int)cell_z] = chess.GetComponent<ChessPiece>();
        // posChess[(int)cell_x, (int)cell_z].SetPosition(spawnPos, true);
        posChess[(int)cell_x, (int)cell_z].currentX = (int)cell_x;
        posChess[(int)cell_x, (int)cell_z].currentY = (int)cell_z;

        GameObject clone = Instantiate(chess, spawnPos, Quaternion.Euler(-90f, 0f, -90f)) as GameObject;
        clone.transform.parent = transform;
        return clone;
    }

    void DisplayChessDefault()
    {
        //Spawn Rook
        DisplayChess(Rook_White, 0, 0);
        DisplayChess(Rook_White, 7, 0);
        DisplayChess(Rook_Black, 0, 7);
        DisplayChess(Rook_Black, 7, 7);
        //Spawn Knight
        DisplayChess(Knight_White, 1, 0);
        DisplayChess(Knight_White, 6, 0);
        DisplayChess(Knight_Black, 1, 7);
        DisplayChess(Knight_Black, 6, 7);
        //Spawn Bishop
        DisplayChess(Bishop_White, 2, 0);
        DisplayChess(Bishop_White, 5, 0);
        DisplayChess(Bishop_Black, 2, 7);
        DisplayChess(Bishop_Black, 5, 7);
        //Spawn Pawn
        for (int i = 0; i < size_col; i++)
        {
            DisplayChess(Pawn_White, i, 1);
            DisplayChess(Pawn_Black, i, 6);
        }
    }

    Vector2Int LookupChessPos(GameObject hitVer)
    {
        // if (type == 1)
        // {
        //     ChessPiece temp = hitVer.GetComponent<ChessPiece>();
        //     return new Vector2Int(temp.currentX, temp.currentY);
        // }
        // else
        // {
        //     Cell temp = hitVer.GetComponent<Cell>();
        //     return vector2Int((int)temp.transfor)
        // }

        return new Vector2Int((int)-hitVer.transform.position.x, (int)hitVer.transform.position.z);

    }

    bool MoveTo(ChessPiece cp, int desX, int desY)
    {
        Vector2Int previousPos = new Vector2Int(cp.currentX, cp.currentY);

        //Di chuy???n
        cp.currentX = desX;
        cp.currentY = desY;
        cp.SetPosition(new Vector3(m_default_posX - desX, m_default_posY, m_default_posZ + desY), false);

        posChess[desX, desY] = cp;
        posChess[previousPos.x, previousPos.y] = null;

        return true;
    }
}
