using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Trichome.Web {
    /// <summary>
    /// In this scope, a new instance will be resolved for each ASP.NET request.
    /// </summary>
    public class RequestScope : IScope {
        private const string HTTP_CONTEXT_KEY = "Trichome.Web.RequestScope";

        public object Resolve(Type type, Creator creator) {
            var context = HttpContext.Current;
            var instances = context.Items[HTTP_CONTEXT_KEY]
                as ConcurrentDictionary<Type, object>;
            if (instances == null) {
                instances = new ConcurrentDictionary<Type, object>();
                context.Items.Add(HTTP_CONTEXT_KEY, instances);
                context.AddOnRequestCompleted(DisposeRequest);
            }
            return instances.GetOrAdd(type, t => creator.Create());
        }

        private void DisposeRequest(HttpContext context) {
            var instances = context.Items[HTTP_CONTEXT_KEY]
                as ConcurrentDictionary<Type, object>;
            foreach (var reference in instances.Values) {
                var disposable = reference as IDisposable;
                if (disposable != null) {
                    disposable.Dispose();
                }
            }
            instances.Clear();
            context.Items.Remove(HTTP_CONTEXT_KEY);
        }

        public void Dispose() { }
    }
}
