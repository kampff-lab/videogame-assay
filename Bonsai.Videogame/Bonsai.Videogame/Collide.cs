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

        public IObservable<ContactGeom[]> Process<TGeom1, TGeom2>(IObservable<Tuple<TGeom1, TGeom2>> source)
            where TGeom1 : Geom
            where TGeom2 : Geom
        {
            return source.Select(input =>
            {
                var contacts = new ContactGeom[MaxContacts];
                var ncontacts = Geom.Collide(input.Item1, input.Item2, contacts);
                Array.Resize(ref contacts, ncontacts);
                return contacts;
            });
        }
    }
}
