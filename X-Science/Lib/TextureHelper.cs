﻿using System.Reflection;
using UnityEngine;
using KSP.IO;


namespace ScienceChecklist{
	/// <summary>
	/// Contains static methods to assist in creating textures.
	/// </summary>

	internal static class TextureHelper {
		/// <summary>
		/// Creates a new Texture2D from an embedded resource.
		/// </summary>
		/// <param name="resource">The location of the resource in the assembly.</param>
		/// <param name="width">The width of the texture.</param>
		/// <param name="height">The height of the texture.</param>
		/// <returns></returns>
		public static Texture2D FromResource(string resource, int width, int height){
			var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
			var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource).ReadToEnd();
			if(iconStream == null) return null;
			tex.LoadImage(iconStream);
			tex.Apply();
			return tex;
		}


		public static Texture2D LoadImage(string filename, int width, int height){
			string filepath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + filename;
			if(System.IO.File.Exists(filepath)){
				var bytes = System.IO.File.ReadAllBytes(filepath);
				Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
				texture.LoadImage(bytes);
				return texture;
			}
			else{
				Debug.Log("X-Science: Can't load texture file (file not found): " + filepath);
				return null;
			}
		}
	}
}
