/*
* Farseer Physics Engine:
* Copyright (c) 2012 Ian Qvist
* 
* Original source Box2D:
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public static class FarseerPhysicsTestEntries
    {
        public static FarseerPhysicsTestEntry[] TestList =
        {
            //Original tests
            new FarseerPhysicsTestEntry {Name = "Car", CreateTest = CarTest.Create},
            new FarseerPhysicsTestEntry {Name = "Pinball", CreateTest = PinballTest.Create},
            new FarseerPhysicsTestEntry {Name = "Revolute", CreateTest = RevoluteTest.Create},
            new FarseerPhysicsTestEntry {Name = "Gears", CreateTest = GearsTest.Create},
            new FarseerPhysicsTestEntry {Name = "RopeJoint", CreateTest = RopeTest.Create},
            new FarseerPhysicsTestEntry {Name = "Mobile", CreateTest = MobileTest.Create},
            new FarseerPhysicsTestEntry {Name = "MobileBalanced", CreateTest = MobileBalancedTest.Create},
            new FarseerPhysicsTestEntry {Name = "Cantilever", CreateTest = CantileverTest.Create},
            new FarseerPhysicsTestEntry {Name = "Character collision", CreateTest = CharacterCollisionTest.Create},
            new FarseerPhysicsTestEntry {Name = "Apply Force", CreateTest = ApplyForceTest.Create},
            new FarseerPhysicsTestEntry {Name = "Chain", CreateTest = ChainTest.Create},
            new FarseerPhysicsTestEntry {Name = "Theo Jansen's Walker", CreateTest = TheoJansenTest.Create},
            new FarseerPhysicsTestEntry {Name = "Bridge", CreateTest = BridgeTest.Create},
            new FarseerPhysicsTestEntry {Name = "Breakable", CreateTest = BreakableTest.Create},
            new FarseerPhysicsTestEntry {Name = "Add Pair Stress Test", CreateTest = AddPairTest.Create},
            new FarseerPhysicsTestEntry {Name = "Dominos", CreateTest = DominosTest.Create},
            new FarseerPhysicsTestEntry {Name = "Motor joint", CreateTest = MotorJointTest.Create},
            new FarseerPhysicsTestEntry {Name = "One-Sided Platform", CreateTest = OneSidedPlatformTest.Create},
            new FarseerPhysicsTestEntry {Name = "Web", CreateTest = WebTest.Create},
            //new FarseerPhysicsTestEntry {Name = "Dump Shell", CreateTest = DumpShellTest.Create},
            new FarseerPhysicsTestEntry {Name = "Ray-Cast", CreateTest = RayCastTest.Create},
            new FarseerPhysicsTestEntry {Name = "Conveyor Belt", CreateTest = ConveyorBeltTest.Create},
            new FarseerPhysicsTestEntry {Name = "ConvexHull", CreateTest = ConvexHullTest.Create},
            new FarseerPhysicsTestEntry {Name = "Varying Restitution", CreateTest = VaryingRestitutionTest.Create},
            new FarseerPhysicsTestEntry {Name = "Tumbler", CreateTest = TumblerTest.Create},
            new FarseerPhysicsTestEntry {Name = "Tiles", CreateTest = TilesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Edge Test", CreateTest = EdgeTest.Create},
            new FarseerPhysicsTestEntry {Name = "Body Types", CreateTest = BodyTypesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Shape Editing", CreateTest = ShapeEditingTest.Create},
            new FarseerPhysicsTestEntry {Name = "Prismatic", CreateTest = PrismaticTest.Create},
            new FarseerPhysicsTestEntry {Name = "Vertical Stack", CreateTest = VerticalStackTest.Create},
            new FarseerPhysicsTestEntry {Name = "SphereStack", CreateTest = SphereStackTest.Create},
            new FarseerPhysicsTestEntry {Name = "Pulleys", CreateTest = PulleysTest.Create},
            new FarseerPhysicsTestEntry {Name = "Polygon Shapes", CreateTest = PolyShapesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Bullet Test", CreateTest = BulletTest.Create},
            new FarseerPhysicsTestEntry {Name = "Confined", CreateTest = ConfinedTest.Create},
            new FarseerPhysicsTestEntry {Name = "Pyramid", CreateTest = PyramidTest.Create},
            new FarseerPhysicsTestEntry {Name = "Edge Shapes", CreateTest = EdgeShapesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Collision Filtering", CreateTest = CollisionFilteringTest.Create},
            new FarseerPhysicsTestEntry {Name = "Collision Processing", CreateTest = CollisionProcessingTest.Create},
            new FarseerPhysicsTestEntry {Name = "Compound Shapes", CreateTest = CompoundShapesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Distance Test", CreateTest = DistanceTest.Create},
            new FarseerPhysicsTestEntry {Name = "Dynamic Tree", CreateTest = DynamicTreeTest.Create},
            new FarseerPhysicsTestEntry {Name = "Sensor Test", CreateTest = SensorTest.Create},
            new FarseerPhysicsTestEntry {Name = "Slider Crank", CreateTest = SliderCrankTest.Create},
            new FarseerPhysicsTestEntry {Name = "Varying Friction", CreateTest = VaryingFrictionTest.Create},
            //FPE tests
            new FarseerPhysicsTestEntry {Name = "YuPeng Polygon", CreateTest = YuPengPolygonTest.Create},
            new FarseerPhysicsTestEntry {Name = "Path Test", CreateTest = PathTest.Create},
            new FarseerPhysicsTestEntry {Name = "Cutting of polygons", CreateTest = CuttingTest.Create},
            new FarseerPhysicsTestEntry {Name = "Gravity Controller Test", CreateTest = GravityControllerTest.Create},
            new FarseerPhysicsTestEntry {Name = "Texture to Vertices", CreateTest = TextureVerticesTest.Create},
            new FarseerPhysicsTestEntry {Name = "Rounded rectangle", CreateTest = RoundedRectangle.Create},
            new FarseerPhysicsTestEntry {Name = "Angle Joint", CreateTest = AngleJointTest.Create},
            new FarseerPhysicsTestEntry {Name = "Explosion", CreateTest = ExplosionTest.Create},
            new FarseerPhysicsTestEntry {Name = "Lock Test", CreateTest = LockTest.Create},
            new FarseerPhysicsTestEntry {Name = "Sphere benchmark", CreateTest = CircleBenchmarkTest.Create},
            new FarseerPhysicsTestEntry {Name = "Edgeshape benchmark", CreateTest = EdgeShapeBenchmarkTest.Create},
            new FarseerPhysicsTestEntry {Name = "Circle penetration", CreateTest = CirclePenetrationTest.Create},
            new FarseerPhysicsTestEntry {Name = "Clone Test", CreateTest = CloneTest.Create},
            new FarseerPhysicsTestEntry {Name = "Deletion test", CreateTest = DeletionTest.Create},
            new FarseerPhysicsTestEntry {Name = "Buoyancy test", CreateTest = BuoyancyTest.Create},
            new FarseerPhysicsTestEntry {Name = "Convex hull test", CreateTest = ConvexHullTest2.Create},
            new FarseerPhysicsTestEntry {Name = "Simple Wind Force Test", CreateTest = SimpleWindForceTest.Create},
            new FarseerPhysicsTestEntry {Name = "Simplification", CreateTest = SimplificationTest.Create},
            //new FarseerPhysicsTestEntry {Name = "Triangulation", CreateTest = TriangulationTest.Create},
            new FarseerPhysicsTestEntry {Name = "Destructible Terrain Test", CreateTest = DestructibleTerrainTest.Create},
            new FarseerPhysicsTestEntry {Name = "Check polygon", CreateTest = CheckPolygonTest.Create},
            new FarseerPhysicsTestEntry {Name = "Continuous Test", CreateTest = ContinuousTest.Create},
            new FarseerPhysicsTestEntry {Name = "Time of Impact", CreateTest = TimeOfImpactTest.Create},
            new FarseerPhysicsTestEntry {Name = "PolyCollision", CreateTest = PolyCollisionTest.Create},
            new FarseerPhysicsTestEntry {Name = null, CreateTest = null}
        };
    }
}
