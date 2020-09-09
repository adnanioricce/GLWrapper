using System;
using System.Collections.Generic;
using System.Linq;
using GLWrapper.Scene;

namespace GLWrapper.Graphics
{
    public class ParticleSystem
    {
        public List<Particle> Particles {get;protected set;} = new List<Particle>();
        protected Model _model;
        protected ParticleSystem(List<Particle> particles,Model model)
        {
            Particles = particles;
            _model = model;
        }
        public static ParticleSystem CreateParticles(int particlesCount)
        {
            var particles = Enumerable.Repeat<Particle>(Particle.WhiteAndCenter,particlesCount);
            var model = Model.CreateModel(particles.Select(p => p.Vertex).ToArray());
            return new ParticleSystem(particles.ToList(),model);
        }
        //VBO probably will not work here, I need to update with the particle list
        public void SetOnDraw(DrawVBOCommand command)
        {
            _model.OnDraw = command;
        }
        public void SetOnUpdate(Action<double> onUpdate)
        {
            _model.OnUpdate = onUpdate;
        }
        public void Draw(float time)
        {
            _model.Draw(time);
        }
        public void Update(float time)
        {
            _model.Update(time);
        }
    }
}