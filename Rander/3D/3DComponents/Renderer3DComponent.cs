using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Rander._3D
{
    class Renderer3DComponent : Component3D
    {
        BasicEffect Material;
        List<VertexPositionColor> Verts = new List<VertexPositionColor>();
        VertexBuffer Buffer;
        RasterizerState DrawSettings;

        public Renderer3DComponent(Mesh mesh)
        {
            // Set up material
            Material = new BasicEffect(Game.graphics.GraphicsDevice);
            Material.Alpha = 1;
            Material.VertexColorEnabled = true;
            Material.LightingEnabled = false;

            // Tris
            for (int i = mesh.VertexCount - 1; i >= 0; i--)
            {
                var Vert = mesh.Vertices[i];
                Verts.Add(new VertexPositionColor(new Vector3(Vert.X, Vert.Y, Vert.Z), Color.White));
            }

            // Buffer
            Buffer = new VertexBuffer(Game.graphics.GraphicsDevice, typeof(VertexPositionColor), Verts.Count, BufferUsage.WriteOnly);
            Buffer.SetData(Verts.ToArray());
        }

        public Renderer3DComponent(Mesh mesh, Vector3 ambientColor, Vector3 tint)
        {
            // Set up material
            Material = new BasicEffect(Game.graphics.GraphicsDevice);
            Material.Alpha = 1;
            Material.VertexColorEnabled = true;
            Material.LightingEnabled = true;
            Material.AmbientLightColor = ambientColor;
            Material.DiffuseColor = tint;

            // Tris
            for (int i = mesh.VertexCount - 1; i >= 0; i--)
            {
                var Vert = mesh.Vertices[i];
                Verts.Add(new VertexPositionColor(new Vector3(Vert.X, Vert.Y, Vert.Z), Color.White));
            }

            // Buffer
            Buffer = new VertexBuffer(Game.graphics.GraphicsDevice, typeof(VertexPositionColor), Verts.Count, BufferUsage.WriteOnly);
            Buffer.SetData(Verts.ToArray());
        }

        public override void Draw()
        {
            Material.Projection = Level.Active3DCamera.ProjectionMatrix;
            Material.View = Level.Active3DCamera.ViewMatrix;
            Material.World = LinkedObject.WorldMatrix;

            Game.graphics.GraphicsDevice.SetVertexBuffer(Buffer);

            DrawSettings = new RasterizerState();
            Game.graphics.GraphicsDevice.RasterizerState = DrawSettings;

            foreach (EffectPass Pass in Material.CurrentTechnique.Passes)
            {
                Pass.Apply();
                Game.graphics.GraphicsDevice.DrawPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, 0, Verts.Count);
            }
        }
    }
}
