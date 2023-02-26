using Chess.Model;
using Unity.VisualScripting;
using UnityEngine;

namespace Chess.View
{
    public class PieceView : AbstractView<Piece>
    {
        private Sprite sprite;
        
        protected override void OnBind()
        {
            sprite = Resources.Load<Sprite>("pieces/" + model.GetPieceType() + "_" + model.Color);
            GetComponent<SpriteRenderer>().sprite = sprite;
            
            transform.localPosition = model.SquareHolder.Value.GetPosVector3();

            model.SquareHolder.ValueChanged += OnFigureMoved;
            model.Desk.Pieces.ObjectRemoved += OnFigureRemoved;
        }

        private void OnFigureMoved(Square oldSquare, Square newSquare)
        {
            LeanTween.moveLocal(gameObject, model.Square.GetPosVector3(), 0.2f).setEase(LeanTweenType.easeOutCirc);
        }

        private void OnFigureRemoved(Piece obj)
        {
            if (obj == model)
            {
                Destroy(gameObject);
            }
        }
    }
}
