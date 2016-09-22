﻿using System.Collections.Generic;
using System.Linq;
using Octgn.DataNew.Entities;

namespace Octgn.DeckBuilder
{
    public class SetPropertyDef : PropertyDef
    {
        private readonly IList<Set> _allSets;

        public SetPropertyDef(IEnumerable<Set> allSets)
        {
            base.Name = "Set";
            base.Type = 0;
            _allSets = allSets.OrderBy(s => s.Name).ToList();
        }

        public IList<Set> Sets
        {
            get { return _allSets; }
        }
    }
}