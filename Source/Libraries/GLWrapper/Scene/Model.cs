using GLWrapper.Factories;
using GLWrapper.Graphics.Vertices;
using System;
using System.Collections.Generic;

namespace GLWrapper.Scene
{
    public class Model
    {
        public virtual VertexBuffer VBO { get; protected set; }
        public virtual ElementBuffer EBO { get; protected set; }
        public virtual VertexArray VAO { get; protected set; }
        public virtual ShaderProgram ShaderProgram { get; protected set; }
        // public virtual List<Texture> Textures { get; protected set; } = new List<Texture>();
        public DrawVBOCommand DrawCommand { get; set; }
        public Action<double> UpdateCommand { get; set; }
        protected Model()
        {            
            
        }
        protected Model(VertexArray vao,VertexBuffer vbo,ShaderProgram shader)
        {
            VAO = vao;
            VBO = vbo;
            ShaderProgram = shader;
        }
        protected Model(VertexArray vao,VertexBuffer vbo,ElementBuffer ebo,ShaderProgram shader)
        {
            VAO = vao;
            VBO = vbo;
            EBO = ebo;
            ShaderProgram = shader;
        }
        /// <summary>
        /// Creates a default model with the given vertices. If no shader is provided, a default will be used assuming a position and color
        /// </summary>
        /// <typeparam name="TVertex">The vertex data type</typeparam>
        /// <param name="vertexData">the vertices to load to the associated VBO</param>
        /// <returns>A <see cref="Model"/> instance</returns>
        public static Model CreateModel<TVertex>(TVertex[] vertexData) where TVertex : struct
        {
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            var shaderProgram = ShaderProgramFactory.CreateDefault2DShaderProgram();
            return new Model(vao, vbo,shaderProgram);
        }
        public static Model CreateModel<TVertex>(TVertex[] vertexData,int[] indices) where TVertex : struct
        {
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            var shaderProgram = ShaderProgramFactory.CreateDefault2DShaderProgram();
            var ebo = ElementBuffer.CreateElementBuffer();
            ebo.Bind();
            ebo.LoadData(indices);
            return new Model(vao,vbo,ebo,shaderProgram);
        }
        public static Model CreateModel<TVertex>(TVertex[] vertexData,ShaderProgram shaderProgram) where TVertex : struct
        {
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            shaderProgram.Use();
            shaderProgram.SetVertexAttributes();
            vbo.Bind();
            return new Model(vao,vbo,shaderProgram);
        }
        
        public static Model CreateModel<TVertex>(TVertex[] vertexData,string vertexShader,string fragmentShader,IEnumerable<VertexAttribute> attributes) where TVertex : struct
        {
            var model = CreateModel(vertexData, ShaderProgram.CreateShaderProgram(vertexShader, fragmentShader,attributes));
            return model;
        }
        public static Model CreateModel<TVertex>(Texture texture,TVertex[] vertexData,int[] indices) where TVertex : struct
        {
            var shader = ShaderProgramFactory.CreateDefault2DShaderProgramWithTexture();
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            shader.Use();
            shader.SetVertexAttributes();
            vbo.Bind();
            texture.Bind();
            var ebo = ElementBuffer.CreateElementBuffer(indices);
            var model = new Model(vao, vbo, ebo, shader);
            return model;
        }
        public static Model CreateModel<TVertex>(TVertex[] vertexData,Texture[] textures,int[] indices,ShaderProgram shader) where TVertex : struct
        {
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertexData);
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            shader.Use();
            shader.SetVertexAttributes();
            vbo.Bind();
            var ebo = ElementBuffer.CreateElementBuffer(indices);
            var model = new Model(vao,vbo,ebo,shader);
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i].Bind(i);                
            }            
            return model;
        }
        /// <summary>
        /// Loads the given vertices to the related Vertex Buffer Object(VBO) on this object. The previous VBO data will be overwritten
        /// </summary>
        /// <typeparam name="TVertex">The vertex data type</typeparam>
        /// <param name="vertexData">the vertices to load to the VertexBuffer representing this model</param>
        public virtual void LoadVertexData<TVertex>(TVertex[] vertexData) where TVertex : struct
        {
            if (!VAO.IsBinded)
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
        public virtual void SetShaderProgram(ShaderProgram shaderProgram)
        {
            ShaderProgram = shaderProgram;
        }
        public virtual void Draw(double time)
        {
            if (!VAO.IsBinded)
            {
                VAO.Bind();
            }            
            this.ShaderProgram.Use();            
            // this.ShaderProgram.SetProjection(Ioc.Camera);
            
            DrawCommand(this.VBO, this.ShaderProgram);
        }
        public virtual void Update(double time)
        {            
            UpdateCommand(time);
        }
    }
}
