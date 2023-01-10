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
        model.MoveAble.StateChanged += ViewMovable;
        model.Marked.StateChanged += ViewMarked;
        
        var sprite = model.Color == ChessColor.White ? whiteSquare : blackSquare;
        SpriteRenderer.sprite = sprite;
        transform.position = model.GetPosVector3();
    }

    private void ViewMovable()
    {
        RemoveCreatedSprite();

        if (model.MoveAble.Value)
        {
            choosedSquare.SetActive(true);
            createdSprite = choosedSquare;
            var dist = Vector2.Distance(transform.position, model.Desk.CurrentPiece.Square.GetPosVector3());
            LeanTween.alpha(createdSprite, 1, dist / 5);
        }
    }

    private void ViewMarked()
    {
        RemoveCreatedSprite();
        if (model.Marked.Value)
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
