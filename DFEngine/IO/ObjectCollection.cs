using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DFEngine.Content.Items;
using DFEngine.Logging;

namespace DFEngine.IO
{
	/// <summary>
	/// Class that contains objects from a file that was processed.
	/// </summary>
	/// <typeparam name="T">The type of object.</typeparam>
	public class ObjectCollection<T>
	{
		/// <summary>
		/// The contained objects.
		/// </summary>
		private readonly Dictionary<dynamic, T> _objects;

		/// <summary>
		/// Creates a new instance of the <see cref="ObjectCollection{T}"/> class.
		/// </summary>
		public ObjectCollection()
		{
			_objects = new Dictionary<dynamic, T>();
		}

		/// <summary>
		/// Returns the object with the identity.
		/// </summary>
		/// <param name="identity">The identity of the object.</param>
		/// <returns>The object.</returns>
		public T this[byte identity] => !_objects.ContainsKey(identity) ? default(T) : _objects[identity];

		/// <summary>
		/// Returns the object with the identity.
		/// </summary>
		/// <param name="identity">The identity of the object.</param>
		/// <returns>The object.</returns>
		public T this[ushort identity] => !_objects.ContainsKey(identity) ? default(T) : _objects[identity];

		/// <summary>
		/// Returns the object with the identity.
		/// </summary>
		/// <param name="identity">The identity of the object.</param>
		/// <returns>The object.</returns>
		public T this[string identity] => !_objects.ContainsKey(identity) ? default(T) : _objects[identity];

		/// <summary>
		/// Adds an item to the collection if it's not already there.
		/// </summary>
		/// <param name="reader">The object to read data from.</param>
		public void Add(DataTableReader reader)
		{
			var instance = (T)Activator.CreateInstance(typeof(T));
			var properties = instance.GetType().GetProperties();

			PropertyInfo identityProperty = null;

			foreach (var property in properties)
			{
				var propertyType = property.PropertyType;
				var value = reader.GetValue(reader.GetOrdinal(property.Name));

				if (propertyType == typeof(byte))
				{
					property.SetValue(instance, (byte)value, null);
				}

				if (propertyType == typeof(ushort))
				{
					property.SetValue(instance, (ushort)value, null);
				}

				if (propertyType == typeof(uint))
				{
					property.SetValue(instance, (uint)value, null);
				}

				if (propertyType == typeof(string))
				{
					property.SetValue(instance, (string)value, null);
				}

				if (propertyType == typeof(ItemEquip))
				{
					property.SetValue(instance, (ItemEquip)(uint)value, null);
				}

				// Get the identity's value.
				if (property.GetCustomAttributes<Identity>().Any())
				{
					identityProperty = property;
				}
			}

			if (identityProperty == null)
			{
				EngineLog.Write(EngineLogLevel.Warning, "Definition identity property did not exist");
				return;
			}

			_objects.Add(identityProperty.GetValue(instance), instance);
		}
	}
}
