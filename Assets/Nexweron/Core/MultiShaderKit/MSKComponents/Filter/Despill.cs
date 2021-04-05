namespace Nexweron.Core.MSK
{
	using System.Collections.Generic;
	using UnityEngine;

	public class Despill : MSKComponentBase
	{
		[SerializeField, Range(0f, 1.0f)]
		private float _despill = 0.0f;
		public float despill
        {
			get { return _despill; }
			set { if (SetStruct(ref _despill, value)) _shaderMaterial.SetFloat("_Despill", value); }
		}

        [SerializeField, Range(0f, 1.0f)]
        private float _despillLuminanceAdd = 0.0f;
        public float despillLuminanceAdd
        {
            get { return _despillLuminanceAdd; }
            set { if (SetStruct(ref _despillLuminanceAdd, value)) _shaderMaterial.SetFloat("_DespillLuminanceAdd", value); }
        }

        private RenderTexture _rtF; //render texture Filter

		private List<string> _availableShaders = new List<string>() { @"MSK/Filter/Despill" };
		public override List<string> GetAvailableShaders() {
			return _availableShaders;
		}

		public override void UpdateShaderProperties() {
			_shaderMaterial.SetFloat("_Despill", _despill);
            _shaderMaterial.SetFloat("_DespillLuminanceAdd", _despillLuminanceAdd);
        }
	
		public override void UpdateSourceTexture() {
			RenderTextureUtils.SetRenderTextureSize(ref _rtF, _mskController.sourceTexture.width, _mskController.sourceTexture.height);
		}

		public override RenderTexture GetRender(Texture src) {
			_rtF.DiscardContents();
			Graphics.Blit(src, _rtF, _shaderMaterial);
			return _rtF;
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			if (_rtF != null) {
				DestroyImmediate(_rtF);
			}
		}
	}
}