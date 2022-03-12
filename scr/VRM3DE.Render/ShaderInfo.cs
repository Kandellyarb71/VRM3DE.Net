using System.Collections.Generic;

namespace VRM3DE.Render
{
	public struct ShaderInfo
	{
		public static ShaderInfo Standard
		{
			get
			{
				return _standard;
			}
		}
		public string? Path { get; set; }
		public Dictionary<ShaderUniforms, string?> UniformNames { get; }
		private static readonly ShaderInfo _standard = new ShaderInfo(Paths.Shader, new[]
		{
			new KeyValuePair<ShaderUniforms, string?>(ShaderUniforms.Resolution, "CameraResolution"),
			new KeyValuePair<ShaderUniforms, string?>(ShaderUniforms.Position, "CameraPosition"),
			new KeyValuePair<ShaderUniforms, string?>(ShaderUniforms.Rotation, "CameraRotation")
		});

		public ShaderInfo(string path, KeyValuePair<ShaderUniforms, string?>[] uniformNames)
		{
			Path = path;
			UniformNames = new Dictionary<ShaderUniforms, string?>();
			foreach (KeyValuePair<ShaderUniforms, string?> pair in uniformNames)
            {
				UniformNames.Add(pair.Key, pair.Value);
            }
		}
	}

	public enum ShaderUniforms
	{
		Resolution,
		Position,
		Rotation
	}
}