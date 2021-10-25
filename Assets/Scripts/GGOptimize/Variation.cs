using System;
using System.Collections.Generic;

namespace GGOptimize
{
	[Serializable]
	public class Variation
	{
		public Variation()
		{
		}

		public Variation(string name)
		{
			this.name = name;
		}

		protected Dictionary<string, NamedProperty> propertyDictionary
		{
			get
			{
				if (this.propertyDictionary_ == null)
				{
					this.propertyDictionary_ = new Dictionary<string, NamedProperty>();
					for (int i = 0; i < this.properties.Count; i++)
					{
						NamedProperty namedProperty = this.properties[i];
						if (!this.propertyDictionary_.ContainsKey(namedProperty.name))
						{
							this.propertyDictionary_.Add(namedProperty.name, namedProperty);
						}
						this.propertyDictionary_[namedProperty.name] = namedProperty;
					}
				}
				return this.propertyDictionary_;
			}
		}

		public NamedProperty GetProperty(string name)
		{
			if (name == null)
			{
				return null;
			}
			Dictionary<string, NamedProperty> propertyDictionary = this.propertyDictionary;
			if (propertyDictionary == null)
			{
				for (int i = this.properties.Count - 1; i >= 0; i--)
				{
					NamedProperty namedProperty = this.properties[i];
					if (namedProperty.name == name)
					{
						return namedProperty;
					}
				}
				return null;
			}
			if (!propertyDictionary.ContainsKey(name))
			{
				return null;
			}
			return propertyDictionary[name];
		}

		public string name;

		public List<NamedProperty> properties = new List<NamedProperty>();

		protected Dictionary<string, NamedProperty> propertyDictionary_;
	}
}
