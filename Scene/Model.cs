using GLWrapper.Graphics.Vertices;
using System;
using System.Collections.Generic;

namespace GLWrapper.Scene
{
    public class Model
    {
        protected bool _isVaoBinded = false;
        protected virtual VertexBuffer VBO { get; set; }
        public virtual VertexArray VAO { get; protected set; }        
        public virtual ShaderProgram Shader { get; protected set; }
        public DrawVBOCommand OnDraw { get; set; }
        public Action<double> OnUpdate { get; set; }
        protected Model()
        {
            Shader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/basicVertex.shader", "./Assets/Shaders/basicFrag.shader");
            Shader.SetVertexAttributes(new VertexAttribute("aPosition", 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                       new VertexAttribute("aColor", 4, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
        }
        protected Model(VertexArray vao,VertexBuffer vbo) : this()
        {
            VAO = vao;
            VBO = vbo;            
        }        
        /// <summary>
        /// Creates a default model with the given vertices. If no shader is provided, a default will be used assuming a position and color
        /// </summary>
        /// <typeparam name="TVertex">The vertex data type</typeparam>
        /// <param name="vertexData">the vertices to load to the associated VBO</param>
        /// <returns>A <see cref="Model"/> instance</returns>
        public static Model CreateModel<TVertex>(TVertex[] vertexData) where TVertex : struct
        {
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            return new Model(vao, vbo);
        }
        public virtual Model CreateModel<TVertex>(TVertex[] vertexData,ShaderProgram shaderProgram) where TVertex : struct
        {
            var model = CreateModel(vertexData);
            model.Shader = shaderProgram;
            return model;
        }
        public virtual Model CreateModel<TVertex>(TVertex[] vertexData,string vertexShader,string fragmentShader) where TVertex : struct
        {
            var model = CreateModel(vertexData, ShaderProgram.CreateShaderProgram(vertexShader, fragmentShader));
            return model;
        }
        /// <summary>
        /// Loads the given vertices to the related Vertex Buffer Object(VBO) on this object. The previous VBO data will be overwritten
        /// </summary>
        /// <typeparam name="TVertex">The vertex data type</typeparam>
        /// <param name="vertexData">the vertices to load to the VertexBuffer representing this model</param>
        public virtual void LoadVertexData<TVertex>(TVertex[] vertexData) where TVertex : struct
        {
            if (!_isVaoBinded)
            {
                VAO.Bind();
            }            
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            VBO = vbo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="vertexData"></param>
        public virtual void UpdateVertexData<TVertex>(TVertex[] vertexData) where TVertex : struct
        {
            VBO.LoadData(vertexData);
        }
        public virtual void SetShader(ShaderProgram shaderProgram,params VertexAttribute[] attributes)
        {
            shaderProgram.SetVertexAttributes(attributes);
        }
        public virtual void Draw(double time)
        {
            VAO.Bind();
            this.Shader.Use();
            this.Shader.SetProjection(Ioc.Camera);
            OnDraw(this.VBO, this.Shader);
        }
        public virtual void Update(double time)
        {            
            OnUpdate(time);
        }
    }
}
