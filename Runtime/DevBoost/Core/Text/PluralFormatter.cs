using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevBoost.Serialization;

namespace DevBoost.Text
{
	/// <summary>
	/// Data object for creating formatters that can respond to a format description being different if the amount is singular or plural.
	/// </summary>
	public class PluralFormatter : ISerializable
	{

		#region Constants

		/// <summary>
		/// The key for the singular formatter.
		/// </summary>
		private const string SINGLUAR_FORMATTER_KEY = "singular";

		/// <summary>
		/// The key for the plural formatter.
		/// </summary>
		private const string PLURAL_FORMATTER_KEY = "plural";

		#endregion

		#region Data

		/// <summary>
		/// Formatter for this object when the amount is exactly 1.
		/// </summary>
		protected string singularFormatter = string.Empty;

		/// <summary>
		/// Formatter for this object when the amount is not 1.
		/// </summary>
		protected string pluralFormatter = string.Empty;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor for use with Serialization utils.
		/// </summary>
		public PluralFormatter() { }

		/// <summary>
		/// Create a formatter manually.
		/// </summary>
		/// <param name="singularFormatter">Formatter to use for amounts where there is only one.</param>
		/// <param name="pluralFormatter">Formatter to use for amounts where the amount noun should be pluralized.</param>
		public PluralFormatter(string singularFormatter, string pluralFormatter)
		{
			this.singularFormatter = singularFormatter;
			this.pluralFormatter = pluralFormatter;
		}

		#endregion

		#region Plural Formattter Logic

		/// <summary>
		/// Gets a formatted string for the provided amount.
		/// </summary>
		/// <param name="amount">The amount to put into a string.</param>
		/// <returns>A formatted string, either singular or plural depending on the amount value.</returns>
		public virtual string GetStringForAmount(int amount)
		{
			if (System.Math.Abs(amount) == 1)
			{
				return string.Format(this.singularFormatter, amount);
			}
			else
			{
				return string.Format(this.pluralFormatter, amount);
			}
		}

		/// <summary>
		/// Gets a formatted string for the provided amount.
		/// </summary>
		/// <param name="amount">The amount to put into a string.</param>
		/// <returns>A formatted string, either singular or plural depending on the amount value.</returns>
		public virtual string GetStringForAmount(long amount, string numericalFormatter = null)
		{
			if (System.Math.Abs(amount) == 1L)
			{
				return string.Format(this.singularFormatter, amount);
			}
			else
			{
				return string.Format(this.pluralFormatter, amount);
			}
		}

		/// <summary>
		/// Gets a formatted string for the provided amount.
		/// </summary>
		/// <param name="amount">The amount to put into a string.</param>
		/// <returns>A formatted string, either singular or plural depending on the amount value.</returns>
		public virtual string GetStringForAmount(float amount, string numericalFormatter = null)
		{
			if (MathUtils.Approximately(System.Math.Abs(amount), 1f))
			{
				return string.Format(this.singularFormatter, amount);
			}
			else
			{
				return string.Format(this.pluralFormatter, amount);
			}
		}

		/// <summary>
		/// Gets a formatted string for the provided amount.
		/// </summary>
		/// <param name="amount">The amount to put into a string.</param>
		/// <returns>A formatted string, either singular or plural depending on the amount value.</returns>
		public virtual string GetStringForAmount(double amount, string numericalFormatter = null)
		{
			if (MathUtils.Approximately(System.Math.Abs(amount), 1.0d))
			{
				return string.Format(this.singularFormatter, amount);
			}
			else
			{
				return string.Format(this.pluralFormatter, amount);
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Deserialize a serialized formatter into this object.
		/// </summary>
		/// <param name="serializedObject">Serialized formatter.</param>
		public void Deserialize(Dictionary<string, object> serializedObject)
		{
			this.singularFormatter = serializedObject.AssertValueForKey<string>(SINGLUAR_FORMATTER_KEY);
			this.pluralFormatter = serializedObject.AssertValueForKey<string>(PLURAL_FORMATTER_KEY);
		}

		/// <summary>
		/// Serialize this formatter.
		/// </summary>
		/// <returns>A serialized formatter.</returns>
		public Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> serializedFormatter = new Dictionary<string, object>();

			serializedFormatter[SINGLUAR_FORMATTER_KEY] = this.singularFormatter;
			serializedFormatter[PLURAL_FORMATTER_KEY] = this.pluralFormatter;

			return serializedFormatter;
		}

		#endregion

	}
}
