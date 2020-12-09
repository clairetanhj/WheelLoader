using UnityEditor;
using Pixyz.Tools.Editor;

namespace Pixyz.Tools {

	// THIS SCRIPT IS AUTOGENERATED. PLEASE DO NOT MODIFY OR MOVE IT.
	public static class ToolboxMenuItems {

		[MenuItem("Pixyz/Toolbox/Create Collider", priority = 22)]
		[MenuItem("GameObject/Pixyz/Create Collider", priority = -21)]
		public static void A137927() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.AddCollider()); }

		[MenuItem("Pixyz/Toolbox/Decimate", priority = 22)]
		[MenuItem("GameObject/Pixyz/Decimate", priority = -19)]
		public static void A1577496() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.Decimate()); }

		[MenuItem("Pixyz/Toolbox/Merge", priority = 22)]
		[MenuItem("GameObject/Pixyz/Merge", priority = -18)]
		public static void A7614467() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.Merge()); }

		[MenuItem("Pixyz/Toolbox/Explode", priority = 22)]
		[MenuItem("GameObject/Pixyz/Explode", priority = -17)]
		public static void A1060204() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.ExplodeSubmeshes()); }

		[MenuItem("Pixyz/Toolbox/Create UVs", priority = 22)]
		[MenuItem("GameObject/Pixyz/Create UVs", priority = -16)]
		public static void A3882666() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.CreateUVs()); }

		[MenuItem("Pixyz/Toolbox/Create UVs for Lightmaps", priority = 22)]
		[MenuItem("GameObject/Pixyz/Create UVs for Lightmaps", priority = -15)]
		public static void A9095070() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.CreateLightmapUVs()); }

		[MenuItem("Pixyz/Toolbox/Create Normals", priority = 22)]
		[MenuItem("GameObject/Pixyz/Create Normals", priority = -14)]
		public static void A586584() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.CreateNormals()); }

		[MenuItem("Pixyz/Toolbox/Flip Normals", priority = 22)]
		[MenuItem("GameObject/Pixyz/Flip Normals", priority = -13)]
		public static void A7776955() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.FlipNormals()); }

		[MenuItem("Pixyz/Toolbox/Replace by...", priority = 22)]
		[MenuItem("GameObject/Pixyz/Replace by...", priority = -12)]
		public static void A9954294() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.ReplaceBy()); }

		[MenuItem("Pixyz/Toolbox/Remove Hidden", priority = 22)]
		[MenuItem("GameObject/Pixyz/Remove Hidden", priority = -11)]
		public static void A8309787() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.RemoveHidden()); }

		[MenuItem("Pixyz/Toolbox/Repair Mesh", priority = 22)]
		[MenuItem("GameObject/Pixyz/Repair Mesh", priority = -10)]
		public static void A7129113() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.RepairMesh()); }

		[MenuItem("Pixyz/Toolbox/Retopologize", priority = 22)]
		[MenuItem("GameObject/Pixyz/Retopologize", priority = -9)]
		public static void A7180458() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.Retopologize()); }

		[MenuItem("Pixyz/Toolbox/Move Pivot", priority = 22)]
		[MenuItem("GameObject/Pixyz/Move Pivot", priority = -7)]
		public static void A5774471() { Toolbox.RunToolboxAction(new Pixyz.Tools.Builtin.MoveOrigin()); }

	}
}