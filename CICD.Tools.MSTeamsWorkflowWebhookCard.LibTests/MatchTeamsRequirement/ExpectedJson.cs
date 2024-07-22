namespace Skyline.DataMiner.CICD.Tools.MSTeamsWorkflowWebhookCard.Lib.Tests
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents an action in a body element.
	/// </summary>
	public class Action
	{
		/// <summary>
		/// Gets or sets the title of the action.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Gets or sets the type of the action.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the URL associated with the action.
		/// </summary>
		public string? Url { get; set; }
	}

	/// <summary>
	/// Represents an attachment in a Teams message.
	/// </summary>
	public class Attachment
	{
		/// <summary>
		/// Gets or sets the content of the attachment.
		/// </summary>
		public Content? Content { get; set; }

		/// <summary>
		/// Gets or sets the content type of the attachment.
		/// </summary>
		public string? ContentType { get; set; }
	}

	/// <summary>
	/// Represents an element in the body of an attachment content.
	/// </summary>
	public class BodyElement
	{
		/// <summary>
		/// Gets or sets the list of actions in the body element.
		/// </summary>
		public List<Action>? Actions { get; set; }

		/// <summary>
		/// Gets or sets the alternative text for the body element.
		/// </summary>
		public string? AltText { get; set; }

		/// <summary>
		/// Gets or sets the list of columns in the body element.
		/// </summary>
		public List<Column>? Columns { get; set; }

		/// <summary>
		/// Gets or sets the list of facts in the body element.
		/// </summary>
		public List<Fact>? Facts { get; set; }

		/// <summary>
		/// Gets or sets the list of items in the body element.
		/// </summary>
		public List<Item>? Items { get; set; }

		/// <summary>
		/// Gets or sets the size of the body element.
		/// </summary>
		public string? Size { get; set; }

		/// <summary>
		/// Gets or sets the text of the body element.
		/// </summary>
		public string? Text { get; set; }

		/// <summary>
		/// Gets or sets the type of the body element.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the URL associated with the body element.
		/// </summary>
		public string? Url { get; set; }

		/// <summary>
		/// Gets or sets the width of the body element.
		/// </summary>
		public string? Width { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the text should wrap.
		/// </summary>
		public bool Wrap { get; set; }
	}

	/// <summary>
	/// Represents a column in a body element.
	/// </summary>
	public class Column
	{
		/// <summary>
		/// Gets or sets the list of items in the column.
		/// </summary>
		public List<Item>? Items { get; set; }

		/// <summary>
		/// Gets or sets the type of the column.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the width of the column.
		/// </summary>
		public string? Width { get; set; }
	}

	/// <summary>
	/// Represents the content of an attachment.
	/// </summary>
	public class Content
	{
		/// <summary>
		/// Gets or sets the list of body elements in the content.
		/// </summary>
		public List<BodyElement>? Body { get; set; }

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the version of the content.
		/// </summary>
		public string? Version { get; set; }
	}

	/// <summary>
	/// Represents a fact in a body element.
	/// </summary>
	public class Fact
	{
		/// <summary>
		/// Gets or sets the title of the fact.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Gets or sets the value of the fact.
		/// </summary>
		public string? Value { get; set; }
	}

	/// <summary>
	/// Represents an item in a body element or column.
	/// </summary>
	public class Item
	{
		/// <summary>
		/// Gets or sets the alternative text for the item.
		/// </summary>
		public string? AltText { get; set; }

		/// <summary>
		/// Gets or sets the size of the item.
		/// </summary>
		public string? Size { get; set; }

		/// <summary>
		/// Gets or sets the text of the item.
		/// </summary>
		public string? Text { get; set; }

		/// <summary>
		/// Gets or sets the type of the item.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the URL associated with the item.
		/// </summary>
		public string? Url { get; set; }

		/// <summary>
		/// Gets or sets the weight of the item.
		/// </summary>
		public string? Weight { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the text should wrap.
		/// </summary>
		public bool Wrap { get; set; }
	}

	/// <summary>
	/// Represents a Teams message.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// Gets or sets the list of attachments in the message.
		/// </summary>
		public List<Attachment>? Attachments { get; set; }

		/// <summary>
		/// Gets or sets the type of the message.
		/// </summary>
		public string? Type { get; set; }
	}
}