using Ode.Net.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class Collide : Transform<Geom, ContactGeom[]>
    {
        public int MaxContacts { get; set; }

        public override IObservable<ContactGeom[]> Process(IObservable<Geom> source)
        {
            return source.Select(input =>
            {
                var contacts = new ContactGeom[MaxContacts];
                var ncontacts = Geom.Collide(input, input.Space, contacts);
                Array.Resize(ref contacts, ncontacts);
                return contacts;
            });
        }
    }
}
