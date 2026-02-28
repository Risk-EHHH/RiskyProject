using System.Collections.Generic;
using UnityEngine;

namespace Risk.Runtime.Utils
{
    /// <summary>
    /// Provides utility methods for validating object dependencies and component existence in Unity.
    /// This class is primarily used to ensure that required references or components are assigned from
    /// the inspector or exist during runtime, offering a standardized approach for error handling and debugging.
    /// </summary>
    public static class DependencyValidator
    {
        /// <summary>
        /// Validates that the provided object reference is not null, used for fields assigned in the inspector.
        /// If the reference is null, an error message is logged and a <see cref="MissingReferenceException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated, constrained to Unity's Object class.</typeparam>
        /// <param name="value">The object reference to validate.</param>
        /// <param name="context">The context in which the validation is being performed,
        /// typically the MonoBehaviour or ScriptableObject that owns the reference.</param>
        /// <exception cref="MissingReferenceException">Thrown if the provided object reference is null.</exception>
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
        /// If the list is null or empty, an error message is logged and a <see cref="MissingReferenceException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list, constrained to Unity's Object class.</typeparam>
        /// <param name="list">The list to validate.</param>
        /// <param name="context">The context in which the validation is being performed,
        /// typically the MonoBehaviour or ScriptableObject that owns the reference.</param>
        /// <exception cref="MissingReferenceException">Thrown if the provided list is null or empty.</exception>
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
        /// Validates that the provided component exists on the GameObject associated with the context.
        /// If the component is not found, an error message is logged and execution is halted.
        /// </summary>
        /// <typeparam name="T">The type of the component being validated, constrained to Unity's Component class.</typeparam>
        /// <param name="component">The component to validate for existence.</param>
        /// <param name="context">The context in which the validation is being performed,
        /// typically the MonoBehaviour or ScriptableObject that owns the reference.</param>
        /// <exception cref="MissingComponentException">Thrown if the specified component does not exist on the GameObject.</exception>
        public static void ComponentExist<T>(T component, Object context)
            where T : Component
        {
            if (component != null)
                return;

            var message = $"[{context.GetType().Name}] Component was not found on this object.";
        }
    }
}
