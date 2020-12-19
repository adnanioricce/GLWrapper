
using GLWrapper.Graphics;
using System.Collections.Generic;

namespace GLWrapper.Scene
{
    public static class Canvas
    {
        private static readonly List<Model> _models = new List<Model>();
        public static void AddModel(Model model)
        {
            _models.Add(model);
        }
        public static void Render(float time)
        {
            foreach (var model in _models)
            {
                Renderer.Draw(model, time);
            }
        }
    }
}
