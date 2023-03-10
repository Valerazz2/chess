using Chess.Model;
using Chess.View;
using UnityEngine;

public class SquareView : AbstractView<Square>
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite blackSquare;
    [SerializeField] private Sprite whiteSquare;

    [SerializeField] private GameObject choosedSquare;
    [SerializeField] private GameObject choosedPiece;
    
    private GameObject createdSprite;

    protected override void OnBind()
    {
        model.MoveAble.ValueChanged += ViewMovable;
        model.Marked.ValueChanged += ViewMarked;
        
        var sprite = model.Color == ChessColor.White ? whiteSquare : blackSquare;
        SpriteRenderer.sprite = sprite;
        transform.position = model.GetPosVector3();
    }

    private void ViewMovable(bool value, bool oldValue)
    {
        RemoveCreatedSprite();

        if (value)
        {
            choosedSquare.SetActive(true);
            createdSprite = choosedSquare;
            var dist = Vector2.Distance(transform.position, model.Desk.CurrentPiece.Square.GetPosVector3());
            LeanTween.alpha(createdSprite, 1, dist / 5);
        }
    }

    private void ViewMarked(bool value, bool oldValue)
    {
        RemoveCreatedSprite();
        if (value)
        {
            choosedPiece.SetActive(true);
            createdSprite = choosedPiece;
            if (createdSprite)
            {
                createdSprite.LeanAlpha(0.39f, 0.1f);
            }
        }
    }

    private void RemoveCreatedSprite()
    {
        if (createdSprite)
        {
            createdSprite.LeanAlpha(0f, 0.01f);
            createdSprite.SetActive(false);
        }
    }
    
}
