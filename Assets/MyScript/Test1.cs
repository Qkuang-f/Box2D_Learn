using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Box2DSharp.Common;
using Box2DSharp.Collision.Shapes;
using Random = System.Random;

namespace QkF.Box2D
{

    public class Test1 : MonoBehaviour 
    {
        // Start is called before the first frame update

        public QkUnityDrawer unityDrawer;

        private Random Random;
        private QkPhysicalDrawer physicalDrawer;
        private World World;

        void Start()
        {
            // 初始化
             
            this.physicalDrawer = new QkPhysicalDrawer();
            this.World = new World(new Vector2(0,-10));

            this.physicalDrawer.Drawer = unityDrawer;
            World.SetDebugDrawer(physicalDrawer);

            this.physicalDrawer.Flags = DrawFlag.DrawShape | DrawFlag.DrawContactPoint;


            // 创建
            this.Random = new Random();

            // bodyDef
            var groundBodyDef = new BodyDef { BodyType = BodyType.StaticBody };
            groundBodyDef.Position.Set(0.0f, -10.0f);

            // body
            var groundBody = World.CreateBody(groundBodyDef);

            // shape
            var groundBox = new PolygonShape();
            groundBox.SetAsBox(1000.0f, 10.0f);

            // fixture : shape -> body
            groundBody.CreateFixture(groundBox, 0.0f);

            // Define the dynamic body. We set its position and call the body factory.
            var bodyDef = new BodyDef { BodyType = BodyType.DynamicBody };

            bodyDef.Position.Set(0, 4f);

            var dynamicBox = new PolygonShape();
            dynamicBox.SetAsBox(1f, 1f, Vector2.Zero, 45f);

            // Define the dynamic body fixture.
            var fixtureDef = new FixtureDef
            {
                Shape = dynamicBox,
                Density = 1.0f,
                Friction = 0.3f
            };

            // Set the box density to be non-zero, so it will be dynamic.

            // Override the default friction.

            // Add the shape to the body.
            var body = World.CreateBody(bodyDef);
            body.CreateFixture(fixtureDef);
             
            for (int i = 0; i < 100; i++)
            {
                bodyDef.Position = new Vector2(Random.Next(-50, 50), Random.Next(0, 500));
                bodyDef.Angle = Random.Next(0, 360);
                World.CreateBody(bodyDef).CreateFixture(fixtureDef);
            }

        }

        private void FixedUpdate()
        {
            unityDrawer.Clear();
            World.Step(Time.fixedDeltaTime, 8, 3);
            World.DebugDraw();
        }

        private void OnRenderObject()
        {
            unityDrawer.GLDraw();
        }

    }
}
