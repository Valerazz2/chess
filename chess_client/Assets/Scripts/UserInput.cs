using Chess.Model;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField]
    public ChessColor UserColor;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectSquare();
        }
    }
    
    public void SelectSquare()
    {
        var target = GetSquareByMousePos();
        target?.Select(UserColor);
    }
    
    private Square GetSquareByMousePos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit2D = Physics2D.Raycast(mousePos, transform.forward, 1000);
        if (hit2D && hit2D.collider.gameObject.GetComponent<SquareView>())
        {
            return hit2D.transform.gameObject.GetComponent<SquareView>().model;
        }
        return null;
    }
}
