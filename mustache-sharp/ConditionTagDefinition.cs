﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mustache
{
    /// <summary>
    /// Defines a tag that conditionally prints its content.
    /// </summary>
    internal abstract class ConditionTagDefinition : TagDefinition
    {
        private const string conditionParameter = "condition";

        /// <summary>
        /// Initializes a new instance of a ConditionTagDefinition.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        protected ConditionTagDefinition(string tagName)
            : base(tagName, true)
        {
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override TagParameter[] GetParameters()
        {
            return new TagParameter[] { new TagParameter(conditionParameter) { IsRequired = true } };
        }

        /// <summary>
        /// Gets whether the tag will contain content.
        /// </summary>
        public override bool HasBody
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the tags that come into scope within the context of the current tag.
        /// </summary>
        /// <returns>The child tag definitions.</returns>
        protected override TagDefinition[] GetChildTags()
        {
            return new TagDefinition[] 
            {
                new ElifTagDefinition(),
                new ElseTagDefinition(),
            };
        }

        /// <summary>
        /// Gets whether the given tag's generator should be used for a secondary (or substitute) text block.
        /// </summary>
        /// <param name="definition">The tag to inspect.</param>
        /// <returns>True if the tag's generator should be used as a secondary generator.</returns>
        public override bool ShouldCreateSecondaryGroup(TagDefinition definition)
        {
            return (definition is ElifTagDefinition) || (definition is ElseTagDefinition);
        }

        /// <summary>
        /// Gets whether the primary generator group should be used to render the tag.
        /// </summary>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <returns>
        /// True if the primary generator group should be used to render the tag;
        /// otherwise, false to use the secondary group.
        /// </returns>
        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            object condition = arguments[conditionParameter];
            return isConditionSatisfied(condition);
        }

        private bool isConditionSatisfied(object condition)
        {
            if (condition == null)
            {
                return false;
            }
            IEnumerable enumerable = condition as IEnumerable;
            if (enumerable != null)
            {
                return enumerable.Cast<object>().Any();
            }
            if (condition is Char)
            {
                return (Char)condition != '\0';
            }
            try
            {
                decimal number = (decimal)Convert.ChangeType(condition, typeof(decimal));
                return number != 0.0m;
            }
            catch
            {
                return true;
            }
        }
    }
}
