using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    class ExpandableCollectionConverter : CollectionConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var collection = (ICollection)value;
            var enumerator = collection.GetEnumerator();
            var properties = new PropertyDescriptor[collection.Count];
            for (int i = 0; i < properties.Length; i++)
            {
                if (!enumerator.MoveNext()) throw new InvalidOperationException("Invalid collection iterator.");
                properties[i] = new ExpandablePropertyDescriptor(i, enumerator.Current);
            }
            return new PropertyDescriptorCollection(properties, true);
        }

        class ExpandablePropertyDescriptor : SimplePropertyDescriptor
        {
            readonly string value;

            public ExpandablePropertyDescriptor(int index, object element)
                : base(typeof(Collection<string>), "[" + index + "]", typeof(string))
            {
                value = element.ToString();
            }

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public override object GetValue(object component)
            {
                return value;
            }

            public override void SetValue(object component, object value)
            {
                throw new NotSupportedException();
            }
        }
    }
}
