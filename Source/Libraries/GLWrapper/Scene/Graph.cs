using GLWrapper.Graphics.Vertices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace GLWrapper.Scene
{
    public class Graph<TVertex> where TVertex : struct
    {
        public List<TVertex> ModelVertices { get; protected set; } = new List<TVertex>();
        public List<Vector3> Positions { get; protected set; } = new List<Vector3>();
        public VertexBuffer ModelVBO { get; protected set; }
        public VertexArray ModelVAO { get; protected set; }
        public ShaderProgram Shader { get; protected set; }
        protected Graph(TVertex[] vertices,Vector3[] positions)
        {
            ModelVertices.AddRange(vertices);
            Positions.AddRange(positions);
        }
        public static Graph<TVertex> CreateGraph(TVertex[] vertices,Vector3[] positions,ShaderProgram shader)
        {
            var graph = new Graph<TVertex>(vertices, positions);
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.Bind();
            vbo.LoadData(vertices);
            var vao = VertexArray.CreateVertexArray();            
            vao.Bind();            
            shader.SetVertexAttributes();
            vbo.Bind();
            graph.ModelVAO = vao;
            graph.ModelVBO = vbo;
            graph.Shader = shader;
            return graph;
        }
        public void Bind()
        {
            ModelVAO.Bind();            
            Shader.Use();
        }
    }
}
