// /** 
//  * Matcher.cs
//  * Will Hart
//  * 20161208
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zen.Common.Extensions;

	#endregion

    public class Matcher
    {
        private readonly List<ComponentTypes> _match;
        private readonly List<ComponentTypes> _dontMatch;
        private readonly List<Entity> _matchedEntities = new List<Entity>();
        private Guid _storedGuid = Guid.Empty;

	    public Matcher() : this(new List<ComponentTypes>(), new List<ComponentTypes>())
	    {}
		
	    public Matcher(List<ComponentTypes> match) : this(match, new List<ComponentTypes>())
        {
        }

        public Matcher(List<ComponentTypes> match, List<ComponentTypes> dontMatch)
        {
            _match = match;
            _dontMatch = dontMatch;
        }

	    public Entity GetSingleMatch()
	    {
		    return GetMatches().First();
	    }

	    public Matcher AllOf(params ComponentTypes[] componentTypesToInclude)
	    {
		    _match.AddRange(componentTypesToInclude);
			InvalidateMatcher();
		    return this;
	    }

	    public Matcher NoneOf(params ComponentTypes[] componentTypesToExclude)
	    {
		    _dontMatch.AddRange(componentTypesToExclude);
			InvalidateMatcher();
			return this;
	    }

        public List<Entity> GetMatches(bool GetDisabled = false)
        {
            if (!_match.Any()) return new List<Entity>();

	        if (_storedGuid == EcsEngine.Instance.CurrentHash)
	        {
		        if (GetDisabled)
		        {
			        return _matchedEntities;
		        }
		        else
		        {
			        return _matchedEntities.Where(x => x.Wrapper.gameObject.HasNoTag(Tags.Disabled)).ToList();
		        }
	        }

            RefreshMatchedEntities();
            _storedGuid = EcsEngine.Instance.CurrentHash;
	        if (GetDisabled)
	        {
		        return _matchedEntities;
	        }
	        else
	        {
		        return _matchedEntities.Where(x => x.Wrapper.gameObject.HasNoTag(Tags.Disabled)).ToList();
	        }
        }

        private void RefreshMatchedEntities()
        { 
            // have to do it this way as we don't have access to the Entities from EcsEngine
            var first = _match.First();
            
            _matchedEntities.Clear();
            _matchedEntities.AddRange(EcsEngine.Instance
                .Get(first)
                .Select(comp => comp.Owner)
                .Where(ent => HasAll(_match, _dontMatch, ent)));
        }

		private void InvalidateMatcher()
		{
			_storedGuid = Guid.Empty;
		}

		private static bool HasAll(IEnumerable<ComponentTypes> match, List<ComponentTypes> dontMatch, Entity ent)
        {
            return match.All(ent.HasComponent) 
                && (!dontMatch.Any() || dontMatch.All(comp => !ent.HasComponent(comp)));
        }
    }
}