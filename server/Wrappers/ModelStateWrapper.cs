using server.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace server.Wrappers
{
	/**
	 * This class is used to wrap .Nets ModelState and implement our custom IValidationDictionary interface so the validation can be done in the service layer without creating a dependency between the service and controller layer
	 * This way our services depend on IValidationDictionary type of classes and not concretely on the ModelState provided by ASP.Net 
	 */
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

