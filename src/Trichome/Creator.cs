using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class Creator {
        Registration registration;
        Container container;
        ConstructorInfo ctor;
        ParameterInfo[] parameters;
        Func<object[], object> create;

        internal Creator(Registration registration, Container container) {
            this.registration = registration;
            this.container = container;
        }

        public object Create() {
            if (registration.Resolution == Resolution.Cached) {
                return registration.CachedInstance;
            }
            if (registration.Resolution == Resolution.Factory) {
                return registration.Factory();
            }
            if (ctor == null) {
                ctor = GetConstructor(registration.InstanceType);
                parameters = ctor.GetParameters();
                create = BuildCreateFunction(ctor);
            }
            var arguments = new object[parameters.Length];
            for (var p = 0; p < parameters.Length; p++) {
                arguments[p] = container.Resolve(parameters[p].ParameterType);
            }
            return create(arguments);
        }

        static ConstructorInfo GetConstructor(Type type) {
            var ctor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
            if (ctor == null) {
                throw new Exception(string.Format(
                    "Could not find a public constructor for the type '{0}'.",
                    type.FullName));
            }
            return ctor;
        }

        static Func<object[], object> BuildCreateFunction(ConstructorInfo ctor) {
            // http://stackoverflow.com/questions/2353174/c-sharp-emitting-dynamic-method-delegate-to-load-parametrized-constructor-proble
            var method = new DynamicMethod(".ctor", ctor.DeclaringType, new Type[] { typeof(object[]) });
            var il = method.GetILGenerator();
            var parameters = ctor.GetParameters();
            for (var p = 0; p < parameters.Length; p++) {
                il.Emit(OpCodes.Ldarg_0);
                switch (p) {
                    case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                    case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                    case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                    case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                    case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                    case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                    case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                    case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                    case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                    default: il.Emit(OpCodes.Ldc_I4, p); break;
                }
                il.Emit(OpCodes.Ldelem_Ref);
                var type = parameters[p].ParameterType;
                il.Emit(type.IsValueType
                    ? OpCodes.Unbox_Any
                    : OpCodes.Castclass, type);
            }
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            return (Func<object[], object>)
                method.CreateDelegate(typeof(Func<object[], object>));
        }
    }
}
