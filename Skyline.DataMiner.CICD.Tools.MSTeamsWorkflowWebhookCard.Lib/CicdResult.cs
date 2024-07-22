namespace Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib
{
	/// <summary>
	/// Represents the result of a CICD process.
	/// </summary>
	public enum CicdResult
	{
		/// <summary>
		/// Indicates a successful result.
		/// </summary>
		Success,

		/// <summary>
		/// Indicates an unstable result.
		/// </summary>
		Unstable,

		/// <summary>
		/// Indicates a failed result.
		/// </summary>
		Failure,

		/// <summary>
		/// Indicates a warning result.
		/// </summary>
		Warning
	}
}