using UnityEngine;

namespace Risk.Runtime.Utils
{
    public static class DependencyValidator
    {
        public static void NotNull<T>(T value, Object context)
            where T : Object
        {
            if (value != null)
                return;

            var message = $"[{context.GetType().Name}] Required reference is not assigned.";
            Debug.LogError(message, context);
            throw new MissingReferenceException(message);
        }
        
        public static void ComponentExist<T>(T component, Object context)
            where T : Component
        {
            if (component != null)
                return;
            
            var message = $"[{context.GetType().Name}] Component was not found on this object.";
        }
    }
}
