using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class RandomName : Transform<Random, string>
    {
        readonly Collection<string> names = new Collection<string>();

        [TypeConverter(typeof(NameConverter))]
        public Collection<string> Names
        {
            get { return names; }
        }

        public override IObservable<string> Process(IObservable<Random> source)
        {
            return source.Select(input => names[input.Next(names.Count)]);
        }

        class NameConverter : CollectionConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                var names = (Collection<string>)value;
                var properties = new PropertyDescriptor[names.Count];
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = new NamePropertyDescriptor(i, names[i]);
                }
                return new PropertyDescriptorCollection(properties, true);
            }

            class NamePropertyDescriptor : SimplePropertyDescriptor
            {
                readonly string value;

                public NamePropertyDescriptor(int index, string name)
                    : base(typeof(Collection<string>), "[" + index + "]", typeof(string))
                {
                    value = name;
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
}
