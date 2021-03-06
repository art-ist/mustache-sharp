﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mustache {
    /// <summary>
    /// Defines a tag that conditionally prints its content, based on whether the passed in values are equal
    /// </summary>
    internal sealed class GteTagDefinition : ConditionTagDefinition {
        private const string ConditionParameter = "condition";
        private const string TargetValueParameter = "targetValue";

        /// <summary>
        /// Initializes a new instance of a IfTagDefinition.
        /// </summary>
        public GteTagDefinition()
            : base("gte") {}

        /// <summary>
        /// Gets whether the tag only exists within the scope of its parent.
        /// </summary>
        protected override bool GetIsContextSensitive() {
            return false;
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override IEnumerable<TagParameter> GetParameters() {
            return new[] {
                             new TagParameter(ConditionParameter) {IsRequired = true},
                             new TagParameter(TargetValueParameter) {IsRequired = true}
                         };
        }


        /// <summary>
        /// Gets whether the primary generator group should be used to render the tag.
        /// </summary>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <returns>
        /// True if the primary generator group should be used to render the tag;
        /// otherwise, false to use the secondary group.
        /// </returns>
        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments) {
            object condition = arguments[ConditionParameter];
            object targetValue = arguments[TargetValueParameter];
            return isConditionSatisfied(condition, targetValue);
        }

        private bool isConditionSatisfied(object condition, object targetValue) {
            if (condition == null || targetValue == null) {
                return false;
            }

            try {
                return Convert.ToDouble(condition) >= Convert.ToDouble(targetValue);
            } catch (Exception exception) {
                return false;
            }



            return false;
        }

        /// <summary>
        /// Gets the parameters that are used to create a new child context.
        /// </summary>
        /// <returns>The parameters that are used to create a new child context.</returns>
        public override IEnumerable<TagParameter> GetChildContextParameters() {
            return new TagParameter[0];
        }
    }
}
