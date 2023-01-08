using System.Collections.Generic;
using UnityEngine;

namespace Chess.View
{
    public abstract class AbstractView<TM> : MonoBehaviour
    {
        public TM model { get; private set; }

        protected void Bind(TM _model)
        {
            model = _model;
            OnBind();
        }

        protected abstract void OnBind();
        
        protected void CreateViews<M, V>(IEnumerable<M> modelList, V viewPrefab, Transform parent) where V : AbstractView<M>
        {
            foreach (var model in modelList)
            {
                CreateView(model, viewPrefab, parent);
            }
        }
        
        protected void CreateView<M, V>(M model, V viewPrefab, Transform parent) where V : AbstractView<M>
        {
            var view = Instantiate(viewPrefab, parent);
            view.Bind(model);
        }

        protected virtual void UnBind()
        {
            model = default(TM);
        }
        
    }
}
