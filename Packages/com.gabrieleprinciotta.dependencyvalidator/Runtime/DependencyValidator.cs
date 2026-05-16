using System.Collections.Generic;
using UnityEngine;

namespace MyUtils.DependencyValidator
{
    /// <summary>
    /// Provides utility methods for validating object dependencies and component existence in Unity.
    /// </summary>
    public static class DependencyValidator
    {
        /// <summary>
        /// Validates that the provided object reference is not null.
        /// </summary>
        public static void NotNull<T>(T value, Object context)
            where T : Object
        {
            if (value != null)
                return;

            var message = $"[{context.GetType().Name}] Required reference is not assigned.";
            Debug.LogError(message, context);
            throw new MissingReferenceException(message);
        }

        /// <summary>
        /// Validates that the provided list is not null and contains at least one element.
        /// </summary>
        public static void ListNotNull<T>(List<T> list, Object context)
            where T : Object
        {
            if (list != null && list.Count > 0)
                return;

            var message = $"[{context.GetType().Name}] Required list reference is not assigned or empty.";
            Debug.LogError(message, context);
            throw new MissingReferenceException(message);
        }

        /// <summary>
        /// Validates that the provided component exists on the GameObject.
        /// </summary>
        public static void ComponentExist<T>(T component, Object context)
            where T : Component
        {
            if (component != null)
                return;

            var message = $"[{context.GetType().Name}] Component was not found on this object.";
            Debug.LogError(message, context);
            throw new MissingComponentException(message);
        }
    }
}
