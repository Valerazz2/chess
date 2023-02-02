using Chess.Model;
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
            
            transform.localPosition = model.Square.GetPosVector3();

            model.Desk.OnMove += OnFigureMoved;
            model.Desk.Pieces.ObjectRemoved += OnFigureRemoved;
        }
        
        private void OnFigureRemoved(Piece obj)
        {
            if (obj == model)
            {
                Destroy(gameObject);
            }
        }

        private void OnFigureMoved(MoveInfo moveInfo)
        {
            if (moveInfo.Piece == model)
            {
                LeanTween.moveLocal(gameObject, model.Square.GetPosVector3(), 0.2f).setEase(LeanTweenType.easeOutCirc);
            }
        }
    }
}
