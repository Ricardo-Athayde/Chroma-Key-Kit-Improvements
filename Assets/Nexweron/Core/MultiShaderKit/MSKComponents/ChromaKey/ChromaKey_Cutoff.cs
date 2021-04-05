namespace Nexweron.Core.MSK
{
	using System.Collections.Generic;
	using UnityEngine;

	public class ChromaKey_Cutoff : MSKComponentBase
	{
		[SerializeField, Range(0.0f, 1.0f), Tooltip("Left Crop")]
		private float _leftCrop = 0f;
		public float leftCrop
        {
			get { return _leftCrop; }
			set { if (SetStruct(ref _leftCrop, value)) _shaderMaterial.SetFloat("_CropLeft", value); }
		}

        [SerializeField, Range(0.0f, 1.0f), Tooltip("Right Crop")]
        private float _rightCrop = 0f;
        public float rightCrop
        {
            get { return _rightCrop; }
            set { if (SetStruct(ref _rightCrop, value)) _shaderMaterial.SetFloat("_CropRight", value); }
        }

        [SerializeField, Range(0.0f, 1.0f), Tooltip("Up Crop")]
        private float _upCrop = 0f;
        public float upCrop
        {
            get { return _upCrop; }
            set { if (SetStruct(ref _upCrop, value)) _shaderMaterial.SetFloat("_CropUp", value); }
        }

        [SerializeField, Range(0.0f, 1.0f), Tooltip("Down Crop")]
        private float _downCrop = 0f;
        public float downCrop
        {
            get { return _downCrop; }
            set { if (SetStruct(ref _downCrop, value)) _shaderMaterial.SetFloat("_CropDown", value); }
        }


        private RenderTexture _rtC; //render texture Chroma

		private List<string> _availableShaders = new List<string>() { @"MSK/ChromaKey/BlendOff/ChromaKey_Cutoff" };
		public override List<string> GetAvailableShaders() {
			return _availableShaders;
		}

		public override void UpdateShaderProperties() {
			_shaderMaterial.SetFloat("_CropLeft", _leftCrop);
			_shaderMaterial.SetFloat("_CropRight", _rightCrop);
			_shaderMaterial.SetFloat("_CropUp", _upCrop);
			_shaderMaterial.SetFloat("_CropDown", _downCrop);
		}

		public override void UpdateSourceTexture() {
			RenderTextureUtils.SetRenderTextureSize(ref _rtC, _mskController.sourceTexture.width, _mskController.sourceTexture.height);
		}

		public override RenderTexture GetRender(Texture src) {
			_rtC.DiscardContents();
			Graphics.Blit(src, _rtC, _shaderMaterial);
			return _rtC;
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			if(_rtC != null) {
				DestroyImmediate(_rtC);
			}
		}
	}
}