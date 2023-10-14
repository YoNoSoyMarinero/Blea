using server.Interfaces;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace server.Wrappers
{
	public class ModelStateWrapper : IValidationDictionary
	{
		private ModelStateDictionary _modelState;

		public ModelStateWrapper(ModelStateDictionary modelState)
		{
			_modelState = modelState;
		}

		public void AddError(string key, string errorMessage)
		{
			_modelState.AddModelError(key, errorMessage);
		}

		public bool IsValid
		{
			get { return _modelState.IsValid; }
		}
	}
}

