namespace server.Interfaces
{
	public interface IValidationDictionary
	{
		void AddError(string key, string errorMessage);
		bool IsValid { get; }
		Dictionary<string, string[]> GetErrors();
	}
}

